import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { UserCar } from '../../../../features/home-page/models/userCar.model';
import { HeaderService } from '../../../../services/headerService/header.service';
import { HandleErrorService } from '../../../../services/errorService/handle-error.service';
import { Car } from '../../../../features/home-page/models/car.model';

@Injectable({
  providedIn: 'root',
})
export class CarsService {
  constructor(private http: HttpClient, private headerService: HeaderService, private handleError: HandleErrorService) {}

  private getUserCars = `${environment.apiBaseUrl}/Car/GetUsersCars`;
  private deleteCarUrl = `${environment.apiBaseUrl}/Car/DeleteCar`;
  private getCarsUrl = `${environment.apiBaseUrl}/Car/GetCars`;
  private sendRequestUrl = `${environment.apiBaseUrl}/Request/SendRequest`;
  private addCarUrl = `${environment.apiBaseUrl}/Car/AddCar`;



  getCars(): Observable<Car[]> {
    const headers = this.headerService.createHeaders();
    return this.http.get<Car[]>(this.getCarsUrl, {headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  addCar(carData: any): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(this.addCarUrl, carData,{ headers, responseType: 'text' as 'json'});
  }

  getUsersCars(email: string): Observable<UserCar[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<UserCar[]>(this.getUserCars, JSON.stringify(email), {headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  deleteCar(id: number): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.delete<string>(`${this.deleteCarUrl}/${id}`, {headers, responseType: 'text' as 'json'});
  }
}
