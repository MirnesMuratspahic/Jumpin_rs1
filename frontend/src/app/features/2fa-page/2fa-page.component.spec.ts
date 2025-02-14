import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoFaCodePageComponent } from './2fa-page.component';

describe('CodePageComponent', () => {
  let component: TwoFaCodePageComponent;
  let fixture: ComponentFixture<TwoFaCodePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TwoFaCodePageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TwoFaCodePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
