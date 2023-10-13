import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { InputErrorStateMatcher } from '../services/InputErrorStateMatcher';
import { LoginService } from '../services/login.service';
import { ILogin } from '../interfaces/ILogin';
import { AuthService } from '../services/auth.service';
import { InputFormControlsService } from '../services/input-form-controls.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  matcher = new InputErrorStateMatcher();
  inputsFormControl = new InputFormControlsService();

  constructor(private auth: AuthService) { }

  ngOnInit(): void {
  }

  login(username: string, password: string) {
    if (username != null && password != null) {
      let loginData: ILogin = {
        username,
        password
      };

      this.auth.login(loginData);
    }
  }

}
