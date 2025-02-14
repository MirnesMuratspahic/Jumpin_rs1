import { Component, OnInit, Input } from '@angular/core';
import { dtoUserRoute } from './models/dtoUserRoute.model';
import { dtoRequest } from './models/dtoRequest.model';
import { AuthService } from '../../../services/auth.service';
import { RoutesServiceService } from './services/routes-service.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProfilePageService } from '../../../features/profile-page/services/profile-page.service';
import { User } from './models/user.model';
import { UserRoute } from './models/userRoute';
import { HeaderService } from '../../../services/headerService/header.service';
import { ToastrService } from 'ngx-toastr';
import { RequestsPageService } from '../../../features/requests-page/services/requests-page.service';
import { Coordinates } from './models/coordinates.model';
import { VoiceRecognitionService } from '../../../services/voiceService/voice-service.service';
import { NgZone } from '@angular/core';

@Component({
  selector: 'app-routes',
  imports: [CommonModule, FormsModule],
  standalone: true,
  templateUrl: './routes.component.html',
  styleUrls: ['./routes.component.scss'],
})
export class RoutesComponent implements OnInit {
  @Input() apiUrl!: string;

  response: string = '';
  isModalOpen: boolean = false;
  user: User | null = null;
  routes: UserRoute[] = [];
  cars: UserRoute[] = [];
  flats: UserRoute[] = [];
  userEmail: string = '';
  searchTerm: string = ''; 
  filteredRoutes: UserRoute[] = [];
  isApiRequestInProgress = false;
  isListening = false;

  newRoute: dtoUserRoute = {
    email: '',
    name: '',
    seatsNumber: 0,
    coordinates: [],
    dateAndTime: '',
    price: 0,
    description: '',
    type: '',
  };

  newRequest: dtoRequest = {
    passengerEmail: '',
    description: '',
    status: '',
    objectId: 0,
    objectType: '',
  };

  constructor(
    private routesService: RoutesServiceService,
    private authService: AuthService,
    private router: Router,
    private profilePageService: ProfilePageService,
    private headerService: HeaderService,
    private toastr: ToastrService,
    private requestService: RequestsPageService,
    private voiceService: VoiceRecognitionService,
    private ngZone: NgZone
  ) {}

  ngOnInit(): void {
    const currentUserEmail = this.authService.getEmailFromToken();
    localStorage.removeItem('coordinates');
    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    if (this.apiUrl === 'https://localhost:7172/Route/GetRoutes') {
      this.getRoutes();
    } else if (this.apiUrl === 'https://localhost:7172/Route/GetUsersRoutes') {
      this.getUsersRoutes();
    }
  }
  
  filterRoutes(): void {
    if (this.searchTerm) {
      const keywords = this.searchTerm
        .toLowerCase()
        .replace(/[^a-zA-Z0-9\s]/g, '')
        .split(' ')
        .filter(keyword => keyword);
  
      this.filteredRoutes = this.routes.filter(route => {
        const routeName = route.route.name.toLowerCase().replace(/[^a-zA-Z0-9\s]/g, '');
  
        return keywords.every(keyword => routeName.includes(keyword));
      });
    } else {
      this.filteredRoutes = [...this.routes];
    }
    this.filterVIPAds();
  }

  filterVIPAds(){
    this.filteredRoutes = this.filteredRoutes.sort((a, b) => {
      if (a.user.accountType === 'VIP' && b.user.accountType !== 'VIP') {
        return -1; 
      } else if (a.user.accountType !== 'VIP' && b.user.accountType === 'VIP') {
        return 1; 
      }
      return 0;
    });
  }
  
  navigateToRating(email : string) : void {
    localStorage.setItem('Email', email);
    this.router.navigate(['/rating']);
  }

  getRoutes(): void {
    this.routes = [];
    this.routesService.getRoutes().subscribe(
      (data: UserRoute[]) => {
        this.routes = data;
        this.filterRoutes();
      },
      (error) => {
        console.error('Error fetching routes:', error);
      }
    );
  }

  openModal(): void {
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.clearNewRoute();
  }

  sendRequest(item: any): void {
    if (this.isApiRequestInProgress) {
      return;
    }
    this.isApiRequestInProgress = true; 
    this.newRequest.passengerEmail = this.userEmail;
    this.newRequest.status = 'Pending';
    this.newRequest.objectId = item.route.id;
    this.newRequest.objectType = 'Route';
    this.newRequest.description = 'None';

    this.requestService.sendRequest(this.newRequest).subscribe(
      (apiResponse: string) => {
        this.response = apiResponse;
        this.getRoutes();
        this.toastr.success(this.response);
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
    setTimeout(() => {
      this.isApiRequestInProgress = false;
    }, 5000);
  }

  startVoiceSearch() {
    this.isListening = true;
    this.voiceService.startListening((transcript) => {
      this.ngZone.run(() => {
        this.searchTerm = transcript;
      });
    });
    setTimeout(() => {
      this.voiceService.stopListening();
      this.isListening = false;
      this.filterRoutes();
    }, 5000);
  }

  submitNewRoute(): void {
    this.routesService.addRoute(this.newRoute).subscribe(
      (apiResponse: any) => {
        this.response = apiResponse;
        this.getRoutes();
        this.toastr.success(this.response);
        this.closeModal();
      },
      (error: any) => {
        this.toastr.error(error);
      }
    );
  }

  clearNewRoute(): void {
    this.newRoute = {
      email: '',
      name: '',
      seatsNumber: 0,
      coordinates: [],
      dateAndTime: '',
      price: 0,
      description: '',
      type: '',
    };
  }

  getUsersRoutes() {
    this.routes = [];
    this.routesService.getUsersRoutes(this.userEmail).subscribe(
      (data: UserRoute[]) => {
        this.routes = data;
        this.filterRoutes(); 
      },
      (error) => {
        console.error('Error fetching routes:', error);
      }
    );
    this.cars = [];
    this.flats = [];
  }


  onDeleteRoute(routeId: number) {
    this.routesService.deleteRoute(routeId).subscribe(
      (data: string) => {
        this.routes = this.routes.filter((route) => route.route.id !== routeId);
        this.getUsersRoutes();
        this.toastr.success(data);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  onEditRoute(routeId: number): void {
    this.router.navigate(['/edit-route', routeId]);
  }

  viewRouteOnMap(coordinates: Coordinates[]): void {
    localStorage.setItem('coordinates', JSON.stringify(coordinates));
    this.router.navigate(['/view-maps-page']);
  }
}
