import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RegisterPageService } from './services/register-page.service';
import { RegisterData } from './models/register-data.model';
import { registerLoginBackground } from '../../constants/imagesConstants';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule, CommonModule],
  standalone: true,
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss'],
})
export class RegisterPageComponent implements OnInit {
  registrationForm: FormGroup;
  errorMessage: string | null = null;
  backgroundImage = `url(${registerLoginBackground})`;
  captchaText: string = '';
  userCaptcha: string = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private registerService: RegisterPageService,
    private toastr: ToastrService
  ) {
    this.registrationForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      role: ['User'],
      captcha: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.generateCaptcha();
  }

  generateCaptcha(): void {
    const characters =
      'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let captcha = '';
    for (let i = 0; i < 6; i++) {
      captcha += characters.charAt(
        Math.floor(Math.random() * characters.length)
      );
    }
    this.captchaText = captcha;
  }

  onSubmit(): void {
    if (this.registrationForm.valid) {
      const userData: RegisterData = this.registrationForm.value;

      if (this.userCaptcha !== this.captchaText) {
        this.toastr.error('CAPTCHA is not valid.');
        return;
      }

      this.registerService.register(userData).subscribe({
        next: (response) => {
          this.errorMessage = null;
          localStorage.setItem('Email', userData.email);
          this.router.navigate(['/code']);
        },
        error: (error) => {
          this.errorMessage = error.error;
        },
      });
    } else {
      this.toastr.error('Form is not valid.');
    }
  }

  onLoginPage(): void {
    this.router.navigate(['/login']);
  }
}
