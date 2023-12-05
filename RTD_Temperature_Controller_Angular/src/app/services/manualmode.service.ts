import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Command } from '../models/Command';
import { environment } from 'src/environments/environment';
import { API_ENDPOINTS } from 'src/config/apiConfig';

/**
 * This service handles all the requests from the the ManualMode Component
 * It includes sending the commands for manual mode to the backend
 */

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

  /**
   * Function to handle HTTP errors
   * @param error This is of type HttpErrorResponse
   * @returns Returns an observable with error message
   */

  private httpError(error:HttpErrorResponse){
    let msg=''
    if(error.error instanceof ErrorEvent){
      msg=error.error.message
    }
    else{
      msg='Error Code: ${error.status}\n Message: ${error.message}';
    }
    return throwError(() => msg);
  }

  /**
   * Function to send the manual mode commands
   * @param val This is of type Command. Holds the new command
   * @returns An observable with a boolean indicating success.
   */

  sendCommand(val: Command): Observable<boolean> {
    console.log(val.Value);
    return this.httpClient
      .post<boolean>(
        this.baseUrl + API_ENDPOINTS.manualMode,
        JSON.stringify(val),
        this.httpHeader
      )
      .pipe(catchError(this.httpError));
  }
}