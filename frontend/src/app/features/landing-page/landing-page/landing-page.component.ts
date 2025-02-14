import { Component } from '@angular/core';
import { landingFeatures } from '../../../constants/landingConstant';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LandingSlideShowComponent } from '../../../core/components/landing-slide-show/landing-slide-show.component';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-landing-page',
  imports: [CommonModule, LandingSlideShowComponent],
  standalone: true,
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss',
  animations: [
    // Fade-in animation for hero section
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('1000ms ease-in', style({ opacity: 1 })),
      ]),
    ]),
  ],
})
export class LandingPageComponent {
  constructor(private router: Router) {}

  features = landingFeatures;

  onRegisterClick() {
    this.router.navigate(['/register']);
  }
}
