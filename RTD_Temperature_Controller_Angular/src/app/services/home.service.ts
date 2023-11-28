import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Command } from '../models/Command';

//////////////////////////////////////////////////////////////////////////
/// <summary>
/// Service responsible for handling communication with the server related to home functionality.
/// </summary>
//////////////////////////////////////////////////////////////////////////

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  baseurl = 'https://localhost:3000';

  httpHeader = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) {}

  /// <summary>
  /// Handles HTTP errors
  /// </summary>
  /// <returns>
  /// An observable with an error message.
  /// </returns>
  /// <param name="error">The HTTP error response.</param>

  private httpError(error: HttpErrorResponse) {
    let msg = '';
    if (error.error instanceof ErrorEvent) {
      msg = error.error.message;
    } else {
      msg = 'Error Code: ${error.status}\n Message: ${error.message}';
    }
    return throwError(msg);
  }

  /// <summary>
  /// Sends a command to the server.
  /// </summary>
  /// <returns>
  /// An observable that emits a boolean indicating the success of the operation.
  /// </returns>
  /// <param name="val">The command to be sent.</param>
  ///<Exceptions>
  /// Network-related errors, HTTP errors are likely to be arised
  ///</Exceptions

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
