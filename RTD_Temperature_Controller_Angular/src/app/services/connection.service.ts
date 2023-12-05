import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Connection } from '../models/Connection';
import { Observable, catchError, throwError } from 'rxjs';
import { HubService } from './hub.service';
import { environment } from 'src/environments/environment';
import { API_ENDPOINTS } from 'src/config/apiConfig';

/**
 * Service for managing connections
 */

@Injectable({
  providedIn: 'root',
})
export class ConnectionService {
  
  private baseurl = environment.baseURL;

  private  httpHeader = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient, private hubService: HubService) {}

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
   * Creates a new connection
   * @param connection The connection details required to start the connection
   * @returns An observable with a boolean indicating success
   */

  createConnection(connection: Connection): Observable<boolean> {
    return this.httpClient
      .post<boolean>(
        this.baseurl + API_ENDPOINTS.createConnection,
        JSON.stringify(connection),
        this.httpHeader
      )
      .pipe(catchError(this.httpError));
  }

  /**
   * Disconnects the current connection
   * @returns An observable with a boolean indicating success
   */

  disconnectConnection(): Observable<boolean> {
    this.hubService.end();
    return this.httpClient
      .post<boolean>(
        this.baseurl + API_ENDPOINTS.disconnectConnection,
        '',
        this.httpHeader
      )
      .pipe(catchError(this.httpError));
  }

  /**
   * Sets the mode of the connection to 'Automatic'
   * @returns An observable with a boolean indicating success
   */

  setMode(): Observable<boolean> {
    return this.httpClient
      .post<boolean>(this.baseurl + API_ENDPOINTS.setMode, '', this.httpHeader)
      .pipe(catchError(this.httpError));
  }
  
  /**
   * Gets the list of port names avaliable in the computer
   * @returns  An observable with an array of port names
   */

  getportNames(): Observable<string[]> {
    return this.httpClient
      .get<string[]>(this.baseurl + API_ENDPOINTS.getportNames)
      .pipe(catchError(this.httpError));
  }
}
