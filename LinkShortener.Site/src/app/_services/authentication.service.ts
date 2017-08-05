import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
 
@Injectable()
export class AuthenticationService {
    public token: string;
 
    constructor(private http: Http) {
        var currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.token = currentUser && currentUser.token;
    }
 
    login(username: string, password: string): Observable<boolean> {
        const headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' }); 
        const options = new RequestOptions({ headers: headers })

        const body = new URLSearchParams();

        body.set("username", username);
        body.set('password', password);
        body.set('grant_type', "password");
        body.set('scope', "email");

        return this.http.post('http://localhost:50110/connect/token', body.toString(), options)
            .map((response: Response) => {
                let access_token = response.json() && response.json().access_token;
                if (access_token) {
                    this.token = access_token;
 
                    localStorage.setItem('currentUser', JSON.stringify({ username: username, token: access_token }));
 
                    return true;
                } else { return false; }
            });
    }
 
    logout(): void {
        this.token = null;
        localStorage.removeItem('currentUser');
    }
}