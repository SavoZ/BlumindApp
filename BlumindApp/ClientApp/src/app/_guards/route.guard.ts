import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HttpBackend } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})

export class RouteGuard implements CanActivate {
    constructor(private router: Router, private handler: HttpBackend) { }

    async canActivate(route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot) {
        const roles = route.data['roles'] as Array<string>;

        return true;
    }
}
