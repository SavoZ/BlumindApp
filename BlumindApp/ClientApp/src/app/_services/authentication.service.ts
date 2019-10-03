import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { AUTH_ENDPOINT } from '../_constants/url.constants';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  baseUrl;
  constructor(private http: HttpClient, private router: Router) {
  }

  login(username: string, password: string, redirectUrl: string) {
    const body = this.createBody(username, password);
    const headers = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    localStorage.removeItem('appUser');
    this.http.post<any>(AUTH_ENDPOINT + '/connect/token', body, { headers: headers }).subscribe(data => {
      if (data && data.access_token) {
        this.http.get<any>(AUTH_ENDPOINT + '/connect/userinfo', {
          headers: new HttpHeaders().set('Authorization', `Bearer ${data.access_token}`)
        }).subscribe(uData => {
          if (uData && uData.sub) {
            const param = {
              'token': data.access_token,
              'id': uData.sub,
              'roles': uData.role
            };
            localStorage.setItem('appUser', JSON.stringify(param));
            this.router.navigate([redirectUrl]);
          }
        }, uError => {
          alert('Unable to login. Please check your username or password');
        });
      }
    }, error => {
      // alert('An error occured');
    });
  }

  logout() {
    localStorage.removeItem('appUser');
    this.router.navigate(['/login']);
  }

  private createBody(username: string, password: string) {
    // tslint:disable-next-line:max-line-length
    return `username=${username}&password=${password}&grant_type=password&client_id=app-client&client_secret=app-client&scope=openid profile email app-api roles`;
  }
}
