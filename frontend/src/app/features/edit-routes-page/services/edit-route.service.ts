import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { HeaderService } from '../../../services/headerService/header.service';
import { HandleErrorService } from '../../../services/errorService/handle-error.service';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EditRouteService {

  constructor(private http: HttpClient, private headerService: HeaderService, private handleError: HandleErrorService) {}

  private getRouteDetailsUrl = `${environment.apiBaseUrl}/Route/GetRouteDetails`;
  private updateRouteUrl = `${environment.apiBaseUrl}/Route/UpdateRoute`;

  getRouteDetails(routeId: string): Observable<any> {
    const headers = this.headerService.createHeaders();
    return this.http.get<any>(`${this.getRouteDetailsUrl}/${routeId}`, {headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  updateRoute(routeId: string, routeData: any): Observable<any> {
    const headers = this.headerService.createHeaders();
    return this.http.put<any>(`${this.updateRouteUrl}/${routeId}`, routeData, {headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }
}
