import { Injectable } from '@angular/core';
import { Headers, RequestOptions, Response } from '@angular/http';
import { AuthHttp, tokenNotExpired } from 'angular2-jwt';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map'

import { Link, PaginatedData } from '../_models/index';
 
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

    getLinksPaginated(skip: number, take: number): Observable<PaginatedData<Link>> {
        return this.authHttp.get("http://localhost:50110/api/shortened?skip=" + skip + "&take=" + take)
            .map((response: Response) => {
                var dataJson = response.json();
                return new PaginatedData<Link>(dataJson);
            });
    }
 
    getLinks(): Observable<Link[]> {
        return this.authHttp.get('http://localhost:50110/api/shortened')
            .map((response: Response) => response.json());
    }
}