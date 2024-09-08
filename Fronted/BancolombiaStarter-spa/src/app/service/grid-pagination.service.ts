import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GridPaginationService {
  private skip: number = 0;
  private take: number = 10;

  constructor() {}
  
  getSkip(): number {
    return this.skip;
  }

  getTake(): number {
    return this.take;
  }

  updatePagination(skip: number, take: number) {
    this.skip = skip;
    this.take = take;
  }
}
