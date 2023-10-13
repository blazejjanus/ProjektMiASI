import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../services/order.service';
import { IOrder } from '../interfaces/order';
import { SnackBarService } from '../services/snack-bar.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  isHistory: boolean = false;
  orders: any;
  historyOrders: any;

  displayedColumns: string[] = ['brand', 'model', 'startDate', 'endDate', 'pricePerDay', 'price', 'cancel'];

  constructor(private route: ActivatedRoute,
    private orderService: OrderService,
    private snackBar: SnackBarService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const historyParam = params['history'];
      if (historyParam) {
        this.isHistory = true;
      } else {
        this.isHistory = false;
      }
    });

    this.getData();
  }

  getData() {
    if (this.isHistory) {
      this.orderService.getUserOrders().subscribe(result => {
        this.historyOrders = result.filter(x => !!x.cancelationTime);
      }, (errors) => {
        this.snackBar.openSnackBar("Wystąpił błąd przy pobieraniu danych dotyczących zamówień");
      })
    } else {
      this.orderService.getUserOrders().subscribe(result => {
        this.orders = result.filter(x => !x.cancelationTime);
      }, (errors) => {
        this.snackBar.openSnackBar("Wystąpił błąd przy pobieraniu danych dotyczących zamówień");

      })
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
