import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { CarsComponent } from './cars/cars.component';
import { RentComponent } from './rent/rent.component';
import { ContactComponent } from './contact/contact.component';
import { RulesComponent } from './rules/rules.component';
import { ErrorComponent } from './error/error.component';
import { CarDetailsComponent } from './cars/car-details/car-details.component';
import { UserInfoComponent } from './user-info/user-info.component';
import { UserDetailsComponent } from './user-details/user-details.component';
import { OrdersComponent } from './orders/orders.component';
import { AuthGuard } from './services/authGuard';
import { AllUsersComponent } from './all-users/all-users.component';
import { AuthAdminGuard } from './services/authAdminGuard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'cars', component: CarsComponent },
  { path: 'cars/:id', component: CarDetailsComponent },
  { path: 'me', component: UserInfoComponent, canActivate: [AuthGuard] },
  { path: 'contact', component: ContactComponent },
  { path: 'rules', component: RulesComponent },
  { path: 'me/info', component: UserDetailsComponent, canActivate: [AuthGuard] },
  { path: 'me/orders', component: OrdersComponent, canActivate: [AuthGuard] },
  { path: 'me/orders/:history', component: OrdersComponent, canActivate: [AuthGuard] },
  { path: 'rules', component: RulesComponent },
  { path: 'allUsers', component: AllUsersComponent, canActivate: [AuthAdminGuard] },
  { path: '404', component: ErrorComponent },
  { path: '', component: CarsComponent, pathMatch: 'full' },
  { path: '**', component: ErrorComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
