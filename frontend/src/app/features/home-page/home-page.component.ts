import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { RoutesComponent } from '../../core/components/routes/routes.component';
import { NavbarComponent } from '../../core/components/navbar/navbar.component';
import { CarsComponent } from '../../core/components/cars/cars.component';
import { FlatsComponent } from '../../core/components/flats/flats.component';

@Component({
    selector: 'app-home-page',
    templateUrl: './home-page.component.html',
    styleUrls: ['./home-page.component.scss'],
    imports: [
        CommonModule,
        RouterModule,
        RoutesComponent,
        NavbarComponent,
        RoutesComponent,
        CarsComponent,
        FlatsComponent,
    ]
})
export class HomePageComponent {
  constructor(private router: Router) {}
  currentSection: 'routes' | 'cars' | 'flats' = 'routes';

  showSection(section: 'routes' | 'cars' | 'flats') {
    this.currentSection = section;
  }

  addAddLink() {
    this.router.navigate(['/add-page']);
  }
}
