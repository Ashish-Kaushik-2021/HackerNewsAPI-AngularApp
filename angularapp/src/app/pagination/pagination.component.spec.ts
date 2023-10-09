import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { PaginationComponent } from './pagination.component';
import { HackerNewsService } from '../services/HackerNewsService';

describe('PaginationComponent', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    declarations: [PaginationComponent],
    providers: [HackerNewsService]
  })
  );

  it('should create the pagination component', () => {
    const fixture = TestBed.createComponent(PaginationComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });


});
