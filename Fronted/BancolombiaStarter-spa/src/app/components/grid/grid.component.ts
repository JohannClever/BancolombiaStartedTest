import { Component, OnInit ,ChangeDetectorRef, EventEmitter, Output, SimpleChanges, Input } from '@angular/core';
import { GridPaginationService } from 'src/app/service/grid-pagination.service';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {
  @Input() gridName: string;
  @Input() showHeaders: boolean = true;
  @Input() customHeaders: { [key: string]: string };
  @Input() columnData: any[];
  @Input() actions: any[] = [];
  @Input() paginate: boolean;
  @Input() totalRecords: number;
  @Input() hiddenColumns: string[] = [];
  @Input() hasActions: boolean;

  @Output() pageChange = new EventEmitter<void>();

  columnHeaders: string[] = [];
  currentPage = 0;
  pageSize = 10;
  pageNumbers: number[] = [];
  visibleHeaders: string[] = [];

  constructor(private paginationService: GridPaginationService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.columnHeaders = Object.keys(this.columnData[0]);
    console.log("column headers "+this.columnHeaders);
    this.calculateVisibleHeaders();
    if (this.totalRecords != undefined && this.totalRecords > 0) {
      this.pageNumbers = this.getPageNumbers();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.columnData && !changes.columnData.firstChange) {
      if (this.columnData != undefined && this.columnData.length > 0) {
        this.columnHeaders = Object.keys(this.columnData[0]);
        this.calculateVisibleHeaders();
        this.cdr.detectChanges();
      }
    }
    if (changes.totalRecords && !changes.totalRecords.firstChange) {
      if (this.totalRecords != undefined && this.totalRecords > 0) {
        this.pageNumbers = this.getPageNumbers();
      }
    }
  }

  prevPage() {
    if (this.currentPage > 0) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  nextPage() {
    const maxPage = Math.floor(this.totalRecords / this.pageSize);
    if (this.currentPage < maxPage) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  goToPage(pageNumber: number) {
    if (pageNumber >= 0 && pageNumber <= Math.floor(this.totalRecords / this.pageSize)) {
      this.currentPage = pageNumber;
      this.updatePagination();
    }
  }

  getPageNumbers(): number[] {
    const pageCount = Math.ceil(this.totalRecords / this.pageSize) - 1;
    return Array.from({ length: pageCount }, (_, i) => i);
  }

  private updatePagination() {
    const skip = this.currentPage * this.pageSize;
    const take = this.pageSize;
    this.paginationService.updatePagination(skip, take);
    this.pageChange.emit();
  }

  private calculateVisibleHeaders() {
    this.visibleHeaders = this.columnHeaders.filter(header => !this.hiddenColumns.includes(header));
    console.log("headers"+this.customHeaders);
    console.log("hides"+this.hiddenColumns);
    console.log("data"+this.columnData);
    console.log("records"+this.totalRecords);
    
    
  }
  getColspan() {
    return this.visibleHeaders.length + 1 
  }

}
