import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CarsService } from './services/cars-service.service';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { UserCar } from '../../../features/home-page/models/userCar.model';
import { dtoRequest } from '../routes/models/dtoRequest.model';
import { ToastrService } from 'ngx-toastr';
import { RequestsPageService } from '../../../features/requests-page/services/requests-page.service';
import { VoiceRecognitionService } from '../../../services/voiceService/voice-service.service';
import { NgZone } from '@angular/core';


@Component({
  selector: 'app-cars',
  imports: [CommonModule, FormsModule],
  standalone: true,
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.scss'],
})
export class CarsComponent implements OnInit {
  @Input() apiUrl!: string;

  cars: UserCar[] = [];
  filteredCars: any[] = [];
  response: string = '';
  userEmail: string = '';
  searchTerm: string = '';
  isApiRequestInProgress = false;
  isListening = false;

  newCar = {
    email: '',
    name: '',
    dateAndTime: '',
    price: 0,
    description: '',
    type: '',
  };

  newRequest: dtoRequest = {
    passengerEmail: '',
    description: '',
    status: '',
    objectId: 0,
    objectType: '',
  };

  constructor(
    private carsService: CarsService,
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService,
    private requestService: RequestsPageService,
    private voiceService: VoiceRecognitionService,
    private ngZone: NgZone
  ) {}

  ngOnInit(): void {
    const currentUserEmail = this.authService.getEmailFromToken();

    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    if (this.apiUrl === 'https://localhost:7172/Car/GetCars') {
      this.getCars();
    } else if (this.apiUrl === 'https://localhost:7172/Car/GetUsersCars') {
      this.getUsersCars();
    }
  }

  navigateToRating(email : string) : void {
    localStorage.setItem('Email', email);
    this.router.navigate(['/rating']);
  }

  getCars(): void {
    this.cars = [];
    this.carsService.getCars().subscribe(
      (data: any[]) => {
        this.cars = data;
        this.filterCars();
      },
      (error) => {
        console.error('Error fetching cars:', error);
      }
    );
  }

  getUsersCars() {
    this.cars = [];
    this.carsService.getUsersCars(this.userEmail).subscribe(
      (data: UserCar[]) => {
        this.cars = data;
        this.filteredCars = this.cars;
      },
      (error) => {
        console.error('Error fetching cars:', error);
      }
    );
  }

  sendRequest(item: any): void {
    if (this.isApiRequestInProgress) {
      return;
    }
    this.isApiRequestInProgress = true;
    this.newRequest.passengerEmail =
      this.userEmail != null ? this.userEmail : '';
    this.newRequest.status = 'Pending';
    this.newRequest.objectId = item.car.id;
    this.newRequest.objectType = 'Car';
    this.newRequest.description = 'None';

    this.requestService.sendRequest(this.newRequest).subscribe(
      (apiResponse: string) => {
        this.response = apiResponse;
        this.getCars();
        this.toastr.success(this.response);
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
    setTimeout(() => {
      this.isApiRequestInProgress = false;
    }, 5000);
  }

  onDeleteCar(carId: number) {
    this.carsService.deleteCar(carId).subscribe(
      (data: string) => {
        this.cars = this.cars.filter((car) => car.car.id !== carId);
        this.getUsersCars();
        this.toastr.success(data);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  filterCars(): void {
    if (this.searchTerm) {
      const keywords = this.searchTerm
        .toLowerCase()
        .replace(/[^a-zA-Z0-9\s-]/g, '')
        .split(' ')
        .filter(keyword => keyword);
  
      this.filteredCars = this.cars.filter(car => {
        const carName = car.car.name.toLowerCase().replace(/[^a-zA-Z0-9\s-]/g, '')
  
        return keywords.every(keyword => carName.includes(keyword));
      });
    } else {
      this.filteredCars = [...this.cars];
    }
    this.filterVIPAds();
  }

  startVoiceSearch() {
    this.isListening = true;
    this.voiceService.startListening((transcript) => {
      this.ngZone.run(() => {
        this.searchTerm = transcript;
      });
    });
    setTimeout(() => {
      this.voiceService.stopListening();
      this.isListening = false;
      this.filterCars();
    }, 5000);
  }

  filterVIPAds(){
    this.filteredCars = this.filteredCars.sort((a, b) => {
      if (a.user.accountType === 'VIP' && b.user.accountType !== 'VIP') {
        return -1; 
      } else if (a.user.accountType !== 'VIP' && b.user.accountType === 'VIP') {
        return 1; 
      }
      return 0;
    });
  }

  onEditCar(carId: number): void {
    this.router.navigate(['/edit-car', carId]);
  }

}
