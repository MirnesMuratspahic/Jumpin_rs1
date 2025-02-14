import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { AppComponent } from '../../app.component';
import { PasswordResetService } from './service/password-reset-page.service';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { error } from 'console';
import { CommonModule } from '@angular/common';
import { registerLoginBackground } from '../../constants/imagesConstants';


@Component({
  selector: 'app-code-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './password-reset-page.component.html',
  styleUrl: './password-reset-page.component.scss'
})
export class PasswordResetPage implements OnInit {

  userEmail: string = '';
  isResendDisabled: boolean = false;
  timer: number = 60;
  backgroundImage = `url(${registerLoginBackground})`;
  newPassword: string = '';
  confirmPassword: string = '';

  constructor(private passwordResetService: PasswordResetService, private authService: AuthService, 
    private toastr: ToastrService, private router: Router, private route: ActivatedRoute){}

  ngOnInit(): void {
    const currentUserEmail = this.authService.getEmailFromToken();

    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }
  }

  onSubmit() {

    this.passwordResetService.resetPassword(this.newPassword, this.userEmail).subscribe(
      (response) => {
        this.router.navigate(['/profile']);
        this.toastr.success(response);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

}
