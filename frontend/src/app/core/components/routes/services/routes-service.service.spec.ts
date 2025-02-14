import { TestBed } from '@angular/core/testing';

import { RoutesServiceService } from './routes-service.service';

describe('RoutesServiceService', () => {
  let service: RoutesServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoutesServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
