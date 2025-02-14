import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlatFormComponent } from './flat-form.component';

describe('FlatFormComponent', () => {
  let component: FlatFormComponent;
  let fixture: ComponentFixture<FlatFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlatFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FlatFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
