import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RouteRequestComponent } from './route-request.component';

describe('RouteRequestComponent', () => {
  let component: RouteRequestComponent;
  let fixture: ComponentFixture<RouteRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouteRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RouteRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
