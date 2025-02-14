import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminPanelService } from './service/admin-panel.service';
import { User } from '../profile-page/models/user.model';
import { error } from 'console';
import { ToastrService } from 'ngx-toastr';
import { UserRoute } from '../home-page/models/userRoute';
import { ActivatedRoute } from '@angular/router';
import { UserCar } from '../home-page/models/userCar.model';
import { UserFlat } from '../home-page/models/userFlat.model';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent implements OnInit {
  type: string = '';
  users: User[] = [];
  routes: UserRoute[] =  [];
  cars: UserCar[] = [];
  flats: UserFlat[] = [];

  constructor(private adminService: AdminPanelService, private toastr:ToastrService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.type = params['type'];
    });

    if(this.type === 'User')
      this.getUsers();
    else if(this.type === 'Route')
      this.getRoutes();
    else if(this.type === 'Car')
      this.getCars();
    else
      this.getFlats();
  }

  getUsers() {
    this.adminService.getUsers().subscribe(
      (data) => {
        this.users = data;
      },
      (error) => {
        this.toastr.error(error);
      }
    )
  }

  getRoutes() {
    this.adminService.getRoutes().subscribe(
      (data) => {
        this.routes = data;
      },
      (error) => {
        this.toastr.error(error);
      }
    )
  }

  getCars() {
    this.adminService.getCars().subscribe(
      (data) => {
        this.cars = data;
      },
      (error) => {
        this.toastr.error(error);
      }
    )
  }

  getFlats() {
    this.adminService.getFlats().subscribe(
      (data) => {
        this.flats = data;
      },
      (error) => {
        this.toastr.error(error);
      }
    )
  }

  toggleStatus(status: number, user: any): void {
    this.adminService.changeUserStatus(status ,user.email).subscribe(
      (data) => {
        this.toastr.success(data);
        this.getUsers();
      },
      (error) => {
        this.toastr.error(error);
      }
    )
  }

  onDeleteRoute(routeId: number) {
    this.adminService.deleteRoute(routeId).subscribe(
      (data: string) => {
        this.routes = this.routes.filter((route) => route.route.id !== routeId);
        this.getRoutes();
        this.toastr.success(data);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  onDeleteCar(carId: number) {
    this.adminService.deleteCar(carId).subscribe(
      (data: string) => {
        this.cars = this.cars.filter((car) => car.car.id !== carId);
        this.getCars();
        this.toastr.success(data);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  onDeleteFlat(flatId: number) {
    this.adminService.deleteFlat(flatId).subscribe(
      (response: string) => {
        this.flats = this.flats.filter((flat) => flat.flat.id !== flatId);
        this.getFlats();
        this.toastr.success(response);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }
}
