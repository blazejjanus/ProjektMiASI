import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CarInfo } from 'src/app/models/cars';
import { InputErrorStateMatcher } from 'src/app/services/InputErrorStateMatcher';
import { CarService } from 'src/app/services/car.service';
import { InputFormControlsService } from 'src/app/services/input-form-controls.service';

@Component({
  selector: 'app-add-car-dialog',
  templateUrl: './add-car-dialog.component.html',
  styleUrls: ['./add-car-dialog.component.css']
})
export class AddCarDialogComponent implements OnInit {

  newCar: CarInfo = {
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
    photos: undefined
  }

  matcher = new InputErrorStateMatcher();
  inputsFormControl = new InputFormControlsService();

  constructor(private carService: CarService,
    private dialogRef: MatDialogRef<AddCarDialogComponent>) { }

  ngOnInit(): void {
  }

  addCar(price: string, brand: string, model: string, year: string, horsePower: string, engineCapacity: string, registration: string, seats: string, shortDescription: string, longDescription: string) {
    let newCar = {
      id: 0,
      brand: brand,
      model: model,
      seatsNumber: seats,
      registrationNumber: registration,
      productionYear: parseInt(year),
      horsepower: parseInt(horsePower),
      engineCapacity: parseInt(engineCapacity),
      fuelType: this.newCar.fuelType,
      shortDescription: shortDescription,
      longDescription: longDescription,
      pricePerDay: price,
    }

    this.carService.addNewCar(newCar).subscribe(result => {
      this.dialogRef.close(true);
    },
      (error) => {
        this.dialogRef.close();
      });
  }

}
