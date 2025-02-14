import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { LoginResponse } from '../models/login-response.model';
import { LoginData } from '../models/login-data.model';

@Injectable({
  providedIn: 'root',
})
export class LoginPageService {
  private loginUrl = `${environment.apiBaseUrl}/User/Login`;

  constructor(private http: HttpClient) {}

  login(data: LoginData): Observable<string> {
    return this.http.post<string>(this.loginUrl, data, {
      responseType: 'text' as 'json',
    });
  }
}
