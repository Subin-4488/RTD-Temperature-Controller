import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Connection } from '../models/Connection';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConnectionService {

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

  createConnection(c:Connection):Observable<Connection>{
    console.log(JSON.stringify(c))
    return this.httpClient.post<Connection>(this.baseurl+'/connection',JSON.stringify(c),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }



}