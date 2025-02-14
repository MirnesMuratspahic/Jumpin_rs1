import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VipPageAdminComponent } from './vip-page-admin.component';

describe('VipPageAdminComponent', () => {
  let component: VipPageAdminComponent;
  let fixture: ComponentFixture<VipPageAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VipPageAdminComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VipPageAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
