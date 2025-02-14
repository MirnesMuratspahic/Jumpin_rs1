import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-google-maps',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './google-maps.component.html',
  styleUrl: './google-maps.component.scss',
})
export class GoogleMapsComponent implements OnInit {
  @ViewChild('googleMap', { static: false }) googleMapElement!: ElementRef;
  center = { lat: 43.8486, lng: 18.3564 };
  zoom = 12;

  markerPositions: google.maps.LatLngLiteral[] = [];
  map!: google.maps.Map;
  directionsService!: google.maps.DirectionsService;
  directionsRenderer!: google.maps.DirectionsRenderer | null;
  markers: google.maps.Marker[] = [];
  buttonStatus = false;

  constructor(private router: Router, private cdRef: ChangeDetectorRef) {
    this.directionsRenderer = null;
  }

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    this.map = new google.maps.Map(this.googleMapElement.nativeElement, {
      center: this.center,
      zoom: this.zoom,
    });

    this.directionsService = new google.maps.DirectionsService();

    this.map.addListener('click', (event: google.maps.MapMouseEvent) => {
      if (event.latLng) {
        this.addMarker(event.latLng);
      }
    });
  }

  addMarker(latLng: google.maps.LatLng | google.maps.LatLngLiteral): void {
    const lat =
      latLng instanceof google.maps.LatLng ? latLng.lat() : latLng.lat;
    const lng =
      latLng instanceof google.maps.LatLng ? latLng.lng() : latLng.lng;

    this.markerPositions.push({ lat, lng });

    const marker = new google.maps.Marker({
      position: { lat, lng },
      map: this.map,
    });

    this.markers.push(marker);

    if (this.markerPositions.length > 1) {
      this.calculateRoute();
      this.buttonStatus = true;
    }

    this.cdRef.detectChanges();
  }

  calculateRoute(): void {
    if (this.markerPositions.length > 1) {
      const start = this.markerPositions[0];
      const end = this.markerPositions[this.markerPositions.length - 1];

      const waypoints = this.markerPositions.slice(1, -1).map((position) => ({
        location: position,
        stopover: true,
      }));

      const request: google.maps.DirectionsRequest = {
        origin: start,
        destination: end,
        waypoints: waypoints,
        travelMode: google.maps.TravelMode.DRIVING,
      };

      if (this.directionsRenderer) {
        this.directionsRenderer.setMap(null);
      }

      this.directionsRenderer = new google.maps.DirectionsRenderer();
      this.directionsRenderer.setMap(this.map);

      this.directionsService.route(request, (response, status) => {
        if (status === google.maps.DirectionsStatus.OK) {
          this.directionsRenderer!.setDirections(response);
        } else {
          console.error('Directions request failed due to ' + status);
        }
      });
    }
  }

  saveRouteCoordinates(): void {
    console.log(this.markerPositions);
    const routeCoordinates = this.markerPositions;
    if (routeCoordinates != null) {
      localStorage.setItem('coordinates', JSON.stringify(routeCoordinates));
      this.router.navigate(['/add-page']);
    }
  }

  clearAll(): void {
    this.buttonStatus = false;
    this.markers.forEach((marker) => marker.setMap(null));
    this.markers = [];
    this.markerPositions = [];

    if (this.directionsRenderer) {
      this.directionsRenderer.setMap(null);
      this.directionsRenderer = null;
    }
  }

  goHome() {
    this.router.navigate(['/home']);
  }
}
