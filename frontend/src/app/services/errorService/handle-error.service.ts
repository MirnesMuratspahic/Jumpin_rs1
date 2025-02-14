import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HandleErrorService {

  constructor() { }

  public handleError(error: HttpErrorResponse) {
    if (error.status === 400) {
      return throwError(() => ({ status: error.status, message: error.error.name || 'Error occurred!' }));
    }else if(error.status === 500){
      return throwError(() => ({ status: error.status, message: 'Server error occurred!'}));
    } 
    else {
      return throwError(() => ({ status: error.status, message: 'Error occurred!' }));
    }
  }
}

