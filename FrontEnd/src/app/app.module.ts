import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { NavbarComponent } from './navbar/navbar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatSidenavModule } from '@angular/material/sidenav';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { CarsComponent } from './cars/cars.component';
import { RentComponent } from './rent/rent.component';
import { ContactComponent } from './contact/contact.component';
import { RulesComponent } from './rules/rules.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { CardComponent } from './cars/card/card.component';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ErrorComponent } from './error/error.component';
import { CarDetailsComponent } from './cars/car-details/car-details.component';
import { GlobalErrorHandlerService } from './services/global-error-handler-service.service';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule } from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { UserInfoComponent } from './user-info/user-info.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { HttpClientModule } from '@angular/common/http';
// import { AuthInterceptorProvider } from './interceptors/auth.interceptor';
import { UserDetailsComponent } from './user-details/user-details.component';
import { OrdersComponent } from './orders/orders.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthGuard } from './services/authGuard';
import { EditCarDialogComponent } from './cars/car-details/edit-car-dialog/edit-car-dialog.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { AllUsersComponent } from './all-users/all-users.component';
import { MatTableModule } from '@angular/material/table';
import { EditUserDialogComponent } from './all-users/edit-user-dialog/edit-user-dialog.component';
import { AddCarDialogComponent } from './cars/add-car-dialog/add-car-dialog.component';
import { NgxMatDatetimePickerModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { UserOrdersDialogComponent } from './user-orders-dialog/user-orders-dialog.component';

const MY_DATE_FORMAT = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY'
  }
};

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    MenuComponent,
    CarsComponent,
    RentComponent,
    ContactComponent,
    RulesComponent,
    LoginComponent,
    RegisterComponent,
    CardComponent,
    ErrorComponent,
    CarDetailsComponent,
    UserInfoComponent,
    UserDetailsComponent,
    OrdersComponent,
    EditCarDialogComponent,
    AllUsersComponent,
    EditUserDialogComponent,
    AddCarDialogComponent,
    UserOrdersDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    MatFormFieldModule,
    MatSelectModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatListModule,
    MatCardModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    HttpClientModule,
    MatTabsModule,
    MatSnackBarModule,
    MatCheckboxModule,
    MatTableModule,
    NgxMatDatetimePickerModule,
    NgxMatTimepickerModule
  ],
  providers: [
    // AuthInterceptorProvider,
    AuthGuard,
    Permissions,
    { provide: ErrorHandler, useClass: GlobalErrorHandlerService },
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMAT },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
