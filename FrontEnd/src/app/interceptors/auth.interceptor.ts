// import { Injectable } from '@angular/core';
// import {
//   HttpRequest,
//   HttpHandler,
//   HttpEvent,
//   HttpInterceptor,
//   HTTP_INTERCEPTORS
// } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { AuthService } from '../services/auth.service';

// @Injectable()
// export class AuthInterceptor implements HttpInterceptor {

//   constructor(private authService: AuthService) { }

//   intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
//     console.log('interceptor sie wrzuca');

//     request = request.clone({
//       headers: this.authService.token ? request.headers.set('authorization', this.authService.token) : request.headers
//     });

//     return next.handle(request);
//   }
// }

// export const AuthInterceptorProvider = {
//   provide: HTTP_INTERCEPTORS,
//   useClass: AuthInterceptor,
//   multi: true
// }
