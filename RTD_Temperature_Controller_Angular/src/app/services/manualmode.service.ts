import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Command } from '../models/Command';
import { environment } from 'src/environments/environment';

//////////////////////////////////////////////////////////////////////////
/// <summary>
/// This service handles all the requests from the the ManualMode Component
/// It includes sending the commands for manual mode to the backend
/// </summary>
//////////////////////////////////////////////////////////////////////////

@Injectable({
  providedIn: 'root',
})
export class ManualmodeService {
  private baseUrl = environment.baseURL;
  private httpHeader = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) {}

  /// <summary>
  /// Function to handle HTTP errors
  /// </summary>
  /// <returns>
  /// Returns an observable with error message
  /// </returns>
  /// <param name="error">This is of type HttpErrorResponse</param>
  httpError(error: HttpErrorResponse) {
    let msg = '';
    if (error.error instanceof ErrorEvent) {
      msg = error.error.message;
    } else {
      msg = 'Error Code: ${error.status}\n Message: ${error.message}';
    }
    return throwError(() => msg);
  }

  /// <summary>
  /// Function to send the manual mode commands
  /// </summary>
  ///  <returns>
  /// An observable with a boolean indicating success.
  /// </returns>
  /// <param name="val">This is of type Command. Holds the new command</param>
  ///<Exceptions>
  /// Network-related errors, HTTP errors are likely to be arised
  ///</Exceptions>
  sendCommand(val: Command): Observable<boolean> {
    console.log(val.Value);
    return this.httpClient
      .post<boolean>(
        this.baseUrl + '/manualmode',
        JSON.stringify(val),
        this.httpHeader
      )
      .pipe(catchError(this.httpError));
  }
}
