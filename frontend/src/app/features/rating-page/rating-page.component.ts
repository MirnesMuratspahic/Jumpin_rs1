import { Component, OnInit } from '@angular/core';
import { ProfilePageService } from '../profile-page/services/profile-page.service';
import { User } from '../profile-page/models/user.model';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../core/components/navbar/navbar.component';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { dtoRating } from './models/dtoRating.model';
import { RatingPageService } from './service/rating-page.service';
import { error } from 'console';
import { dtoUserRating } from './models/dtoUserRating.model';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-rating-page',
  imports: [
      CommonModule,
      FormsModule,
      NavbarComponent
    ],
  standalone: true,
  templateUrl: './rating-page.component.html',
  styleUrl: './rating-page.component.scss'
})
export class RatingPageComponent {

  user: User | null = null;
  userEmail: string = '';
  editingUser: Partial<User> = {};
  isEditingProfile: boolean = false;
  activeTab: 'routes' | 'cars' | 'flats' = 'routes';
  showReviewPopup = false;
  dtoRating: dtoRating = {
    usersRatingEmail: '',
    userWritingEmail: '',
    review: 1,
    comment: ''
  };
  reviews: dtoUserRating[] = [];
  email: string = '';

  constructor(
    private profilePageService: ProfilePageService,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService,
    private ratingService: RatingPageService
  ) {}

  ngOnInit(): void {
    const currentUserEmail = this.authService.getEmailFromToken();
    this.email = localStorage.getItem('Email') || '';

    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    this.profilePageService.getUserInformations(this.email).subscribe(
      (data: User) => {
        this.user = data;
      },
      (error) => {
        this.toastr.error('Error ocured!');
      }
    );

    this.getUsersReviews();
  }

  openReviewPopup() {
    this.showReviewPopup = true;
  }

  closeReviewPopup() {
    this.showReviewPopup = false;
    this.dtoRating = {
      usersRatingEmail: '',
      userWritingEmail: '',
      review: 1,
      comment: ''
    };
  }

  getUsersReviews() {
    this.ratingService.getUserReviews(this.email).subscribe(
      (data) => {
        this.reviews = data;
        this.sortReviews();
      },
    )
  }
  
  submitReview(form: NgForm){
    if (form.valid) {
      if (this.dtoRating.review) {
        this.dtoRating.review = parseFloat(parseFloat(this.dtoRating.review.toString()).toFixed(1));
      }
      this.dtoRating.userWritingEmail = this.userEmail;
      this.dtoRating.usersRatingEmail = this.email;
      this.ratingService.addRating(this.dtoRating).subscribe(
        (data:string) => {
          this.toastr.success(data);
          this.closeReviewPopup();
          this.getUsersReviews();
        },
        (error) => {
          this.toastr.error(error.error);
        }
    )
  }
}

validateDecimalPlaces(event: any) {
  const value = event.target.value;
  const regex = /^\d+(\.\d{0,1})?$/;

  if (!regex.test(value)) {
    event.target.value = value.slice(0, -1);
  }
}

deleteReview(reviewId: number) {
  console.log(reviewId);
  this.ratingService.deleteRating(reviewId).subscribe(
    (data) => {
      this.toastr.success(data);
      this.reviews = this.reviews.filter(review => review.id !== reviewId);
      this.getUsersReviews();
    },
    (error) => {
      this.toastr.error(error);
    }
  )
}

sortReviews(): void {
  this.reviews.sort((a, b) => {
    if (a.userWritingEmail === this.userEmail && b.userWritingEmail !== this.userEmail) {
      return -1;
    } else if (a.userWritingEmail !== this.userEmail && b.userWritingEmail === this.userEmail) {
      return 1; 
    }
    return 0;
  });
}

}
