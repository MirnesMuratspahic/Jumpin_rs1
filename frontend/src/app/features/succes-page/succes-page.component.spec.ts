import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccesPageComponent } from './succes-page.component';

describe('SuccesPageComponent', () => {
  let component: SuccesPageComponent;
  let fixture: ComponentFixture<SuccesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuccesPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuccesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
