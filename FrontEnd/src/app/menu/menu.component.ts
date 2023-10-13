import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  constructor(private auth: AuthService, private user: UserService) { }

  isLoggedIn: boolean = false;

  ngOnInit(): void {
    this.auth.iisLoggedIn$.subscribe(isLogged => {
      this.isLoggedIn = isLogged;
    })
  }

  logout() {
    this.auth.logout();
  }

}
