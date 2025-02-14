import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { dtoUserCode } from '../models/dtoUserCode.model';


@Injectable({
  providedIn: 'root'
})
export class CodePageService {

  constructor(private http: HttpClient) {}

  sendCodeUrl = `${environment.apiBaseUrl}/User/AcceptUserCode`;
  resendCodeUrl = `${environment.apiBaseUrl}/User/ResendCode`;

  sendCodeToVerifyEmail(dtoUserCode: dtoUserCode) : Observable<string> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post<string>(this.sendCodeUrl, dtoUserCode, {headers, responseType: 'text' as 'json'} );
  }

  resendCodeToVerifyEmail(email: string, type: string) : Observable<string> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post<string>(`${this.resendCodeUrl}/${type}`, JSON.stringify(email), {headers, responseType: 'text' as 'json'} );
  }

  
}
