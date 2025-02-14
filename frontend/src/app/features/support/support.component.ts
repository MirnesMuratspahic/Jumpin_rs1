import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { NavbarComponent } from '../../core/components/navbar/navbar.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-support',
  templateUrl: './support.component.html',
  styleUrls: ['./support.component.scss'],
  imports: [CommonModule, FormsModule, NavbarComponent],
  standalone: true,
})
export class SupportComponent {
  userQuery: string = '';
  aiResponse: string | null = null;
  endpoint: string = `${environment.apiBaseUrl}/AI/AIQuery`;

  constructor(private http: HttpClient, private router: Router) {}

  sendQuery() {
    if (!this.userQuery.trim()) {
      alert('Please enter a question!');
      return;
    }

    const body = JSON.stringify(this.userQuery);
    const headers = { 'Content-Type': 'application/json' };

    this.http
      .post(this.endpoint, body, { headers, responseType: 'text' })
      .subscribe({
        next: (response: string) => {
          this.aiResponse = response;
          this.userQuery = '';
        },
        error: (err) => {
          console.error('Error:', err);
          alert('An error occurred while contacting the AI service.');
        },
      });
  }

  goHome() {
    this.router.navigate(['/home']);
  }
}
