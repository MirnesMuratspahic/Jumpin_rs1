import { Component, OnInit } from '@angular/core';
import { RoutesServiceService } from '../routes/services/routes-service.service';
import { AuthService } from '../../../services/auth.service';
import { dtoUserRoute } from '../routes/models/dtoUserRoute.model';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { Coordinates } from '../routes/models/coordinates.model';

@Component({
  selector: 'app-route-form',
  imports: [FormsModule, CommonModule],
  standalone: true,
  templateUrl: './route-form.component.html',
  styleUrls: ['./route-form.component.scss'],
})
export class RouteFormComponent implements OnInit {
  coordinatesCreated = false;
  coordinates: Coordinates[] = [];
  newRoute: dtoUserRoute = {
    email: '',
    name: '',
    coordinates: [],
    seatsNumber: 0,
    dateAndTime: '',
    price: 0,
    description: '',
    type: '',
  };
  response: string = '';

  constructor(
    private routesService: RoutesServiceService,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.coordinates = [];
    this.coordinatesCreated = false;
    const storedCoordinates = localStorage.getItem('coordinates');
    if (storedCoordinates) {
      this.coordinates = JSON.parse(storedCoordinates);
      this.coordinatesCreated = true;
    } else {
      this.coordinates = [];
    }
  }

  navigateToCoordinates() {
    this.router.navigate(['/google-maps-page']);
  }

  submitNewRoute(): void {
    const currentUserEmail = this.authService.getEmailFromToken();
    localStorage.removeItem('coordinates');
    if (currentUserEmail) {
      this.newRoute.email = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }
    this.newRoute.coordinates = this.coordinates;

    this.routesService.addRoute(this.newRoute).subscribe(
      (apiResponse: string) => {
        this.response = apiResponse;
        this.toastr.success(this.response);
        this.coordinates = [];
        this.coordinatesCreated = false;
        this.router.navigate(['/home']);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }
}
