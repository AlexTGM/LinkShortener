import { Component, OnInit } from '@angular/core';

import { Observable } from 'rxjs';
 
import { LinksService, LinksPaginationService } from '../_services/index';
import { Link } from '../_models/index';
 
@Component({
    moduleId: module.id,
    templateUrl: 'links.component.html',
    styles: [`
    p {
        font-family: Consolas, monaco, monospace;
        font-size: 14px;
        font-style: normal;
        font-variant: normal;
        font-weight: 400;
        line-height: 20px;
    }
    `]
})
 
export class LinksComponent implements OnInit {
    constructor(private paginationService: LinksPaginationService) { }
 
    ngOnInit() {
        this.paginationService.init(0, 10);
    }

    getPage(page: number) {
        this.paginationService.getPage(page);
    }
}