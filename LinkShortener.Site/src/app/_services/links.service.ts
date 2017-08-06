import { Injectable } from '@angular/core';
import { Headers, RequestOptions, Response } from '@angular/http';
import { AuthHttp, tokenNotExpired } from 'angular2-jwt';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'

import { Link } from '../_models/index';
 
@Injectable()
export class LinksService {
    constructor(private authHttp: AuthHttp) { }

    getFullLink(shortLink: string): Observable<string> {
        return this.authHttp.get('http://localhost:50110/api/shortened?shortLink=' + shortLink)
            .map((response: Response) => response.json());
    }

    createLink(fullLink: any): Observable<any> {
        return this.authHttp.post('http://localhost:50110/api/shortened', JSON.stringify(fullLink))
            .map((response: Response) => response.json());
    }
 
    getLinks(): Observable<Link[]> {
        return this.authHttp.get('http://localhost:50110/api/shortened')
            .map((response: Response) => response.json());
    }
}