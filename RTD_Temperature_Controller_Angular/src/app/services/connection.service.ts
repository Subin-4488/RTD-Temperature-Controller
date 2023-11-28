import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Connection } from '../models/Connection';
import { Observable, catchError, throwError } from 'rxjs';
import { HubService } from './hub.service';

//////////////////////////////////////////////////////////////////////////
/// <summary>
/// Service for managing connections.
/// </summary>
//////////////////////////////////////////////////////////////////////////

@Injectable({
  providedIn: 'root',
})
export class ConnectionService {
  baseurl = 'https://localhost:3000';

  httpHeader = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient, private hubService: HubService) {}

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
  /// Creates a new connection.
  /// </summary>
  /// <returns>
  /// Return results are described through the returns tag.
  /// </returns>
  /// <param name="connection"> The connection details required to start the connection</param>
  ///<Exceptions>
  /// Network-related errors, HTTP errors are likely to be arised
  ///</Exceptions>

  createConnection(connection: Connection): Observable<boolean> {
    return this.httpClient
      .post<boolean>(
        this.baseurl + '/connection',
        JSON.stringify(connection),
        this.httpHeader
      )
      .pipe(catchError(this.httpError));
  }

  /// <summary>
  /// Disconnects the current connection.
  /// </summary>
  /// <returns>
  /// An observable with a boolean indicating success.
  /// </returns>
  ///<Exceptions>
  /// Network-related errors, HTTP errors are likely to be arised
  ///</Exceptions>

  disconnectConnection(): Observable<boolean> {
    this.hubService.end();
    return this.httpClient
      .post<boolean>(
        this.baseurl + '/connection/disconnect/',
        '',
        this.httpHeader
      )
      .pipe(catchError(this.httpError));
  }

  /// <summary>
  /// Sets the mode of the connection to 'Automatic'.
  /// </summary>
  /// <returns>
  /// An observable with a boolean indicating success.
  /// </returns>
  ///<Exceptions>
  /// Network-related errors, HTTP errors are likely to be arised
  ///</Exceptions>

  setMode(): Observable<boolean> {
    return this.httpClient
      .post<boolean>(this.baseurl + '/connection/setatm/', '', this.httpHeader)
      .pipe(catchError(this.httpError));
  }

  /// <summary>
  /// Gets the list of port names avaliable in the computer.
  /// </summary>
  /// <returns>
  /// An observable with an array of port names.
  /// </returns>
  ///<Exceptions>
  /// Network-related errors, HTTP errors are likely to be arised
  ///</Exceptions>

  getportNames(): Observable<string[]> {
    return this.httpClient
      .get<string[]>(this.baseurl + '/connection/ports/')
      .pipe(catchError(this.httpError));
  }
}
