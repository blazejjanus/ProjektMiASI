import { Component, Input, OnInit } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { CarInfo } from 'src/app/models/cars';
import { CarService } from 'src/app/services/car.service';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {

  @Input() car: CarInfo | undefined;

  isImagesLoading: boolean = true;

  constructor(private carService: CarService, private router: Router) { }

  ngOnInit(): void {
    if (this.car) {
      this.carService.getMainCarPhoto(this.car.id).subscribe(result => {
        if (this.car) {
          this.car.mainPhoto = "data:image/jpeg;base64," + result.toString();
          this.isImagesLoading = false;
        }
      },
        (error) => {

        });
    }
  }

  openCarDetails() {
    this.router.navigateByUrl(`/cars/${this.car?.id}`);
  }

  getFuelTypeName(fuelType: number) {
    switch (fuelType) {
      case 0:
        return 'Benzyna';
      case 1:
        return 'Diesel';
      case 2:
        return 'LPG';
      case 3:
        return 'Elektryczny';
      case 4:
        return 'Wodorowy';
      default:
        return 'Brak informacji'
    }
  }

}
