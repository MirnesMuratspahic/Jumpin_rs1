import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { RequestsPageService } from '../../../features/requests-page/services/requests-page.service';
import { FormsModule } from '@angular/forms';
import { dtoRequestAcceptOrDecline } from '../../../features/requests-page/models/dtoRequestAcceptOrDecline.model';
import { CarRequest } from '../../../features/requests-page/models/carRequest.model';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-car-request',
    imports: [CommonModule, FormsModule],
    templateUrl: './car-request.component.html',
    styleUrl: './car-request.component.scss'
})
export class CarRequestComponent {
  constructor(
    private authService: AuthService,
    private requestService: RequestsPageService,
    private toastr: ToastrService
  ) {}

  carRequests: CarRequest[] = [];
  userEmail: string = '';
  response: string = '';
  isApiRequestInProgress = false;

  dtoRequest: dtoRequestAcceptOrDecline = {
    requestId: 0,
    decision: -1,
    requestType: '',
  };

  activeTab2: string = 'recived';

  setActiveTab2(tab: string) {
    this.activeTab2 = tab;
    if (this.activeTab2 === 'recived') {
      this.carRequests = [];
      this.getCarsRecivedRequests();
    } else if (this.activeTab2 === 'sent') {
      this.carRequests = [];
      this.getCarsSentRequests();
    }
  }

  ngOnInit(): void {
    const currentUserEmail = this.authService.getEmailFromToken();

    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    this.requestService.getCarsRecivedRequests(this.userEmail).subscribe(
      (data: CarRequest[]) => {
        this.carRequests = [];
        this.carRequests = data;
      },
      (error) => {
        console.error('Error fetching sent requests:', error);
      }
    );
  }

  public getCarsSentRequests() {
    this.carRequests = [];
    this.requestService.getCarsSentRequests(this.userEmail).subscribe(
      (data: CarRequest[]) => {
        this.carRequests = data;
      },
      (error) => {
        console.error('Error fetching sent requests:', error);
      }
    );
  }

  public getCarsRecivedRequests() {
    this.carRequests = [];
    this.requestService.getCarsRecivedRequests(this.userEmail).subscribe(
      (data: CarRequest[]) => {
        this.carRequests = data;
      },
      (error) => {
        console.error('Error fetching requests:', error);
      }
    );
  }

  acceptOrDeclineRequest(item: any, decision: number) {
    if (this.isApiRequestInProgress) {
      return;
    }
    this.isApiRequestInProgress = true; 
    this.dtoRequest.requestId = item.id;
    this.dtoRequest.decision = decision;
    this.dtoRequest.requestType = 'Car';

    this.requestService.acceptOrDeclineRequest(this.dtoRequest).subscribe(
      (data: string) => {
        this.response = data;
        this.getCarsRecivedRequests();
        this.toastr.success(data);
      },
      (error) => {
        this.toastr.error(error);
      }
    );
    setTimeout(() => {
      this.isApiRequestInProgress = false;
    }, 5000);
  }
}
