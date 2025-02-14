import { TestBed } from '@angular/core/testing';

import { EditRouteService } from './edit-route.service';

describe('EditRouteService', () => {
  let service: EditRouteService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EditRouteService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
