import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {
  @Input() totalItems = 0;
  @Input() itemsPerPage = 10;
  @Output() pageChange = new EventEmitter<number>();

  pages: number[] = [];
  currentPage = 1;

  ngOnInit() {
    this.calculatePages();
  }

  // get total pages for pagination
  calculatePages() {
    const pageCount = Math.ceil(this.totalItems / this.itemsPerPage);
    this.pages = Array.from({ length: pageCount }, (_, i) => i + 1);
  }

  changePage(page: number) {
    if (this.currentPage !== page) {
      this.currentPage = page;
      this.pageChange.emit(page);
    }
  }
}
