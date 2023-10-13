import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ILogin } from '../interfaces/ILogin';
import { environment } from 'src/environments/environment.prod';
import { IRegister } from '../interfaces/iregister';

@Injectable({
  providedIn: 'root'
})
export class LoginService implements OnInit {

  constructor(private http: HttpClient) { }

  private API_URL = environment.API_URL;

  ngOnInit(): void {

  }

  post(path: string, data: any): Observable<any> {
    return this.http.post(this.API_URL + path, null, { headers: data, responseType: 'text' });
  }

  login(loginData: ILogin): Observable<any> {
    const headers = new HttpHeaders({
      'email': loginData.username,
      'password': loginData.password
    });
    return this.post('Login/Login', headers);
  }

  register(user: any): Observable<any> {
    return this.http.post(this.API_URL + "Login/Register", user, { responseType: 'text' });
  }

}
