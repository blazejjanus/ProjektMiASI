
import { Injectable } from '@angular/core';
import { CarInfo } from '../models/cars';
import { environment } from 'src/environments/environment.prod';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { JwtService } from './jwt.service';
import { UserService } from './user.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CarService {

  photoToUpload: any;

  car: CarInfo = {
    id: 0,
    brand: '',
    model: '',
    fuelType: 0,
    horsepower: 0,
    productionYear: 0,
    engineCapacity: 0,
    shortDescription: '',
    longDescription: '',
    seatsNumber: 0,
    registrationNumber: '',
    productionDate: '',
    pricePerDay: 0,
    mainPhoto: '',
    photos: {
      id: 0,
      photo: ''
    }
  }

  private API_URL = environment.API_URL;
  constructor(private http: HttpClient, private jwt: JwtService, private userService: UserService) { }

  getCars(count: number, startIndex: number, includeUnoperational: boolean = false) {

    const headers = new HttpHeaders({
      'includeUnoperational': includeUnoperational.toString(),
      'count': count.toString(),
      'startIndex': startIndex.toString()
    });

    return this.http.get<CarInfo[]>(this.API_URL + 'CarManagement/GetAllCars', { headers });
  }

  getMainCarPhoto(id: number) {
    return this.http.get(this.API_URL + `Image/GetCarMainImage/${id}`, { responseType: 'text' });
  }

  getCarById(id: number) {
    return this.http.get<CarInfo>(this.API_URL + `CarManagement/GetCarByID/${id}`);
  }

  getCarImages(carId: number) {
    return this.http.get(this.API_URL + `Image/GetCarImages/${carId}`);
  }

  rentCar(carId: number, startDate: string, endDate: string) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    let order = {
      id: 0,
      customer: {
        id: this.userService.userInfo.id,
        email: "string",
        name: "string",
        surname: "string",
        password: "Qwerty123!@#",
        userType: 0,
        phoneNumber: "string",
        address: {
          street: "string",
          houseNumber: "string",
          postalCode: "string",
          city: "string"
        },
        isDeleted: false
      },
      car: {
        id: carId,
        brand: "string",
        model: "string",
        registrationNumber: "string",
        seatsNumber: 5,
        isOperational: true,
        productionYear: 0,
        horsepower: 0,
        engineCapacity: 0,
        fuelType: 0,
        shortDescription: "string",
        longDescription: "string",
        pricePerDay: 0,
        isDeleted: false
      },
      rentStart: startDate,
      rentEnd: endDate
    }

    return this.http.post(this.API_URL + `Orders/PostOrder`, order, { headers });
  }

  updateCar(car: any) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    return this.http.put(this.API_URL + `CarManagement/UpdateCar`, car, { headers });
  }

  addNewPhoto(carId: number, file: File) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    if (file) {
      const formData: FormData = new FormData();
      formData.append('file', file, file.name);

      this.http.post(this.API_URL + `Image/AddCarImage/${carId}`, formData, { headers }).subscribe(x => {

      });
    }

  }

  removePhoto(photoId: number) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    return this.http.delete(this.API_URL + `Image/DeleteImage/${photoId}`, { headers });
  }

  addNewCar(newCar: any) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    return this.http.post(this.API_URL + `CarManagement/AddCar`, newCar, { headers });
  }

  deleteCar(carId: number) {
    const headers = new HttpHeaders({
      'jwt': this.jwt.getJwt() || ''
    });

    return this.http.delete(this.API_URL + `CarManagement/DeleteCar/${carId}`, { headers });
  }
}
