import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouteRequest } from '../../../features/requests-page/models/RouteRequest.model';
import { AuthService } from '../../../services/auth.service';
import { RequestsPageService } from '../../../features/requests-page/services/requests-page.service';
import { FormsModule } from '@angular/forms';
import { dtoRequestAcceptOrDecline } from '../../../features/requests-page/models/dtoRequestAcceptOrDecline.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-route-request',
  imports: [CommonModule, FormsModule],
  standalone: true,
  templateUrl: './route-request.component.html',
  styleUrls: ['./route-request.component.html'],
})
export class RouteRequestComponent {
  constructor(
    private authService: AuthService,
    private requestService: RequestsPageService,
    private toastr: ToastrService
  ) {}

  routeRequests: RouteRequest[] = [];
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
      this.routeRequests = [];
      this.getRoutesRecivedRequests();
    } else if (this.activeTab2 === 'sent') {
      this.routeRequests = [];
      this.getRoutesSentRequests();
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

    this.requestService.getRoutesRecivedRequests(currentUserEmail).subscribe(
      (data: RouteRequest[]) => {
        this.routeRequests = [];
        this.routeRequests = data;
      },
      (error) => {
        console.error('Error fetching requests:', error);
      }
    );
  }

  //----------------Route requests-------------------//

  public getRoutesSentRequests() {
    this.routeRequests = [];
    this.requestService.getRoutesSentRequests(this.userEmail).subscribe(
      (data: RouteRequest[]) => {
        this.routeRequests = data;
      },
      (error) => {
        console.error('Error fetching sent requests:', error);
      }
    );
  }

  public getRoutesRecivedRequests() {
    this.routeRequests = [];
    this.requestService.getRoutesRecivedRequests(this.userEmail).subscribe(
      (data: RouteRequest[]) => {
        this.routeRequests = data;
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
    this.dtoRequest.requestType = 'Route';

    this.requestService.acceptOrDeclineRequest(this.dtoRequest).subscribe(
      (data: string) => {
        this.response = data;
        this.getRoutesRecivedRequests();
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
