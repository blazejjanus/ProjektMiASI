import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { UserService } from './user.service';

@Injectable({
    providedIn: 'root'
})
export class Permissions {

    constructor(private route: Router, private auth: AuthService, private userService: UserService) {

    }

    canActivate(token: string, id: string): boolean {
        if ((token && this.userService.user.userType == "ADMIN" || parseInt(this.userService.user.userType) == 0) || (this.userService.user.userType == "EMPLOYEE" || parseInt(this.userService.user.userType) == 1)) {
            return true;
        }
        else {
            this.route.navigate(['/cars']);
            return false;
        }
    }
}

@Injectable({
    providedIn: 'root'
})
export class AuthAdminGuard implements CanActivate {
    constructor(private permissions: Permissions, private currentUser: AuthService) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        return this.permissions.canActivate(this.currentUser.token || "", route.params['id'] || "");
    }
}