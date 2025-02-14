import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouteFormComponent } from '../../core/components/route-form/route-form.component';
import { CarsFormComponent } from '../../core/components/cars-form/cars-form.component';
import { FlatFormComponent } from '../../core/components/flat-form/flat-form.component';

@Component({
  selector: 'app-add-page',
  imports: [
    FormsModule,
    CommonModule,
    RouteFormComponent,
    CarsFormComponent,
    FlatFormComponent,
  ],
  templateUrl: './add-page.component.html',
  styleUrls: ['./add-page.component.scss'],
})
export class AddPageComponent {
  constructor(private router: Router) {}

  selectedForm: 'cars' | 'routes' | 'flats' = 'routes';

  showCarsForm() {
    this.selectedForm = 'cars';
  }

  showRoutesForm() {
    this.selectedForm = 'routes';
  }

  showFlatForm() {
    this.selectedForm = 'flats';
  }

  goHome() {
    this.router.navigate(['/home']);
  }
}
