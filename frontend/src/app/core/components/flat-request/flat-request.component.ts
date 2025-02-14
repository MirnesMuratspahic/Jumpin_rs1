import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { RequestsPageService } from '../../../features/requests-page/services/requests-page.service';
import { FormsModule } from '@angular/forms';
import { dtoRequestAcceptOrDecline } from '../../../features/requests-page/models/dtoRequestAcceptOrDecline.model';
import { CarRequest } from '../../../features/requests-page/models/carRequest.model';
import { FlatRequest } from '../../../features/requests-page/models/flatRequest.model';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-flat-request',
    imports: [CommonModule, FormsModule],
    templateUrl: './flat-request.component.html',
    styleUrl: './flat-request.component.scss'
})
export class FlatRequestComponent {
  constructor(
    private authService: AuthService,
    private requestService: RequestsPageService,
    private toastr: ToastrService
  ) {}

  flatRequests: FlatRequest[] = [];
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
      this.flatRequests = [];
      this.getFlatsRecivedRequests();
    } else if (this.activeTab2 === 'sent') {
      this.flatRequests = [];
      this.getFlatsSentRequests();
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

    this.requestService.getFlatsRecivedRequests(this.userEmail).subscribe(
      (data: FlatRequest[]) => {
        this.flatRequests = [];
        this.flatRequests = data;
      },
      (error) => {
        console.error('Error fetching sent requests:', error);
      }
    );
  }

  public getFlatsSentRequests() {
    this.flatRequests = [];
    this.requestService.getFlatsSentRequests(this.userEmail).subscribe(
      (data: FlatRequest[]) => {
        this.flatRequests = data;
      },
      (error) => {
        console.error('Error fetching sent requests:', error);
      }
    );
  }

  public getFlatsRecivedRequests() {
    this.flatRequests = [];
    this.requestService.getFlatsRecivedRequests(this.userEmail).subscribe(
      (data: FlatRequest[]) => {
        this.flatRequests = data;
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
    this.dtoRequest.requestType = 'Flat';

    this.requestService.acceptOrDeclineRequest(this.dtoRequest).subscribe(
      (data: string) => {
        this.response = data;
        this.getFlatsRecivedRequests();
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
