import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {

  constructor(private snackBar: MatSnackBar) { }

  openSnackBar(text: string, close: string = "", success = false) {
    this.snackBar.open(text, close, {
      horizontalPosition: 'center',
      verticalPosition: 'top',
      duration: 2000,
      panelClass: [success ? 'snackbar-green' : 'snackbar-red']
    });
  }


}
