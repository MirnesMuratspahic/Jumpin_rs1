import { Routes } from '@angular/router';
import { LandingPageComponent } from './features/landing-page/landing-page/landing-page.component';
import { LoginComponent } from './features/login-page/login-page.component';
import { RegisterPageComponent } from './features/register-page/register-page.component';
import { HomePageComponent } from './features/home-page/home-page.component';
import { ProfilePageComponent } from './features/profile-page/profile-page.component';
import { RequestsPageComponent } from './features/requests-page/requests-page.component';
import { EditRoutesPageComponent } from './features/edit-routes-page/edit-routes-page.component';
import { AddPageComponent } from './features/add-page/add-page.component';
import { GoogleMapsComponent } from './features/google-maps/google-maps.component';
import { ViewMapsComponent } from './features/view-maps/view-maps.component';
import { AdminComponent } from './features/admin/admin.component';
import { CodePageComponent } from './features/code-page/code-page.component';
import { TwoFaCodePageComponent } from './features/2fa-page/2fa-page.component';
import { RatingPageComponent } from './features/rating-page/rating-page.component';
import { StripePageComponent } from './features/stripe-page/stripe-page.component';
import { SuccessComponent } from './features/succes-page/succes-page.component';
import { CancelPageComponent } from './features/cancel-page/cancel-page.component';
import { VipPageAdminComponent } from './features/vip-page-admin/vip-page-admin.component';
import { SupportComponent } from './features/support/support.component';
import { PasswordResetPage } from './features/password-reset-page/password-reset-page';
import { AdminPanelComponent } from './features/admin-panel/admin-panel.component';


export const appRoutes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterPageComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'profile', component: ProfilePageComponent },
  { path: 'requests', component: RequestsPageComponent },
  { path: 'edit-route/:id', component: EditRoutesPageComponent },
  { path: 'add-page', component: AddPageComponent },
  { path: 'google-maps-page', component: GoogleMapsComponent },
  { path: 'view-maps-page', component: ViewMapsComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'code', component: CodePageComponent },
  { path: '2fa', component: TwoFaCodePageComponent },
  { path: 'rating', component: RatingPageComponent },
  { path: 'stripe', component: StripePageComponent },
  { path: 'succes-page', component: SuccessComponent },
  { path: 'cancel-page', component: CancelPageComponent },
  { path: 'rating', component: RatingPageComponent },
  { path: 'vip-page/admin', component: VipPageAdminComponent },
  { path: 'support', component: SupportComponent },
  { path: 'rating', component: RatingPageComponent},
  { path: 'stripe', component: StripePageComponent},
  { path: 'succes-page', component: SuccessComponent},
  { path: 'cancel-page', component: CancelPageComponent},
  { path: 'password-reset-page', component: PasswordResetPage},
  { path: 'admin-panel', component: AdminPanelComponent}
];
