import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError,Observable } from 'rxjs';
import { UserRoute } from '../models/userRoute';
import { dtoUserRoute } from '../models/dtoUserRoute.model';
import { dtoRequest } from '../models/dtoRequest.model';
import { environment } from '../../../../../environments/environment';
import { HeaderService } from '../../../../services/headerService/header.service';
import { HandleErrorService } from '../../../../services/errorService/handle-error.service';

@Injectable({
  providedIn: 'root',
})
export class RoutesServiceService {
  private addRouteUrl = `${environment.apiBaseUrl}/Route/AddRoute`;
  private getRoutesUrl = `${environment.apiBaseUrl}/Route/GetRoutes`;
  private sendRequestUrl = `${environment.apiBaseUrl}/Request/SendRequest`;
  private getUsersRouteUrl = `${environment.apiBaseUrl}/Route/GetUsersRoutes`;
  private deleteRouteUrl = `${environment.apiBaseUrl}/Route/DeleteRoute`;

  constructor(private http: HttpClient, private headerService: HeaderService, private handleError: HandleErrorService) {}

  getRoutes(): Observable<UserRoute[]> {
    const headers = this.headerService.createHeaders();
    return this.http.get<UserRoute[]>(this.getRoutesUrl, {headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  addRoute(route: dtoUserRoute): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(this.addRouteUrl, route, {headers, responseType: 'text' as 'json'});
  }

  getUsersRoutes(email: string ): Observable<UserRoute[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<UserRoute[]>(this.getUsersRouteUrl, JSON.stringify(email), { headers })
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  deleteRoute(id: number): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.delete<string>(`${this.deleteRouteUrl}/${id}`, {headers, responseType: 'text' as 'json'});
  }
}
