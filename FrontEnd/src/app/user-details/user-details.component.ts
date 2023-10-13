import { Component, OnInit } from '@angular/core';
import { InputFormControlsService } from '../services/input-form-controls.service';
import { InputErrorStateMatcher } from '../services/InputErrorStateMatcher';
import { UserService } from '../services/user.service';
import { IUser } from '../interfaces/IUser';
import { SnackBarService } from '../services/snack-bar.service';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {

  editedUser!: IUser;
  enableDelete: boolean = false;

  constructor(private userService: UserService,
    private snackBar: SnackBarService) {

  }

  inputsFormControl = new InputFormControlsService();
  matcher = new InputErrorStateMatcher();

  ngOnInit(): void {
    this.editedUser = { ...this.userService.user };
  }

  updateUser(email: string, password: string, name: string, surname: string, telephone: string, city: string, postalCode: string, street: string, houseNumber: string) {
    if (email != this.editedUser.email || !password || name != this.editedUser.name || surname != this.editedUser.surname || telephone != this.editedUser.telephone || city != this.editedUser.address.city || postalCode != this.editedUser.address.postalCode || street != this.editedUser.address.street || houseNumber != this.editedUser.address.houseNumber) {

      let newUser: IUser = {
        id: this.editedUser.id,
        email: email,
        name: name,
        surname: surname,
        password: password,
        userType: this.editedUser.userType,
        telephone: telephone,
        address: {
          city: city,
          street: street,
          postalCode: postalCode,
          houseNumber: houseNumber
        }
      }

      this.userService.updateUserInfo(newUser).subscribe(result => {
        this.userService.setUser(newUser);
        this.snackBar.openSnackBar("Pomyślnie zaktualizowano dane użytkownika", "", true);
      }, (error) => {
        this.snackBar.openSnackBar("Niestety nie udało się zmienić danych użytkownika");
      });
    }

  }

  deleteUser() {
    this.userService.deleteUser();
  }

}
