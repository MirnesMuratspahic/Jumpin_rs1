import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlatRequestComponent } from './flat-request.component';

describe('FlatRequestComponent', () => {
  let component: FlatRequestComponent;
  let fixture: ComponentFixture<FlatRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlatRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FlatRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
