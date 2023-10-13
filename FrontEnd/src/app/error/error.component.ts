import { Component, OnInit } from '@angular/core';
import { SnackBarService } from '../services/snack-bar.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {

  constructor(
    private snackBar: SnackBarService,
    private router: Router
  ) {
    this.snackBar.openSnackBar("Nie udało się wykonać operacji");
  }

  ngOnInit(): void {
    this.router.navigate(['/cars']);
  }

}
