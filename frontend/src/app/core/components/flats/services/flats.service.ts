import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { catchError, Observable } from 'rxjs';
import { UserFlat } from '../../../../features/home-page/models/userFlat.model';
import { HeaderService } from '../../../../services/headerService/header.service';
import { HandleErrorService } from '../../../../services/errorService/handle-error.service';
import { dtoRequest } from '../../routes/models/dtoRequest.model';

@Injectable({
  providedIn: 'root',
})
export class FlatsService {
  constructor(
    private http: HttpClient,
    private headerService: HeaderService,
    private handleError: HandleErrorService
  ) {}

  private getUserFlatsUrl = `${environment.apiBaseUrl}/Flat/GetUserFlats`;
  private deleteFlatUrl = `${environment.apiBaseUrl}/Flat/DeleteFlat`;
  private getFlatsUrl = `${environment.apiBaseUrl}/Flat/GetFlats`;
  private addFlatUrl = `${environment.apiBaseUrl}/Flat/AddFlat`;
  private sendRequestUrl = `${environment.apiBaseUrl}/Request/SendRequest`;

  getFlats(): Observable<any[]> {
    const headers = this.headerService.createHeaders();
    return this.http
      .get<any[]>(this.getFlatsUrl, { headers })
      .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  addFlat(carData: any): Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(this.addFlatUrl, carData, {
      headers,
      responseType: 'text' as 'json',
    });
  }

  getUserFlats(email: string): Observable<UserFlat[]> {
    const headers = this.headerService.createHeaders();
    return this.http
      .post<UserFlat[]>(this.getUserFlatsUrl, JSON.stringify(email), {
        headers,
      })
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
