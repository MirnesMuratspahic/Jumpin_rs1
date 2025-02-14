import { TestBed } from '@angular/core/testing';

import { RequestsPageService } from './requests-page.service';

describe('RequestsPageService', () => {
  let service: RequestsPageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RequestsPageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
