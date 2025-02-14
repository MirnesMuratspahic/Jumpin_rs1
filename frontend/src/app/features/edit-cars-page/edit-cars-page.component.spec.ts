import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCarsPageComponent } from './edit-cars-page.component';

describe('EditCarsPageComponent', () => {
  let component: EditCarsPageComponent;
  let fixture: ComponentFixture<EditCarsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditCarsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditCarsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
