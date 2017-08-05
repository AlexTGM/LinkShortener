import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'
 
import { AuthenticationService } from '../_services/authentication.service';
import { Link } from '../_models/index';
 
@Injectable()
export class LinksService {
    constructor(
        private http: Http,
        private authenticationService: AuthenticationService) {
    }

    private get header(): RequestOptions {
        let headers = new Headers({ 
            'Authorization': 'Bearer ' + this.authenticationService.token,
            'Content-Type': 'application/json' });

        return new RequestOptions({ headers: headers });
    }

    getFullLink(shortLink: string): Observable<string> {
        return this.http.post('http://localhost:50110/api/shortened?shortLink=' + shortLink, this.header)
            .map((response: Response) => response.json());
    }

    createLink(fullLink: any): Observable<any> {
        return this.http.post('http://localhost:50110/api/shortened', JSON.stringify(fullLink), this.header)
            .map((response: Response) => response.json());
    }
 
    getLinks(): Observable<Link[]> {
        return this.http.get('http://localhost:50110/api/shortened', this.header)
            .map((response: Response) => response.json());
    }
}