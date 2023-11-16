import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  private hubConnection: signalR.HubConnection;
  constructor() {
    console.log("sadfasfasfsdfasfas")
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:3000/temperatureHub',{ withCredentials: false })
      .build();

    this.hubConnection.start().then(() => {
      console.log('Connection started');
    });
    console.log("registered callback...")
    this.hubConnection.on('UpdateTemperature', (temperature: number) => {
      //this.temperature = temperature;
      console.log(`Received temperature update: ${temperature}`);
    });
    console.log("finished registering callback...")
    
  }

  readSocket(){
    
  }

  close(){
    this.hubConnection.off('UpdateTemperature')
  }

}
