import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  private readonly TOKEN_NAME = 'token';

  constructor() { }

  setJwt(token: any) {
    localStorage.setItem(this.TOKEN_NAME, token);
  }

  getJwt() {
    return localStorage.getItem(this.TOKEN_NAME);
  }

  removeJwt() {
    if (this.getJwt()) {
      localStorage.removeItem(this.TOKEN_NAME);
    }
  }
}
