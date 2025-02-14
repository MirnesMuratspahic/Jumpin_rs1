import { Component, OnInit } from '@angular/core';
import { ProfilePageService } from '../profile-page/services/profile-page.service';
import { User } from '../profile-page/models/user.model';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../core/components/navbar/navbar.component';
import { UserRoute } from '../home-page/models/userRoute';
import { Router } from '@angular/router';
import { RoutesComponent } from '../../core/components/routes/routes.component';
import { CarsComponent } from '../../core/components/cars/cars.component';
import { FlatsComponent } from '../../core/components/flats/flats.component';
import { ToastrService } from 'ngx-toastr';
import { error } from 'console';

@Component({
  selector: 'app-profile-page',
  imports: [
    CommonModule,
    FormsModule,
    NavbarComponent,
    RoutesComponent,
    CarsComponent,
    FlatsComponent,
  ],
  standalone: true,
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss'],
})
export class ProfilePageComponent implements OnInit {
  user: User | null = null;
  userEmail: string = '';
  editingUser: Partial<User> = {};
  isEditingProfile: boolean = false;
  activeTab: 'routes' | 'cars' | 'flats' = 'routes';
  rola: string = '';
  

  constructor(
    private profilePageService: ProfilePageService,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    const currentUserEmail = this.authService.getEmailFromToken();
    this.rola = this.authService.getRoleFromToken() || '';

    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    this.profilePageService.getUserInformations(this.userEmail).subscribe(
      (data: User) => {
        this.user = data;
        console.log(data.accountType);
      },
      (error) => {
        this.toastr.error('Error ocured!');
      }
    );
  }

  public setActiveTab(tab: 'routes' | 'cars' | 'flats') {
    this.activeTab = tab;
  }

  toggleEditProfile() {
    this.isEditingProfile = !this.isEditingProfile;

    if (this.isEditingProfile && this.user) {
      this.editingUser = { ...this.user };
    }
  }

  onUpdateProfile(): void {
    if (!this.user) return;

    const previousUserData = { ...this.user };

    if (
      !this.editingUser.firstName ||
      !this.editingUser.lastName ||
      !this.editingUser.imageUrl
    ) {
      this.toastr.error('All fields are required!');
      return;
    }

    const updatedData: User = {
      email: this.user.email,
      firstName: this.editingUser.firstName ?? this.user.firstName,
      lastName: this.editingUser.lastName ?? this.user.lastName,
      imageUrl: this.editingUser.imageUrl ?? this.user.imageUrl,
      phoneNumber: this.editingUser.phoneNumber ?? this.user.phoneNumber,
      accountType: this.user.accountType,
      status: "Active",
      phoneConfirmed: this.user.phoneConfirmed
    };

    this.user = { ...this.user, ...updatedData };

    this.profilePageService.updateUserProfile(updatedData).subscribe({
      next: () => {
        this.toastr.success('Profile updated!');
        this.isEditingProfile = false;
      },
      error: (err) => {
        this.toastr.error('Failed to update!');
        this.user = previousUserData;
      },
    });

  }

  confirmPhoneNumber(){
    this.profilePageService.sendSmsForPhoneVerification(this.userEmail).subscribe(
      (data) => {
        this.toastr.success(data);
        localStorage.setItem('Email', this.userEmail);
        this.router.navigate(['/code'], { queryParams: { type: "Phone"} });
      },
      (error) => {
        this.toastr.error(error);
      }
    )
  }

  sendEmail(){
    this.profilePageService.sendEmailForPasswordReset(this.userEmail).subscribe(
      (data) => {
        this.isEditingProfile = !this.isEditingProfile;
        this.toastr.success("Link sent! Check you email!")
      },
      (error) => {
        this.toastr.error(error);
      }
    )
  }



  navigateToVIP(){
    this.router.navigate(['/stripe']);
  }
}
