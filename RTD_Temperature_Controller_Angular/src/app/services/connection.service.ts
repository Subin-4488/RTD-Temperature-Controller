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

  createConnection(c:Connection):Observable<boolean>{
    console.log(JSON.stringify(c))
    return this.httpClient.post<boolean>(this.baseurl+'/connection',JSON.stringify(c),this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }

  disconnectConnection():Observable<boolean>{
    return this.httpClient.post<boolean>(this.baseurl+'/connection/disconnect/','',this.httpHeader)
    .pipe(
      catchError(this.httpError)
    );
  }

  getportNames():Observable<string[]>{
    return this.httpClient.get<string[]>(this.baseurl+'/connection/ports/')
    .pipe(
      catchError(this.httpError)
    );
  }

}
