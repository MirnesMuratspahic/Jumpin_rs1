import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { HeaderService } from '../../../services/headerService/header.service';


@Injectable({
  providedIn: 'root'
})
export class PasswordResetService {

  constructor(private http: HttpClient, private headerService: HeaderService) {}


  resetPasswordUrl = `${environment.apiBaseUrl}/User/ResetPassword`;

  resetPassword(password: string, email: string) : Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(`${this.resetPasswordUrl}/${email}`, JSON.stringify(password), {headers, responseType: 'text' as 'json'} );
  }

}
