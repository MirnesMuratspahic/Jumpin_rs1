import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditRoutesPageComponent } from './edit-routes-page.component';

describe('EditRoutesPageComponent', () => {
  let component: EditRoutesPageComponent;
  let fixture: ComponentFixture<EditRoutesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditRoutesPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditRoutesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
