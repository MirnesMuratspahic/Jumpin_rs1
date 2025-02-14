import { Component } from '@angular/core';
import { NavbarComponent } from '../../core/components/navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { RequestsPageService } from './services/requests-page.service';
import { FormsModule } from '@angular/forms';
import { dtoRequestAcceptOrDecline } from './models/dtoRequestAcceptOrDecline.model';
import { error } from 'console';
import { UserCar } from '../home-page/models/userCar.model';
import { CarRequest } from './models/carRequest.model';
import { RouteRequestComponent } from '../../core/components/route-request/route-request.component';
import { CarRequestComponent } from '../../core/components/car-request/car-request.component';
import { FlatRequestComponent } from '../../core/components/flat-request/flat-request.component';

@Component({
    selector: 'app-requests-page',
    imports: [
        NavbarComponent,
        CommonModule,
        FormsModule,
        RouteRequestComponent,
        CarRequestComponent,
        FlatRequestComponent
    ],
    templateUrl: './requests-page.component.html',
    styleUrl: './requests-page.component.scss'
})
export class RequestsPageComponent {
  activeTab: 'routes' | 'cars' | 'flats' = 'routes';
  activeSubTab: 'received' | 'sent' = 'received'

  constructor(
    private authService: AuthService,
    private requestService: RequestsPageService
  ) {}

  setActiveTab(tab: any) {
    this.activeTab = tab;
  }

  setActiveSubTab(subTab: any) {
    this.activeSubTab = subTab;
  }
}
