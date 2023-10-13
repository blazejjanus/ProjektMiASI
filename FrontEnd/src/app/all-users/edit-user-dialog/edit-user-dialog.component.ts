import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IUser } from 'src/app/interfaces/IUser';
import { InputErrorStateMatcher } from 'src/app/services/InputErrorStateMatcher';
import { InputFormControlsService } from 'src/app/services/input-form-controls.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-edit-user-dialog',
  templateUrl: './edit-user-dialog.component.html',
  styleUrls: ['./edit-user-dialog.component.css']
})
export class EditUserDialogComponent implements OnInit {

  editedUser!: IUser

  matcher = new InputErrorStateMatcher();
  inputsFormControl = new InputFormControlsService();

  constructor(private userService: UserService, @Inject(MAT_DIALOG_DATA) public user: { user: IUser }, private snackBar: SnackBarService) {
    this.editedUser = { ...user.user };
  }

  ngOnInit(): void {
  }

  updateUser(email: string, password: string, name: string, surname: string, telephone: string, city: string, postalCode: string, street: string, houseNumber: string) {
    if (email != this.editedUser.email || this.user.user.userType != this.editedUser.userType || !password || name != this.editedUser.name || surname != this.editedUser.surname || telephone != this.editedUser.telephone || city != this.editedUser.address.city || postalCode != this.editedUser.address.postalCode || street != this.editedUser.address.street || houseNumber != this.editedUser.address.houseNumber) {

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
          postalCode: postalCode.toString(),
          houseNumber: houseNumber
        }
      }

      this.userService.updateUserInfo(newUser).subscribe(result => {
        this.snackBar.openSnackBar("Pomyślnie zaktualizowano dane", "", true);
      }, (error) => {
        if (error.status == 403) {
          this.snackBar.openSnackBar("Niestety nie masz uprawnień do tego modułu - skontaktuj się z administratorem");
        }
        else {
          this.snackBar.openSnackBar("Nie udało się zaktualizować danych");
        }
      });
    }

  }

}
