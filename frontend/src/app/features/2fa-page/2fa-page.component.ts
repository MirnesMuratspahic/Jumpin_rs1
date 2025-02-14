import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppComponent } from '../../app.component';
import { TwoFaPageService } from './service/2fa-page.service';
import { dtoUserCode } from '../code-page/models/dtoUserCode.model';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { registerLoginBackground } from '../../constants/imagesConstants';

@Component({
  selector: 'app-code-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './2fa-page.component.html',
  styleUrls: ['./2fa-page.component.scss'], 
})
export class TwoFaCodePageComponent implements OnInit {
  icode: string = '';
  userEmail: string = '';
  isResendDisabled: boolean = false;
  timer: number = 60;
  backgroundImage = `url(${registerLoginBackground})`;

  dtoUserCode: dtoUserCode = {
    email: '',
    code: '',
    codeType: 'Email'
  };

  constructor(
    private twoFaPageService: TwoFaPageService,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const email = params['email'];
      if (email) {
        localStorage.setItem('Email', email);
      }
    })
    this.userEmail = localStorage.getItem('Email') || '';
    this.startResendTimer();
  }

  onSubmit() {
    if (this.icode.length === 6) {
      this.userEmail = localStorage.getItem('Email') || '';
      if(this.userEmail === '')
        {
          this.toastr.error('Log in again!');
          localStorage.removeItem('token');
          localStorage.removeItem('role');
          this.router.navigate(['/login']);
        }
      this.dtoUserCode.email = this.userEmail;
      this.dtoUserCode.code = this.icode;
      this.twoFaPageService.sendCodeToVerifyEmail(this.dtoUserCode).subscribe(
        (data: string) => {
          localStorage.removeItem('Email');
          this.router.navigate(['/home']);
          this.toastr.success('Successfully logged in!');
      },
      (error) => {
        console.log(error);
        this.toastr.error(error);
      });
    } else {
      console.error('Invalid code.');
    }
  }

  startResendTimer() {
    this.isResendDisabled = true;
    const interval = setInterval(() => {
      if (this.timer > 0) {
        this.timer--;
      } else {
        clearInterval(interval);
        this.isResendDisabled = false;
        this.timer = 60;
      }
    }, 1000);
  }

  onResendCode() {
    if (this.isResendDisabled) {
      return;
    }

    this.userEmail = localStorage.getItem('Email') || '';
    if(this.userEmail === '')
      {
        this.toastr.error('Log in again!');
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        this.router.navigate(['/login']);
      }
    this.startResendTimer();
    this.twoFaPageService.resendCodeToVerifyEmail(this.userEmail, "2fa").subscribe(
      (data) =>
      {
        this.toastr.success(data);
      },
      (error) =>
      {
        this.toastr.error(error);
      }
    )
  }
}
