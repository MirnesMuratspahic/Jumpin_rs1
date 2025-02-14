import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  private isLocalStorageAvailable = typeof localStorage !== 'undefined';

  private roleLocalStorageAvailable(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  getEmailFromToken(): string | null {
    const token = localStorage.getItem('token');

    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        return decodedToken.sub;
      } catch (error) {
        console.error('Error decoding token:', error);
        return null;
      }
    } else {
      console.error('Token not found in local storage');
      return null;
    }
  }

  getRoleFromToken(): string | null {
    if (!this.roleLocalStorageAvailable()) {
      return null;
    }

    const token = localStorage.getItem('token');

    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        return (
          decodedToken.role ||
          decodedToken[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ] ||
          null
        );
      } catch (error) {
        return null;
      }
    } else {
      console.error('Token not found!');
      return null;
    }
  }
}
