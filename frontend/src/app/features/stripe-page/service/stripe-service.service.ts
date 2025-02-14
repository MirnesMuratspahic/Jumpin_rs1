import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { HandleErrorService } from '../../../services/errorService/handle-error.service';
import { HeaderService } from '../../../services/headerService/header.service';

@Injectable({
  providedIn: 'root'
})
export class StripeService {

  private checkoutSesionUrl = `${environment.apiBaseUrl}/Stripe/CheckoutSesion`;
  private upgradeUsersAccountUrl = `${environment.apiBaseUrl}/User/UpgradeUsersAccount`;

  constructor(private http: HttpClient, private handleError: HandleErrorService, private headerService: HeaderService) {}

  checkoutSesion(amount: number) :Observable<any>{
    const headers = this.headerService.createHeaders();
    return this.http.post<any>(`${this.checkoutSesionUrl}/${amount}`, {}, {headers, responseType: 'text' as 'json'});
  }

  upgradeUsersAccount(email: string) :Observable<any>{
    const headers = this.headerService.createHeaders();
    return this.http.post<any>(this.upgradeUsersAccountUrl, JSON.stringify(email), {headers, responseType: 'text' as 'json'});
  }


}
