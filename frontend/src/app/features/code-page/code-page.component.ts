import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { AppComponent } from '../../app.component';
import { CodePageService } from './service/code-page.service';
import { dtoUserCode } from './models/dtoUserCode.model';
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
  templateUrl: './code-page.component.html',
  styleUrl: './code-page.component.scss'
})
export class CodePageComponent implements OnInit {
  icode: string = '';
  userEmail: string = '';
  isResendDisabled: boolean = false;
  timer: number = 60;
  backgroundImage = `url(${registerLoginBackground})`;

  dtoUserCode : dtoUserCode = {
    email: this.userEmail,
    code: this.icode,
    codeType: "Email"
  };

  constructor(private codePageService: CodePageService, private authService: AuthService, 
    private toastr: ToastrService, private router: Router, private route: ActivatedRoute){}

  ngOnInit(): void {
    this.startResendTimer();
    this.route.queryParams.subscribe(params => {
      const email = params['email'];
      if (email) {
        localStorage.setItem('Email', email);
      }
    })
    
    this.route.queryParams.subscribe(params => {
      this.dtoUserCode.codeType = params['type'];
    });
  }

  onSubmit() {
    if (this.icode.length === 6) {
      this.userEmail = localStorage.getItem('Email') || '';
      if(this.userEmail === '')
        {
          this.toastr.error('Open link from email you got!');
          localStorage.removeItem('token');
          localStorage.removeItem('role');
          this.router.navigate(['/login']);
        }
      this.dtoUserCode.email = this.userEmail;
      this.dtoUserCode.code = this.icode;
      this.codePageService.sendCodeToVerifyEmail(this.dtoUserCode).subscribe(
        (data: string) => {
          localStorage.removeItem('Email');
          if(this.dtoUserCode.codeType != 'Phone')
          {
            this.router.navigate(['/login']);
            this.toastr.success('Profile is created.');
          }
          else
          {
            this.toastr.success('Phone confirmed!');
            this.router.navigate(['/profile']);
          }
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
    this.startResendTimer();
    if(this.userEmail === '')
    {
      this.toastr.error('Log in again!');
      localStorage.removeItem('token');
      this.router.navigate(['/login']);
    }
    this.codePageService.resendCodeToVerifyEmail(this.userEmail, "email").subscribe(
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
