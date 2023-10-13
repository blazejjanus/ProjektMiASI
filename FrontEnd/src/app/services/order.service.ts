import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { JwtService } from './jwt.service';
import { UserService } from './user.service';
import { IOrder } from '../interfaces/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private API_URL = environment.API_URL;
  constructor(private http: HttpClient, private jwt: JwtService, private userService: UserService) { }

  getUserOrders() {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    let userId = this.userService.user.id;

    return this.http.get<IOrder[]>(this.API_URL + `Orders/GetUserOrdersByID/${userId}`, { headers })
  }

  getUserOrdersByUserId(userId: number) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    return this.http.get<IOrder[]>(this.API_URL + `Orders/GetUserOrdersByID/${userId}`, { headers })
  }

  cancelOrder(orderId: number) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    return this.http.put(this.API_URL + `Orders/CancelOrder/${orderId}`, null, { headers });
  }

  getCarOrders(carId: number) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    return this.http.get<IOrder[]>(this.API_URL + `Orders/GetCarOrdersByID/${carId}`, { headers });
  }
}
