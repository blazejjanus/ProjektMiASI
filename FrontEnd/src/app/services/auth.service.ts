import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { LoginService } from './login.service';
import { ILogin } from '../interfaces/ILogin';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { UserService } from './user.service';
import { JwtService } from './jwt.service';
import { IRegister } from '../interfaces/iregister';
import { SnackBarService } from './snack-bar.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _isLoggedIn$ = new BehaviorSubject<boolean>(false);
  iisLoggedIn$ = this._isLoggedIn$.asObservable();

  get token() {
    return this.jwtService.getJwt();
  }

  constructor(
    private loginService: LoginService,
    private route: Router,
    private userService: UserService,
    private jwtService: JwtService,
    private snackBar: SnackBarService
  ) {
    this._isLoggedIn$.next(!!this.token);
  }

  login(loginData: ILogin) {
    return this.loginService.login(loginData).subscribe(response => {
      if (response) {
        let json = this.getDecodedAccessToken(response);
        if (json.role == "customer" || json.role === "2") {
          this.userService.setUserType("customer");
        }
        else if (json.role == "employee" || json.role === "1") {
          this.userService.setUserType("employee");
        }
        else if (json.role == "admin" || json.role === "0") {
          this.userService.setUserType("admin");
        }

        this._isLoggedIn$.next(true);
        this.jwtService.setJwt(response);
        this.userService.getUserByEmail(json.email);
        localStorage.setItem("email", json.email);
        this.route.navigate(['/me']);
      }
    });
  }

  register(registerData: any) {
    return this.loginService.register(registerData).subscribe(result => {
      let json = this.getDecodedAccessToken(result);
      if (json.role == "customer") {
        this.userService.setUserType("customer");
      }
      else if (json.role == "employee") {
        this.userService.setUserType("employee");
      }
      else if (json.role == "admin") {
        this.userService.setUserType("admin");
      }

      this._isLoggedIn$.next(true);
      this.jwtService.setJwt(result);
      this.userService.getUserByEmail(json.email);
      this.snackBar.openSnackBar('Pomyślnie zarejestrowano', "", true);
      this.route.navigate(['/cars']);
    },
      error => {
        console.error('Error:', error);
        this.snackBar.openSnackBar('Nie udało się zarejestrować');
      });
  }

  logout() {
    this.jwtService.removeJwt();

    this.userService.setUserType("customer");
    this._isLoggedIn$.next(false);
  }

  getDecodedAccessToken(token: string): any {
    try {
      return jwtDecode(token);
    } catch (Error) {
      this.snackBar.openSnackBar("Wystąpił problem z zalogowaniem - spróbuj ponownie");
      return null;
    }
  }
}
