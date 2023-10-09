import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IHackerNewsStory } from '../interface/IHackerNewsStory';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})

export class HackerNewsService {

    private baseUrl = environment.baseUrl;
    constructor(private httpClient: HttpClient) { }

  getAllStories(searchTerm: string): Observable<IHackerNewsStory[]> {
      return this.httpClient.get<IHackerNewsStory[]>(`${environment.baseUrl}/api/hackernews?searchTerm=${searchTerm}`);
    } 

}
