import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { AddCarDialogComponent } from '../cars/add-car-dialog/add-car-dialog.component';
import { UserService } from '../services/user.service';
import { SnackBarService } from '../services/snack-bar.service';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {

  isAdmin: boolean = false;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private userService: UserService,
    private snackBarService: SnackBarService
  ) { }

  ngOnInit(): void {
    this.isAdmin = (this.userService.user.userType == "admin" || parseInt(this.userService.user.userType) == 0) || (this.userService.user.userType == "employee" || parseInt(this.userService.user.userType) == 1) || (this.userService.user.email != "" && parseInt(this.userService.user.userType) == 0);
  }

  navigate(url: string) {
    this.router.navigateByUrl('/me/' + url);
  }

  navigateToUsers() {
    this.router.navigate(['/allUsers']);
  }

  openAddCar() {
    const dialogRef = this.dialog.open(AddCarDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.snackBarService.openSnackBar("Samochód został dodany!", "", true);
      }
      else {
        this.snackBarService.openSnackBar("Nie udało dodać się samochodu");
      }
    });
  }

}
