import { Component, HostListener, OnInit } from '@angular/core';
import { CarInfo } from '../models/cars';
import { CarDetailsComponent } from './car-details/car-details.component';
import { NavigationExtras, Router } from '@angular/router';
import { CarService } from '../services/car.service';
import { SnackBarService } from '../services/snack-bar.service';

@Component({
  selector: 'app-cars',
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.css']
})
export class CarsComponent implements OnInit {
  cars: CarInfo[] = [];

  constructor(
    private router: Router,
    private carService: CarService,
    private snackBar: SnackBarService) { }

  ngOnInit(): void {
    this.loadCars();
  }

  loadCars() {
    this.carService.getCars(6, 0, false).subscribe(result => {
      this.cars = result;
    },
      (error) => {
        this.snackBar.openSnackBar("Występują pewnie problemy po naszej stronie. Proszę spróbować później");
      });
  }
}
