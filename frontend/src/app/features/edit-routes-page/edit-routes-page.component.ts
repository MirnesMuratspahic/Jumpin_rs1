import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EditRouteService } from './services/edit-route.service';
import { ToastrService } from 'ngx-toastr';
import { Coordinates } from '../../core/components/routes/models/coordinates.model';

@Component({
    selector: 'app-edit-routes-page',
    imports: [ReactiveFormsModule],
    templateUrl: './edit-routes-page.component.html',
    styleUrls: ['./edit-routes-page.component.scss']
})
export class EditRoutesPageComponent implements OnInit {
  routeId!: string;
  routeForm!: FormGroup;
  coordinates: Coordinates[] = [{ lat: 45.1234, lng: 19.8765 }, { lat: 46.2345, lng: 20.9876 }];
  coordinatesAsString: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private editRouteService: EditRouteService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.routeId = this.route.snapshot.paramMap.get('id')!;

    this.routeForm = this.fb.group({
      email: [{ value: '', disabled: true }],
      name: [''],
      seatsNumber: [''],
      dateAndTime: [''],
      price: [''],
      description: [''],
      type: [''],
      coordinates: [{ value: '', disabled: true }]
    });

    this.loadRouteData();
  }

  loadRouteData(): void {
    this.editRouteService.getRouteDetails(this.routeId).subscribe(
      (routeData) => {
        this.routeForm.patchValue({
          email: routeData.user.email,
          name: routeData.route.name,
          seatsNumber: routeData.route.seatsNumber,
          dateAndTime: routeData.route.dateAndTime,
          price: routeData.route.price,
          coordinates: routeData.route.coordinates,
          description: routeData.route.description,
          type: routeData.route.type,
        });
      },
      (error) => {
        console.error('Error loading route details:', error);
      }
    );
  }

  updateRoute(): void {
    const formData = { ...this.routeForm.getRawValue() };
    const completeData = {
      email: this.routeForm.get('email')?.value,
      coordinates: formData.coordinates,
      ...formData,
    };

    this.editRouteService.updateRoute(this.routeId, completeData).subscribe(
      () => {
        this.router.navigate(['/profile']);
        this.toastr.success("Route updated!");
      },
      (error) => {
        console.log(error);
        this.toastr.error(error.message);
      }
    );
  }
}
