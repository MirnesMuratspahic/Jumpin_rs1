import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cancel-page',
  templateUrl: './cancel-page.component.html',
  styleUrls: ['./cancel-page.component.scss']
})
export class CancelPageComponent {

  constructor(private router: Router) { }

  goBack() {
    this.router.navigate(['/profile']);
  }
}
