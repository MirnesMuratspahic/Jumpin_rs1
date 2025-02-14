import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VipPageComponent } from './vip-page.component';

describe('VipPageComponent', () => {
  let component: VipPageComponent;
  let fixture: ComponentFixture<VipPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VipPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VipPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
