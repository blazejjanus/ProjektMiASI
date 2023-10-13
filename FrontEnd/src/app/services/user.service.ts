import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { IUser } from '../interfaces/IUser';
import { JwtService } from './jwt.service';
import { Router } from '@angular/router';
import { SnackBarService } from './snack-bar.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  userInfo: IUser = {
    email: '',
    name: '',
    surname: '',
    password: '',
    telephone: '',
    address: {
      city: '',
      street: '',
      postalCode: '',
      houseNumber: ''
    },
    id: 0,
    userType: ''
  };

  get user() {
    return this.userInfo;
  }

  setUser(newUserInfo: IUser) {
    this.userInfo = newUserInfo;
  }

  constructor(
    private http: HttpClient,
    private jwt: JwtService,
    private route: Router,
    private snackBar: SnackBarService
  ) { }

  private API_URL = environment.API_URL;

  getUserByEmail(email: string) {
    if (email) {
      const headers = {
        'jwt': this.jwt.getJwt() || ""
      };
      if (headers.jwt) {
        this.http.get<IUser>(this.API_URL + 'User/GetUserByEmail/' + `${email}`, { headers }).subscribe(result => {
          this.userInfo = result;
        });
      }
    }
  }

  updateUserInfo(userToUpdate: IUser) {
    const headers = {
      'jwt': this.jwt.getJwt() || ""
    };

    return this.http.put(this.API_URL + 'User/UpdateUser/', userToUpdate, { headers });
  }

  deleteUser() {
    const headers = {
      'jwt': this.jwt.getJwt() || ""
    };

    if (this.user.email) {
      this.http.delete(this.API_URL + 'User/DeleteUserByEmail/' + `${this.user.email}`, { headers }).subscribe(result => {
        if (result) {
          this.jwt.removeJwt();
          this.route.navigate(['/cars']);
          this.snackBar.openSnackBar("Usunięto konto");
        }
      });
    }
  }

  deleteUserById(userId: number) {
    const headers = {
      'jwt': this.jwt.getJwt() || ""
    };

    return this.http.delete(this.API_URL + 'User/DeleteUserById/' + `${userId}`, { headers });
  }

  getAllUsers() {
    const headers = {
      'jwt': this.jwt.getJwt() || ""
    };
    return this.http.get<IUser[]>(this.API_URL + 'User/GetAllUsers', { headers: headers });
  }

  setUserInfoIfWasInLocalStorage() {
    if (this.jwt.getJwt()) {
      if (!this.user.email) {
        this.getUserByEmail(localStorage.getItem("email") || "");
      }
    }
  }

  setUserType(type: string) {
    this.userInfo.userType = type;
  }
}
