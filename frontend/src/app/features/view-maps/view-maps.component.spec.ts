import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewMapsComponent } from './view-maps.component';

describe('ViewMapsComponent', () => {
  let component: ViewMapsComponent;
  let fixture: ComponentFixture<ViewMapsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewMapsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewMapsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
