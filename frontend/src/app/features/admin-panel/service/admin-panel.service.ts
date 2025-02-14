import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { HeaderService } from '../../../services/headerService/header.service';
import { HandleErrorService } from '../../../services/errorService/handle-error.service';
import { User } from '../../profile-page/models/user.model';
import { environment } from '../../../../environments/environment';
import { UserRoute } from '../../home-page/models/userRoute';
import { UserCar } from '../../home-page/models/userCar.model';
import { UserFlat } from '../../home-page/models/userFlat.model';

@Injectable({
  providedIn: 'root'
})
export class AdminPanelService {

  constructor(private http: HttpClient, private headerService: HeaderService, private handleError: HandleErrorService) {}

  private getUsersUrl = `${environment.apiBaseUrl}/User/GetUsers`;
  private userStatusUrl = `${environment.apiBaseUrl}/User/ChangeUserStatus`;
  private getRoutesUrl = `${environment.apiBaseUrl}/Route/GetRoutes`;
  private getCarsUrl = `${environment.apiBaseUrl}/Car/GetCars`;
  private getFlatsUrl = `${environment.apiBaseUrl}/Flat/GetFlats`;

  private deleteRouteUrl = `${environment.apiBaseUrl}/Route/DeleteRoute`;
  private deleteCarUrl = `${environment.apiBaseUrl}/Car/DeleteCar`;
  private deleteFlatUrl = `${environment.apiBaseUrl}/Flat/DeleteFlat`;


  getUsers(): Observable<User[]>{
      const headers = this.headerService.createHeaders();
      return this.http.get<User[]>(this.getUsersUrl, {headers})
      .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  changeUserStatus(status: number, email: string) : Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(`${this.userStatusUrl}/${status}`, JSON.stringify(email), {headers, responseType: 'text' as 'json'});}

  getRoutes(): Observable<UserRoute[]> {
   const headers = this.headerService.createHeaders();
   return this.http.get<UserRoute[]>(this.getRoutesUrl, {headers})
   .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  deleteRoute(id: number): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.delete<string>(`${this.deleteRouteUrl}/${id}`, {headers, responseType: 'text' as 'json'});
  }

  getCars(): Observable<UserCar[]> {
    const headers = this.headerService.createHeaders();
    return this.http.get<UserCar[]>(this.getCarsUrl, {headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));
   }

   deleteCar(id: number): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.delete<string>(`${this.deleteCarUrl}/${id}`, {headers, responseType: 'text' as 'json'});
  }

   getFlats(): Observable<UserFlat[]> {
    const headers = this.headerService.createHeaders();
    return this.http.get<UserFlat[]>(this.getFlatsUrl, {headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));
   }

   deleteFlat(id: number): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.delete<string>(`${this.deleteFlatUrl}/${id}`, {
      headers,
      responseType: 'text' as 'json',
    });
  }
  
  
}
