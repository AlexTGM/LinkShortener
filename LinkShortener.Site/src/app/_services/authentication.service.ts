import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response, URLSearchParams } from '@angular/http';
import { Observer } from 'rxjs/Observer';
import { Observable } from 'rxjs/Observable'
import 'rxjs/add/operator/do'

import { tokenNotExpired } from 'angular2-jwt';

@Injectable()
export class AuthenticationService {
    private tokenEndpoint = 'http://localhost:50110/connect/token';
    private registerEndpoint = 'http://localhost:50110/api/account/register';

    constructor(private http: Http) { }

    get loggedIn() : boolean {
        return tokenNotExpired();
    }

    get username() : string {
        return localStorage.getItem('current_user');
    }

    login(username: string, password: string) {
        const headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        const options = new RequestOptions({ headers: headers })

        const body = new URLSearchParams();

        body.set("username", username);
        body.set('password', password);
        body.set('grant_type', "password");
        body.set('scope', "offline_access openid");

        return this.http.post(this.tokenEndpoint, body.toString(), options)
                        .do(res => {
                            var json = res.json();

                            localStorage.setItem('current_user', username);
                            localStorage.setItem('token', json.id_token);
                            localStorage.setItem('refresh_token', json.refresh_token);
                        });
    }

    register(username: string, password: string) : Observable<Response> {
        return this.http.post(this.registerEndpoint, {username, password});
    }

    logout() {
        localStorage.removeItem('current_user');
        localStorage.removeItem('token'); 
        localStorage.removeItem('refresh_token'); 
    }
}