import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions, URLSearchParams } from '@angular/http';
import { Observer } from 'rxjs/Observer';
import { Observable } from 'rxjs/Observable'
import 'rxjs/add/operator/map'

import { tokenNotExpired } from 'angular2-jwt';

@Injectable()
export class AuthenticationService {
    private tokenEndpoint = 'http://localhost:50110/connect/token';

    constructor(private http: Http) { }

    get loggedIn() : boolean {
        return tokenNotExpired();
    }

    get username() : string {
        return localStorage.getItem('current_user');
    }

    login(username: string, password: string) : Observable<any> {
        const headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        const options = new RequestOptions({ headers: headers })

        const body = new URLSearchParams();

        body.set("username", username);
        body.set('password', password);
        body.set('grant_type', "password");
        body.set('scope', "offline_access openid");

        return new Observable<any>((observer: Observer<any>)  => {
            this.http.post(this.tokenEndpoint, body.toString(), options)
                .map(res => res.json())
                .subscribe(res => {
                    localStorage.setItem('current_user', username);
                    localStorage.setItem('token', res.id_token);
                    localStorage.setItem('refresh_token', res.refresh_token);

                    observer.next(res);
                    observer.complete();
                });
        });
    }

    logout() {
        localStorage.removeItem('current_user');
        localStorage.removeItem('token'); 
        localStorage.removeItem('refresh_token'); 
    }
}