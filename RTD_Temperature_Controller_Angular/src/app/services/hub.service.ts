import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  //temperature provider
  temperatureFromSensor: number[] = [];


  public hubConnection: signalR.HubConnection;
  constructor() {
    console.log("sadfasfasfsdfasfas")
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:3000/temperatureHub',{ withCredentials: false })
      .build();

    this.hubConnection.start().then(() => {
      console.log('Connection started');
    });
    console.log("registered callback...")
    
    console.log("finished registering callback...")
    
  }

  readSocket(){
    this.hubConnection.on('UpdateTemperature', (temperature: any) => {
      //this.temperature = temperature;
      console.log(temperature.time);
      console.log(temperature.temperature)
    });
  }

  close(){
    this.hubConnection.off('UpdateTemperature')
  }

}
