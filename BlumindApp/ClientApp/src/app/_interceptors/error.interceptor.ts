import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpEvent, HttpHandler } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError(err => {
            if (err.status === 401 || err.status === 403) {
                this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
                return throwError(err.error);
            }

            if (err.status === 404) {
                this.router.navigate(['/404']);
            }

            if (err.error.error === 'invalid_grant') {
            } else {
                if (err.error.Message.includes(';')) {
                    const errors = err.error.Message.split(';');
                    errors.forEach(message => {
                    });
                } else {
                }
            }
            return throwError(err.error);
        }));
    }

}
