import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OrderService } from '../services/order.service';
import { SnackBarService } from '../services/snack-bar.service';

@Component({
  selector: 'app-user-orders-dialog',
  templateUrl: './user-orders-dialog.component.html',
  styleUrls: ['./user-orders-dialog.component.css']
})
export class UserOrdersDialogComponent implements OnInit {

  isHistory: boolean = false;
  orders: any;
  historyOrders: any;

  displayedColumns: string[] = ['brand', 'model', 'startDate', 'endDate', 'pricePerDay', 'price', 'cancel'];

  constructor(
    private orderService: OrderService,
    private snackBar: SnackBarService,
    private dialogRef: MatDialogRef<UserOrdersDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public user: { userId: number }
  ) { }

  ngOnInit(): void {
    this.getUserOrders(this.user.userId);
  }

  getUserOrders(userId: number) {
    if (userId != 0) {
      this.orderService.getUserOrdersByUserId(userId).subscribe(result => {
        this.historyOrders = result.filter(x => !!x.cancelationTime);
        this.orders = result.filter(x => !x.cancelationTime);

      }, (error) => {
        if (error.status == 404) {
          this.snackBar.openSnackBar(`Użytkownik o ID: ${userId} nie ma jeszcze żadnych zamówień`);
        }
        else {
          this.snackBar.openSnackBar(`Wystąpiły problemy przy pobieraniu danych użytkownika z ID: ${userId}`);
        }
        this.dialogRef.close();
      });
    }
  }

  calculatePrice(price: number, startDate: any, endDate: any) {
    if (startDate && endDate && price) {
      return (Math.ceil((new Date(endDate).getTime() - new Date(startDate).getTime()) / (1000 * 3600 * 24)) + 1) * price;
    }
    else {
      return price;
    }
  }

  cancelOrder(orderId: number) {
    this.orderService.cancelOrder(orderId).subscribe(result => {
      this.snackBar.openSnackBar("Pomyślnie anulowano zamówienie", "", true);
    },
      (error) => {
        this.snackBar.openSnackBar("Nie udało się anulować zamówienia - zgłoś ten fakt pod numerem: 123 456 789");
      });
  }

}
