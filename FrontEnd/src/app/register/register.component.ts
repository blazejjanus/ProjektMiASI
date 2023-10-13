import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { InputErrorStateMatcher } from '../services/InputErrorStateMatcher';
import { LoginService } from '../services/login.service';
import { IRegister } from '../interfaces/iregister';
import { InputFormControlsService } from '../services/input-form-controls.service';
import { SnackBarService } from '../services/snack-bar.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  matcher = new InputErrorStateMatcher();

  inputsFormControl = new InputFormControlsService();

  constructor(
    private loginService: LoginService,
    private snackBar: SnackBarService,
    private auth: AuthService) { }

  ngOnInit(): void {
  }

  register(email: string, password: string, name: string, surname: string, telephone: string, city: string, postalCode: string, street: string, houseNumer: string) {
    let newUser = {
      id: 0,
      email: email,
      name: name,
      surname: surname,
      password: password,
      userType: 2,
      phoneNumber: telephone,
      address: {
        street: street,
        houseNumber: houseNumer,
        postalCode: postalCode.toString(),
        city: city
      },
      isDeleted: false
    }

    this.auth.register(newUser);
  }

  checkValidForm(): boolean {
    return this.inputsFormControl.emailFormControl.invalid || this.inputsFormControl.passwordFormControl.invalid || this.inputsFormControl.nameFormControl.invalid || this.inputsFormControl.surnameFormControl.invalid || this.inputsFormControl.telephoneFormControl.invalid || this.inputsFormControl.cityFormControl.invalid || this.inputsFormControl.postalCodeFormControl.invalid || this.inputsFormControl.streetFormControl.invalid || this.inputsFormControl.houseNumberControl.invalid;
  }

}
