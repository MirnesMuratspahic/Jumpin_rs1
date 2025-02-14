import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LandingSlideShowComponent } from './landing-slide-show.component';

describe('LandingSlideShowComponent', () => {
  let component: LandingSlideShowComponent;
  let fixture: ComponentFixture<LandingSlideShowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LandingSlideShowComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LandingSlideShowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
