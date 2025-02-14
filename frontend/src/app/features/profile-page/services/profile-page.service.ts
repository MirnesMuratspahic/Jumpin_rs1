import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from '../../../../environments/environment';
import { UserRoute } from '../../home-page/models/userRoute';
import { HeaderService } from '../../../services/headerService/header.service';
import { HandleErrorService } from '../../../services/errorService/handle-error.service';

@Injectable({
  providedIn: 'root',
})
export class ProfilePageService {
  
  constructor(private http: HttpClient, private headerService: HeaderService, private handleError: HandleErrorService) {}
  
  getUserInfo = `${environment.apiBaseUrl}/User/GetUserInformations`;
  sendCodeUrl = `${environment.apiBaseUrl}/User/SendEmailForPasswordReset`;
  confirmPhoneUrl = `${environment.apiBaseUrl}/User/SendSmsForPhoneVerification`;

  
  getUserInformations(email: string): Observable<User> {
    const headers = this.headerService.createHeaders();
    return this.http.post<User>(this.getUserInfo, JSON.stringify(email), {headers,})
    .pipe(catchError((error) => this.handleError.handleError(error)));
  }

  updateUserProfile(payload: Partial<User>): Observable<void> {
    const headers = this.headerService.createHeaders();
    const url = `${environment.apiBaseUrl}/User/UpdateUserInformations`;
    return this.http.put<void>(url, payload, {headers, responseType: 'text' as 'json'});
  }

  sendEmailForPasswordReset(email: string) : Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(this.sendCodeUrl, JSON.stringify(email), {headers, responseType: 'text' as 'json'} );
  }

  sendSmsForPhoneVerification(email: string) : Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(this.confirmPhoneUrl, JSON.stringify(email), {headers, responseType: 'text' as 'json'} );
  }
}
