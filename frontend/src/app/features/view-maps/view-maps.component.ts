import { Component, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-view-maps',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './view-maps.component.html',
  styleUrl: './view-maps.component.scss',
})
export class ViewMapsComponent implements AfterViewInit {
  map!: google.maps.Map;
  directionsRenderer!: google.maps.DirectionsRenderer;
  coordinates: google.maps.LatLng[] = [];

  constructor(private router: Router) {}

  ngAfterViewInit(): void {
    this.initializeMap();
    this.loadRouteCoordinates();
  }

  private initializeMap(): void {
    const mapOptions: google.maps.MapOptions = {
      center: { lat: 0, lng: 0 },
      zoom: 6,
    };

    const mapElement = document.getElementById('googleMap') as HTMLElement;

    if (!mapElement) {
      console.error('Google Map element not found!');
      return;
    }

    this.map = new google.maps.Map(mapElement, mapOptions);
    this.directionsRenderer = new google.maps.DirectionsRenderer();
    this.directionsRenderer.setMap(this.map);
  }

  private loadRouteCoordinates(): void {
    const coordinates = localStorage.getItem('coordinates');
    if (coordinates) {
      this.coordinates = JSON.parse(coordinates).map(
        (coord: { lat: number; lng: number }) =>
          new google.maps.LatLng(coord.lat, coord.lng)
      );
      this.plotRoute();
    }
  }

  private plotRoute(): void {
    if (this.coordinates.length > 1) {
      const directionsService = new google.maps.DirectionsService();

      const request: google.maps.DirectionsRequest = {
        origin: this.coordinates[0],
        destination: this.coordinates[this.coordinates.length - 1],
        travelMode: google.maps.TravelMode.DRIVING,
        waypoints: this.coordinates
          .slice(1, -1)
          .map((coord) => ({ location: coord, stopover: true })),
        optimizeWaypoints: true,
      };

      directionsService.route(request, (response, status) => {
        if (status === google.maps.DirectionsStatus.OK) {
          this.directionsRenderer.setDirections(response);
        } else {
          console.error('Error retrieving directions: ' + status);
        }
      });
    } else {
      console.error('Not enough coordinates to plot the route');
    }
  }

  goHome() {
    this.router.navigate(['/home']);
  }
}
