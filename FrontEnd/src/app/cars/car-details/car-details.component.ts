import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { CarInfo } from 'src/app/models/cars';
import { RentComponent } from 'src/app/rent/rent.component';
import { EditCarDialogComponent } from './edit-car-dialog/edit-car-dialog.component';
import { CarService } from 'src/app/services/car.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { HttpResponse } from '@angular/common/http';
import { OrderService } from 'src/app/services/order.service';
import { UserService } from 'src/app/services/user.service';
import * as moment from 'moment';

export interface orderDates {
  startDate: string;
  endDate: string;
}

@Component({
  selector: 'app-car-details',
  templateUrl: './car-details.component.html',
  styleUrls: ['./car-details.component.css']
})
export class CarDetailsComponent implements OnInit {
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
  };

  isAdmin: boolean = false;

  disabledDates: orderDates[] = [];
  photos: string[] = [];
  photosObj: any;
  dialogRef!: MatDialogRef<EditCarDialogComponent>;

  carId: number | undefined;

  isImagesLoading = true;
  isMainImageLoading = true;
  currentPhotoIndex: number = 0;

  constructor(
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private carService: CarService,
    private snackBar: SnackBarService,
    private orderService: OrderService,
    private userService: UserService) {
    this.getCarIdFromQuery();
  }

  ngOnInit(): void {
    this.getAllPhotos();
    this.isAdmin = (this.userService.user.userType == "admin" || parseInt(this.userService.user.userType) == 0) || (this.userService.user.userType == "employee" || parseInt(this.userService.user.userType) == 1) || (this.userService.user.email != "" && parseInt(this.userService.user.userType) == 0);
  }

  getCarIdFromQuery() {
    let carIdFromQuery = this.route.snapshot.paramMap.get('id');
    if (carIdFromQuery && parseInt(carIdFromQuery)) {
      if (carIdFromQuery != parseInt(carIdFromQuery).toString()) {
        throw new Error;
      }
      this.car.id = parseInt(carIdFromQuery);
      this.getRentInfo(this.car.id);
      this.carService.getCarById(parseInt(carIdFromQuery)).subscribe(car => {
        this.car = car;
      });
    }
  }

  getAllPhotos() {
    this.isImagesLoading = true;
    if (this.car && this.car.id != 0) {
      this.carService.getCarImages(this.car.id).subscribe(result => {
        this.car.photos = result;
        this.photos = Object.values(result);
        this.photosObj = result;
        this.isImagesLoading = false;
      },
        (error) => {
        });
    }
  }

  reserveCar(carId: number) {
    const dialogRef = this.dialog.open(RentComponent, {
      data: {
        carId: carId,
        pricePerDay: this.car.pricePerDay,
        disabledDates: this.disabledDates
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.startDate != null && result.endDate) {
        this.disabledDates.push({ startDate: moment(result.startDate).utcOffset('+0100').format('YYYY-MM-DDTHH:mm'), endDate: moment(result.endDate).utcOffset('+0100').format('YYYY-MM-DDTHH:mm') });
      }
    }, (error) => {

    });
  }

  getRentInfo(carId: number) {
    this.orderService.getCarOrders(carId).subscribe(result => {
      result.forEach(order => {
        if (!order.cancelationTime) {
          this.disabledDates.push({ startDate: order.rentStart, endDate: order.rentEnd });
        }
      });
    }, (error) => {

    });
  }

  editCar(car: CarInfo) {
    this.dialogRef = this.dialog.open(EditCarDialogComponent, {
      data: {
        car: car
      },
    });

    this.dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.car = result.data;
        this.snackBar.openSnackBar('Udało zmienić się dane samochodu', '', true);
      }
    }, (error) => {
    });
  }

  previousPhoto() {
    this.currentPhotoIndex = (this.currentPhotoIndex - 1 + this.photos.length) % this.photos.length;
  }

  nextPhoto() {
    this.currentPhotoIndex = (this.currentPhotoIndex + 1) % this.photos.length;
  }

  getFuelTypeName(fuelType: number) {
    switch (fuelType) {
      case 0:
        return 'Benzyna';
      case 1:
        return 'Diesel';
      case 2:
        return 'LPG';
      case 3:
        return 'Elektryczny';
      case 4:
        return 'Wodorowy';
      default:
        return 'Brak informacji'
    }
  }

  deleteCar(carId: number) {
    this.carService.deleteCar(carId).subscribe(result => {
      this.snackBar.openSnackBar("Pomyślnie usunięto samochód", '', true);
      this.dialogRef.close();
    },
      (error) => {
        this.snackBar.openSnackBar("Nie udało się usunąć samochodu");
      });
  }
}
