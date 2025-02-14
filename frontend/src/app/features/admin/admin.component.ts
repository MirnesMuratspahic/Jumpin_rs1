import { Component, OnInit } from '@angular/core';
import { AdminService } from './services/admin.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../core/components/navbar/navbar.component';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  imports: [CommonModule, FormsModule, NavbarComponent],
  standalone: true,
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
  chart: any;
  usersCount: number = 0;
  routesCount: number = 0;
  carsCount: number = 0;
  flatsCount: number = 0;

  constructor(private adminService: AdminService, private router: Router) {}

  ngOnInit(): void {
    this.fetchAppData();
  }

  navigateToAdminPanel(type: string) {
    this.router.navigate(['/admin-panel'], { queryParams: { type: type} });
  }

  fetchAppData(): void {
    this.adminService.fetchAppData().subscribe((data) => {
      console.log(data);

      const labels = data.map((item) => item.name);
      const counts = data.map((item) => item.count);

      this.usersCount = data.find((item) => item.name === 'Users')?.count || 0;
      this.routesCount =
        data.find((item) => item.name === 'Routes')?.count || 0;
      this.carsCount = data.find((item) => item.name === 'Cars')?.count || 0;
      this.flatsCount = data.find((item) => item.name === 'Flats')?.count || 0;

      this.chart = this.adminService.createChart(labels, counts); // Kreiranje grafa
    });
  }
}
