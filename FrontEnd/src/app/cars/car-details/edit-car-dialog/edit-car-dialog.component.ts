import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CarInfo } from 'src/app/models/cars';
import { InputErrorStateMatcher } from 'src/app/services/InputErrorStateMatcher';
import { CarService } from 'src/app/services/car.service';
import { InputFormControlsService } from 'src/app/services/input-form-controls.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';

@Component({
  selector: 'app-edit-car-dialog',
  templateUrl: './edit-car-dialog.component.html',
  styleUrls: ['./edit-car-dialog.component.css']
})
export class EditCarDialogComponent implements OnInit {

  editedCar: any;

  isMainPhoto: boolean = false;
  file: any;
  matcher = new InputErrorStateMatcher();
  inputsFormControl = new InputFormControlsService();


  constructor(
    @Inject(MAT_DIALOG_DATA) public car: { car: CarInfo },
    private carService: CarService,
    private snackBar: SnackBarService,
    private dialogRef: MatDialogRef<EditCarDialogComponent>) {
    this.editedCar = { ...car.car };
  }

  ngOnInit(): void {

  }

  editCar(price: string, brand: string, model: string, year: string, horsePower: string, engineCapacity: string, registration: string, seats: string, shortDescription: string, longDescription: string) {
    let newCar = {
      id: this.editedCar.id,
      brand: brand,
      model: model,
      seatsNumber: seats,
      registrationNumber: registration,
      productionYear: parseInt(year),
      horsepower: parseInt(horsePower),
      engineCapacity: parseInt(engineCapacity),
      fuelType: this.editedCar.fuelType,
      shortDescription: shortDescription,
      longDescription: longDescription,
      pricePerDay: price,
      isMain: this.isMainPhoto
    }

    this.carService.updateCar(newCar).subscribe(result => {
      this.snackBar.openSnackBar("Pomyślnie zaktualizowano dane dotyczące samochodu", "", true);
      this.dialogRef.close({ data: newCar });

    }, (error) => {
      this.snackBar.openSnackBar("Nie udało zaktualizować danych dotyczących samochodu");
    });

  }

  onFileChange(event: any) {
    this.file = event.target.files[0];
  }

  addNewPhoto() {
    if (this.file) {
      this.carService.addNewPhoto(this.editedCar.id, this.file);
    }
  }

  removePhoto(photoId: any) {
    let photo = parseInt(photoId);
    if (photo) {
      this.carService.removePhoto(photo).subscribe(result => {
        this.snackBar.openSnackBar("Pomyślnie usunięto zdjęcie", "", true);
      },
        (error) => {
          this.snackBar.openSnackBar("Wystąpiły problemy z usuniecięm zdjęcia");
        });
    }
  }

}
