import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Settings } from '../models/Settings';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  baseurl="https://localhost:3000";

  httpHeader={
    headers:new HttpHeaders({
      'Content-Type':'application/json'
    })}

  constructor(private httpClient:HttpClient) { 

  }


  httpError(error:HttpErrorResponse){
    let msg=''
    if(error.error instanceof ErrorEvent){
      msg=error.error.message
    }
    else{
      msg='Error Code: ${error.status}\n Message: ${error.message}';
    }
    console.log(msg)
    return throwError(msg)

  }

  updateSettings(s:Settings):Observable<Settings>{
    return this.httpClient.post<Settings>(this.baseurl+'/settings',JSON.stringify(s),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }

  resetSettings():Observable<Settings>{
    return this.httpClient.get<Settings>(this.baseurl+'/settings')
    .pipe(
      catchError(this.httpError)
    );
  }

}
