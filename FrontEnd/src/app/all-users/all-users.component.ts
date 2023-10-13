import { Component, OnDestroy, OnInit } from '@angular/core';
import { IUser } from '../interfaces/IUser';
import { UserService } from '../services/user.service';
import { Subscription } from 'rxjs';
import { SnackBarService } from '../services/snack-bar.service';
import { MatDialog } from '@angular/material/dialog';
import { EditUserDialogComponent } from './edit-user-dialog/edit-user-dialog.component';
import { UserOrdersDialogComponent } from '../user-orders-dialog/user-orders-dialog.component';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.css']
})
export class AllUsersComponent implements OnInit, OnDestroy {

  allUsers: IUser[] = [];

  deleteUserSub!: Subscription;
  enableDelete: boolean = false;

  constructor(private userService: UserService, private snackBar: SnackBarService, private dialog: MatDialog) { }

  ngOnDestroy(): void {
    if (this.deleteUserSub) {
      this.deleteUserSub.unsubscribe();
    }
  }

  displayedColumns: string[] = ['id', 'userType', 'email', 'name', 'surname', 'phoneNumber', 'orders', 'edit', 'delete'];

  ngOnInit(): void {
    this.userService.getAllUsers().subscribe(result => {
      this.allUsers = result;
    })
  }

  deleteUserById(userId: number) {
    this.deleteUserSub = this.userService.deleteUserById(userId).subscribe((result) => {
      this.snackBar.openSnackBar("Usunięto konto");
      this.allUsers = this.allUsers.filter(user => user.id !== userId);
    },
      (error) => {
        this.snackBar.openSnackBar("Nie udało się usunąć konta");

      });
  }

  editUser(user: IUser) {

    const dialogRef = this.dialog.open(EditUserDialogComponent, {
      data: { user: user },
    });
  }

  getUserType(userType: number) {
    switch (userType) {
      case 0:
        return 'Administrator';
      case 1:
        return 'Pracownik'
      case 2:
        return 'Użytkownik'
      default:
        return 'Nie zdefiniowano'
    }
  }

  userOrders(userId: number) {
    const dialogRef = this.dialog.open(UserOrdersDialogComponent, {
      data: { userId: userId },
    });
  }

}
