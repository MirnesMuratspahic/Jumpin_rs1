import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';
import { StripeService } from '../stripe-page/service/stripe-service.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-succes-page',
  imports: [],
  templateUrl: './succes-page.component.html',
  styleUrl: './succes-page.component.scss'
})
export class SuccessComponent implements OnInit {

  sessionId: string = '';
  userEmail: string = '';

  constructor(
    private route: ActivatedRoute,
    private stripeService: StripeService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {

    const currentUserEmail = this.authService.getEmailFromToken();
    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    this.stripeService.upgradeUsersAccount(this.userEmail).subscribe(
      (response) => {
        console.log('User profile upgraded:', response);
        this.router.navigate(['/profile']);
      },
      (error) => {
        console.error('Error upgrading profile:', error);
      }
    );
  }
       
}


