import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { RouteRequest } from '../models/RouteRequest.model';
import { CarRequest } from '../models/carRequest.model';
import { environment } from '../../../../environments/environment';
import { stringify } from 'querystring';
import { json } from 'stream/consumers';
import { dtoRequestAcceptOrDecline } from '../models/dtoRequestAcceptOrDecline.model';
import { FlatRequest } from '../models/flatRequest.model';
import { HeaderService } from '../../../services/headerService/header.service';
import { HandleErrorService } from '../../../services/errorService/handle-error.service';
import { dtoRequest } from '../../home-page/models/dtoRequest.model';

@Injectable({
  providedIn: 'root',
})
export class RequestsPageService {
  constructor(private http: HttpClient, private headerService: HeaderService, private handleError: HandleErrorService) {}

  private acceptOrDeclineRequestUrl = `${environment.apiBaseUrl}/Request/AcceptOrDeclineRequest`;
  private getRoutesRecivedRequestsUrl = `${environment.apiBaseUrl}/Request/GetRouteRequestsRecived`;
  private getRoutesSentRequestsUrl = `${environment.apiBaseUrl}/Request/GetRouteSentRequests`;
  private getCarsRecivedRequestsUrl = `${environment.apiBaseUrl}/Request/GetCarRequestsRecived`;
  private getCarsSentRequestsUrl = `${environment.apiBaseUrl}/Request/GetCarSentRequests`;
  private getFlatsRecivedRequestsUrl = `${environment.apiBaseUrl}/Request/GetFlatRequestsRecived`;
  private getFlatsSentRequestsUrl = `${environment.apiBaseUrl}/Request/GetFlatSentRequests`;
  private sendRequestUrl = `${environment.apiBaseUrl}/Request/SendRequest`;

  acceptOrDeclineRequest(dtoRequest: dtoRequestAcceptOrDecline): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(this.acceptOrDeclineRequestUrl, dtoRequest, {headers, responseType: 'text' as 'json'});
  }

  sendRequest(request: dtoRequest): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(this.sendRequestUrl, request, {headers, responseType: 'text' as 'json'});
  }

  //----------------Route requests-------------------//

  getRoutesRecivedRequests(email: string): Observable<RouteRequest[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<RouteRequest[]>(this.getRoutesRecivedRequestsUrl,JSON.stringify(email),{ headers })
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  getRoutesSentRequests(email: string): Observable<RouteRequest[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<RouteRequest[]>(this.getRoutesSentRequestsUrl,JSON.stringify(email),{ headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  //----------------Cars requests-------------------//

  getCarsRecivedRequests(email: string): Observable<CarRequest[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<CarRequest[]>(this.getCarsRecivedRequestsUrl,JSON.stringify(email),{ headers })
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  getCarsSentRequests(email: string): Observable<CarRequest[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<CarRequest[]>(this.getCarsSentRequestsUrl,JSON.stringify(email),{ headers })
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  //----------------Flats requests-------------------//

  getFlatsRecivedRequests(email: string): Observable<FlatRequest[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<FlatRequest[]>(this.getFlatsRecivedRequestsUrl,JSON.stringify(email),{ headers })
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  getFlatsSentRequests(email: string): Observable<FlatRequest[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<FlatRequest[]>(this.getFlatsSentRequestsUrl,JSON.stringify(email),{ headers })
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }
}
