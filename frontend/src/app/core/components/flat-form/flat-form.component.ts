import { Component } from '@angular/core';
import { FlatsService } from '../flats/services/flats.service';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-flat-form',
    imports: [CommonModule, FormsModule],
    templateUrl: './flat-form.component.html',
    styleUrl: './flat-form.component.scss'
})
export class FlatFormComponent {
  newFlat = {
    email: '',
    name: '',
    dateAndTime: '',
    price: '',
    description: '',
    type: '',
  };
  response: string = '';

  constructor(
    private flatService: FlatsService,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  submitNewFlat(): void {
    const currentUserEmail = this.authService.getEmailFromToken();

    if (currentUserEmail) {
      this.newFlat.email = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    this.newFlat.dateAndTime = new Date(this.newFlat.dateAndTime).toISOString();

    this.flatService.addFlat(this.newFlat).subscribe(
      (apiResponse: string) => {
        this.response = apiResponse;
        this.toastr.success(apiResponse);
        this.router.navigate(['/home']);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }
}
