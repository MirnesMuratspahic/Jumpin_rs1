import { Component } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoginPageService } from './services/login-page.service';
import { LoginData } from './models/login-data.model';
import { registerLoginBackground } from '../../constants/imagesConstants';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule],
  standalone: true,
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginComponent {
  backgroundImage = `url(${registerLoginBackground})`;
  loginForm: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginPageService,
    private router: Router,
    private toastr: ToastrService,
    private authService: AuthService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  async onLoginSubmit() {
    if (this.loginForm.valid) {
      const loginData: LoginData = this.loginForm.value;

      try {
        const response = await this.loginService.login(loginData).toPromise();
        if (response) {
          localStorage.setItem('token', response);
          const role = this.authService.getRoleFromToken();
          if (role) {
            localStorage.setItem('role', role);
          }
          this.errorMessage = null;
          localStorage.setItem('Email', loginData.email);
          this.router.navigate(['/2fa']);
        }
      } catch (error: any) {
          if (error instanceof HttpErrorResponse) {
            if (error.status === 0) {
              this.errorMessage = 'Server error!';
            }
          else if (error.status === 400) {
            this.toastr.error(error.error);
          }
        }
      }
    }
  }

  onRegisterPage() {
    this.router.navigate(['/register']);
  }
}
