import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Command } from '../models/Command';

@Injectable({
  providedIn: 'root'
})
export class ManualmodeService {

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

  sendCommand(val:Command):Observable<boolean>{
    console.log(val)
    return this.httpClient.post<boolean>(this.baseurl+'/manualmode',JSON.stringify(val),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }

}
