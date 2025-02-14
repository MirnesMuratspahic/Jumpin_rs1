import { Component } from '@angular/core';
import { CarsService } from '../cars/services/cars-service.service';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-cars-form',
    imports: [FormsModule, CommonModule],
    templateUrl: './cars-form.component.html',
    styleUrls: ['./cars-form.component.scss']
})
export class CarsFormComponent {
  newCar = {
    email: '',
    name: '',
    dateAndTime: '',
    price: '',
    description: '',
    type: '',
  };
  response: string = '';

  constructor(
    private carsService: CarsService,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  submitNewCar(): void {
    const currentUserEmail = this.authService.getEmailFromToken();

    if (currentUserEmail) {
      this.newCar.email = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    this.carsService.addCar(this.newCar).subscribe(
      (apiResponse: string) => {
        this.response = apiResponse;
        this.router.navigate(['/home']);
        this.toastr.success(this.response);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }
}
