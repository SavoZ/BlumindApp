import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class AuthGuard implements CanActivate {
    baseUrl;
    constructor(private router: Router) { }
    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

        if (localStorage.getItem('appUser')) {
            return true;
        }

        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
