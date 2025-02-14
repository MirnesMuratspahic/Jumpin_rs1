import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { RegisterData } from '../models/register-data.model';
import { HandleErrorService } from '../../../services/errorService/handle-error.service';

@Injectable({
  providedIn: 'root',
})
export class RegisterPageService {
  private registerUrl = `${environment.apiBaseUrl}/User/Registration`;

  constructor(private http: HttpClient, private handleError: HandleErrorService) {}

  register(userData: RegisterData): Observable<string> {
    return this.http.post<string>(this.registerUrl, userData, {responseType: 'text' as 'json'});
  }
}
