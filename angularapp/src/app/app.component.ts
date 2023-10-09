import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { IHackerNewsStory } from './interface/IHackerNewsStory'
import { HackerNewsService } from './services/HackerNewsService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public stories: IHackerNewsStory[] = [];
  public paginatedStories: IHackerNewsStory[] = [];
  public currentPage = 1;
  public itemsPerPage = 10;
  public totalItems = 0;
  constructor(private http: HttpClient, private _hnService: HackerNewsService)
  {
    this.get("");
  }

  // fetch hacker news all stories
  get(searchTerm: string) {
    this._hnService.getAllStories(searchTerm).subscribe( res => {
      this.stories = res;
      this.totalItems = this.stories.length;
      this.onPageChange();
    });
  }

  // fetch only stories hvaing search term
  search(event: KeyboardEvent) {
    this.get((event.target as HTMLTextAreaElement).value);
  }

  open(url: string) {
    window.open(url, "_blank");
  }

  // set stories according to passed page no.
  onPageChange(page: number = 1) {
    this.currentPage = page;
    const startItem = (page - 1) * this.itemsPerPage;
    const endItem = page * this.itemsPerPage;
    this.paginatedStories = this.stories.slice(startItem, endItem);
  }

}
