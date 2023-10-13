import { ErrorHandler, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SnackBarService } from './snack-bar.service';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandlerService implements ErrorHandler {

  constructor(private routerService: Router, private snackBar: SnackBarService) { }

  handleError(error: any): void {
    if (error) {
      if (error.status == 403) {
        this.snackBar.openSnackBar("Niestety nie masz uprawnień do tego modułu - skontaktuj się z administratorem");
      }
      else if (error.status >= 200 && error.status < 300) {

      }
      else if (error.status != 200) {
        this.routerService.navigateByUrl('/404');
      }
    }
  }
}
