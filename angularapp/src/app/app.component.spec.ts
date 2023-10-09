import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { HackerNewsService } from './services/HackerNewsService';
import { of } from 'rxjs';
import { IHackerNewsStory } from './interface/IHackerNewsStory';

describe('AppComponent', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    declarations: [AppComponent],
    providers: [HackerNewsService]
  })
  );

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should call hacker news stories get function', () => {
    const fakeData: IHackerNewsStory[] = [{ by:"samaysharma",title:"Pythagorean Theorem found on clay",url:"https://test.com" } ]
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    const service = TestBed.inject(HackerNewsService);
    const mySpy = spyOn(service , 'getAllStories').and.returnValue(of(fakeData));
    app.get("");
    expect(mySpy).toHaveBeenCalledTimes(1);

  });
});
