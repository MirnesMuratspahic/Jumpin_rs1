import { TestBed } from '@angular/core/testing';

import { RatingPageService } from './rating-page.service';

describe('RatingPageService', () => {
  let service: RatingPageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RatingPageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
