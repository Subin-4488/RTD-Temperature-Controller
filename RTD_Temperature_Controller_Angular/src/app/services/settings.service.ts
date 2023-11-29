import {
  HttpHeaders,
  HttpClient,
  HttpErrorResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Settings } from '../models/Settings';
import { environment } from 'src/environments/environment';

/**
 * This service handles all the requests from the settingsComponent.
 * 
 * It includes getting the configuration from the backend as well as to set new configuration.
 */

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  private baseUrl = environment.baseURL;
  private httpHeader = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) {}

  /**
   * Function to handle HTTP errors
   * @param error This is of type HttpErrorResponse
   * @returns Returns an observable with error message
   */
  
  private handleHttpError(error: HttpErrorResponse) {
    let msg = '';
    if (error.error instanceof ErrorEvent) {
      msg = error.error.message;
    } else {
      msg = 'Error Code: ${error.status}\n Message: ${error.message}';
    }
    return throwError(() => msg);
  }

  /**
   * Function to update the settings configuration
   * @param s This is of type Settings. Holds the new Settings values
   * @returns An observable with the updated settings
   */

  updateSettings(s: Settings): Observable<any> {
    return this.httpClient
      .post<Settings>(
        this.baseUrl + '/settings',
        JSON.stringify(s),
        this.httpHeader
      ).pipe(catchError(this.handleHttpError));
  }
  
  /**
   * Function to retrieve the stored settings value
   * @returns An observable with the stored settings
   */
  resetSettings(): Observable<Settings> {
    return this.httpClient
      .get<Settings>(this.baseUrl + '/settings')
      .pipe(catchError(this.handleHttpError));
  }
}
