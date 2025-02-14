import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarsFormComponent } from './cars-form.component';

describe('CarsFormComponent', () => {
  let component: CarsFormComponent;
  let fixture: ComponentFixture<CarsFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CarsFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
