import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StripePageComponent } from './stripe-page.component';

describe('StripePageComponent', () => {
  let component: StripePageComponent;
  let fixture: ComponentFixture<StripePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StripePageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StripePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
