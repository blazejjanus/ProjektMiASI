import { Component, OnInit } from '@angular/core';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  ngOnInit(): void {
    this.checkIfUserWasLoggedIn();
  }

  constructor(private userService: UserService) {

  }

  isSideNav: boolean = false;
  title = 'car-rental';
  sidenavOpened = false;

  isToggled(event: any) {
    this.isSideNav = event;
  }

  toggleSidenav() {
    this.sidenavOpened = !this.sidenavOpened;
  }

  checkIfUserWasLoggedIn() {
    this.userService.setUserInfoIfWasInLocalStorage();
  }

}
