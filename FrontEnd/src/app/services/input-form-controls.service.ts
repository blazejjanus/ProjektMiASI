import { Injectable } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class InputFormControlsService {

  emailFormControl = new FormControl('', [Validators.required, Validators.email, Validators.minLength(4), Validators.maxLength(50)]);
  passwordFormControl = new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(32)]);

  nameFormControl = new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(32)]);
  surnameFormControl = new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(32)]);
  telephoneFormControl = new FormControl('', [Validators.required, Validators.min(100000000), Validators.max(999999999999), Validators.maxLength(15), Validators.minLength(9)]);

  cityFormControl = new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(32)]);
  postalCodeFormControl = new FormControl('', [Validators.required, Validators.min(10000), Validators.max(99999), Validators.minLength(5), Validators.maxLength(5)]);
  streetFormControl = new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(32)]);
  houseNumberControl = new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(6)]);

  brandFormControl = new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(32)]);
  modelFormControl = new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(32)]);
  yearFormControl = new FormControl('', [Validators.required, Validators.min(1930), Validators.max(2100)]);
  horsePowerFormControl = new FormControl('', [Validators.required, Validators.min(2), Validators.max(3500)]);
  engineCapacityFormControl = new FormControl('', [Validators.required, Validators.min(2), Validators.max(10000)]);
  shortDescriptionFormControl = new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]);
  longDescriptionFormControl = new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(500)]);
  pricePerDayFormControl = new FormControl('', [Validators.required, Validators.min(0), Validators.max(15000)]);
  seatsFormControl = new FormControl('', [Validators.required, Validators.min(1), Validators.max(100)]);
  registrationFormControl = new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(10)]);


  constructor() { }
}
