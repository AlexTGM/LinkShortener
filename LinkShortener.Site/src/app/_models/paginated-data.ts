export class PaginatedData<T> {
    data: T[];
    totalCount: number;
    page: number;
    size: number;

    constructor(json: any) {
        this.data = json.data;
        this.page = json.page;
        this.size = json.size;
        this.totalCount = json.totalCount;
    }
}