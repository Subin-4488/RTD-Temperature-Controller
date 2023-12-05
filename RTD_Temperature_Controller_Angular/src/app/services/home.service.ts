import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Command } from '../models/Command';
import { environment } from 'src/environments/environment';

/**
 * Service responsible for handling communication with the server related to home functionality.
 */

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  private baseurl = environment.baseURL;

  private httpHeader = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) {}

  /**
   * Handles HTTP errors
   * @param error The HTTP error response
   * @returns An observable with an error message
   */

  private httpError(error: HttpErrorResponse) {
    let msg = '';
    if (error.error instanceof ErrorEvent) {
      msg = error.error.message;
    } else {
      msg = 'Error Code: ${error.status}\n Message: ${error.message}';
    }
    return throwError(msg);
  }

  /**
   * Sends a command to the server
   * @param val The command to be sent
   * @returns An observable that emits a boolean indicating the success of the operation
   */

  sendCommand(val: Command): Observable<boolean> {
    return this.httpClient
      .post<boolean>(
        this.baseurl + '/home',
        JSON.stringify(val),
        this.httpHeader
      )
      .pipe(catchError(this.httpError));
  }
}
