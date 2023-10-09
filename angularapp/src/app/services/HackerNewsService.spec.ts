import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { HackerNewsService } from '../services/HackerNewsService';
import { IHackerNewsStory } from '../interface/IHackerNewsStory';
import { of } from 'rxjs';

describe('HackerNewsService', () => {
    let _hackerNewsServiceSpy: jasmine.SpyObj<HackerNewsService>;
    beforeEach(() => {
        const spy = jasmine.createSpyObj('HackerNewsService', ['getAllStories']);
        TestBed.configureTestingModule({
        imports: [HttpClientTestingModule],
        providers: [ { provide: HackerNewsService, useValue: spy }]
      });
      _hackerNewsServiceSpy = TestBed.inject(HackerNewsService) as jasmine.SpyObj<HackerNewsService>; 
    });

    it('getAllStories should return data', (done) => {
      const expectedData: IHackerNewsStory[] = [
            {   by:"samaysharma",
                title:"Pythagorean Theorem found on clay",
                url:"https://test.com" 
            },
            {   by:"ashish",
                title:"build demo",
                url:"https://test.com" 
            }
        ];
  
        _hackerNewsServiceSpy.getAllStories.and.returnValue(of(expectedData));
        _hackerNewsServiceSpy.getAllStories("").subscribe(data => {
            expect(data).toEqual(expectedData);
            done();
          });
      });
    });
