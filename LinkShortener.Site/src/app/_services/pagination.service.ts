import { LinksService } from './links.service';
import { Link } from '../_models/index';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class LinksPaginationService {
    totalPages: number;
    currentPage: number;
    pageSize: number;
    data: Link[];

    pages: number[] = [];

    constructor(private dataProvider: LinksService) {
        this.pages = [] as number[];
        this.data = [] as Link [];
        this.totalPages = this.currentPage = this.pageSize = 0;
    }

    init(skip: number, take: number) {
        this.dataProvider.getLinksPaginated(skip, take)
            .subscribe(data => {
                this.pages = [] as number[];
                
                this.data = data.data;
                this.currentPage = data.page;
                this.pageSize = data.size;                
                this.totalPages = Math.ceil(data.totalCount / take);

                if (this.pages.length != this.totalPages) {
                    for (var i = 1; i < this.totalPages + 1; i++) {
                        this.pages.push(i);
                    }
                }
            })
    }

    getPage(page: number) {
        this.dataProvider.getLinksPaginated((page - 1) * this.pageSize, this.pageSize)
            .subscribe(data => {
                this.data = data.data;
                this.currentPage = data.page;
            })
    }
}