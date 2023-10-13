import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CarService } from '../services/car.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { orderDates } from '../cars/car-details/car-details.component';
import { SnackBarService } from '../services/snack-bar.service';
import * as moment from 'moment';
import { UserService } from '../services/user.service';
import { JwtService } from '../services/jwt.service';

@Component({
  selector: 'app-rent',
  templateUrl: './rent.component.html',
  styleUrls: ['./rent.component.css']
})
export class RentComponent implements OnInit {
  carId: number | undefined;
  startDate!: Date;
  endDate!: Date;
  price: number = 0;
  now!: Date;
  isNotloggedIn = true;

  filterDates = this.filter.bind(this);


  disabledDates: orderDates[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private carService: CarService,
    private snackBar: SnackBarService,
    private userService: UserService,
    private jwt: JwtService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<RentComponent>) {
    this.disabledDates = this.data.disabledDates;
  }

  ngOnInit(): void {
    this.isNotloggedIn = !(this.userService.user.email && this.jwt.getJwt());
    this.carId = this.data.carId;
    this.now = new Date();
  }

  rent() {
    if (!(this.userService.user.email && this.jwt.getJwt())) {
      this.snackBar.openSnackBar("Aby zarezerwować pojazd najpierw należy się zalogować");
    }
    else {
      if (new Date(this.endDate) > new Date(this.startDate) && this.carId != 0 && this.checkIfDateIsValid()) {
        this.carService.rentCar(this.carId || 0, moment(this.startDate).utcOffset('+0100').format('YYYY-MM-DDTHH:mm'), moment(this.endDate).utcOffset('+0100').format('YYYY-MM-DDTHH:mm')).subscribe(result => {
          this.snackBar.openSnackBar("Pomyślnie udało się złożyć zamówienie", "", true);
          if (this.startDate && this.endDate) {
            this.dialogRef.close({ startDate: this.startDate, endDate: this.endDate });
          }
          else {
            this.dialogRef.close();
          }
        }, (error) => {
          if (error.status >= 200 && error.status < 300) {
            this.snackBar.openSnackBar("Pomyślnie udało się złożyć zamówienie", "", true);
            if (this.startDate && this.endDate) {
              this.dialogRef.close({ startDate: this.startDate, endDate: this.endDate });
            }
            else {
              this.dialogRef.close();
            }
          }
          else {
            this.snackBar.openSnackBar("Coś poszło nie tak, spróbuj ponownie później");
          }
        });
      }
      else {
        this.snackBar.openSnackBar("Wprowadzono niepoprawną datę, spróbuj dobrać inną");
      }
    }

  }

  calculatePrice() {
    if (this.endDate && this.startDate && this.checkIfDateIsValid()) {
      let startDate = new Date(this.startDate).getTime();
      let endDate = new Date(this.endDate).getTime();
      if (startDate < endDate) {
        return (Math.ceil((endDate - startDate) / (1000 * 3600 * 24)) + 1) * this.data.pricePerDay;
      }
    }
    else {
      return this.data.pricePerDay;
    }
  }

  filter(date: Date | null): boolean {
    let newDate = (date || new Date());
    let parsedDate = (new Date(newDate) || new Date()).toDateString();
    if (this.disabledDates && this.disabledDates.length > 0) {
      return !this.disabledDates.some(disabledDate => {
        return Date.parse(parsedDate) >= Date.parse(disabledDate.startDate) &&
          Date.parse(parsedDate) <= Date.parse(disabledDate.endDate);
      });
    }

    return true;
  }

  checkIfDateIsValid(): boolean {
    if (this.startDate && this.endDate) {
      const selectedStartDate = new Date(this.startDate).getTime();
      const selectedEndDate = new Date(this.endDate).getTime();

      const overlappingDates = this.disabledDates.filter(dateRange => {
        const rangeStartDate = new Date(dateRange.startDate).getTime();
        const rangeEndDate = new Date(dateRange.endDate).getTime();

        return (
          (selectedStartDate >= rangeStartDate && selectedStartDate <= rangeEndDate) ||
          (selectedEndDate >= rangeStartDate && selectedEndDate <= rangeEndDate) ||
          (selectedStartDate <= rangeStartDate && selectedEndDate >= rangeEndDate)
        );
      });

      return !(overlappingDates.length > 0);
    }
    else {
      return false;
    }
  }
}
