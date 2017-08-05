import { Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs';
 
import { LinksService } from '../_services/index';
import { Link } from '../_models/index';
 
@Component({
    moduleId: module.id,
    templateUrl: 'links.component.html'
})
 
export class LinksComponent implements OnInit {
    links: Link[] = [];
 
    constructor(private linksService: LinksService) { }
 
    ngOnInit() {
        this.linksService.getLinks()
            .subscribe(l => this.links = l);
    }
}