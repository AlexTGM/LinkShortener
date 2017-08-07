import { Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs';
 
import { LinksService } from '../_services/index';
 
@Component({
    moduleId: module.id,
    templateUrl: 'link-shortener.component.html'
})
 
export class LinkShortenerComponent {
    fullLink: string;
    shortLink: string;
 
    loading: boolean;
    error: string;

    constructor(private linksService: LinksService) { }

    shorten() {
        this.loading = true;

        this.linksService.createLink(this.fullLink)
            .subscribe(l => {
                this.shortLink = l.shortLink;
                this.loading = false;
            });
    }
}