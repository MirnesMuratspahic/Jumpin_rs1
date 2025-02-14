import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { UserRoute } from '../../home-page/models/userRoute';
import { HeaderService } from '../../../services/headerService/header.service';
import { HandleErrorService } from '../../../services/errorService/handle-error.service';
import { dtoRating } from '../models/dtoRating.model';
import { dtoUserRating } from '../models/dtoUserRating.model';
import { each } from 'chart.js/dist/helpers/helpers.core';

@Injectable({
  providedIn: 'root'
})
export class RatingPageService {

  constructor(private http: HttpClient, private headerService: HeaderService, private handleError: HandleErrorService) {}

  private addRatingUrl = `${environment.apiBaseUrl}/Rating/AddRating`;
  private getUserReviewsUrl = `${environment.apiBaseUrl}/Rating/GetUsersRatings`;
  private deleteRatingUrl = `${environment.apiBaseUrl}/Rating/DeleteRating`;

  addRating(dtoRating: dtoRating) : Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.post<string>(this.addRatingUrl, dtoRating, {headers, responseType: 'text' as 'json'});
  }

  getUserReviews(email: string) : Observable<dtoUserRating[]> {
    const headers = this.headerService.createHeaders();
    return this.http.post<dtoUserRating[]>(this.getUserReviewsUrl, JSON.stringify(email), {headers})
    .pipe(catchError((error) => this.handleError.handleError(error)));;
  }

  deleteRating(reviewId: number) : Observable<string> {
    const headers = this.headerService.createHeaders();
    return this.http.delete<string>(`${this.deleteRatingUrl}/${reviewId}`, {headers, responseType: 'text' as 'json'});
  }
}
