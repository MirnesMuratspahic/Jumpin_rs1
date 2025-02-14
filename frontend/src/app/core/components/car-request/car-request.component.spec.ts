import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarRequestComponent } from './car-request.component';

describe('CarRequestComponent', () => {
  let component: CarRequestComponent;
  let fixture: ComponentFixture<CarRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CarRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
