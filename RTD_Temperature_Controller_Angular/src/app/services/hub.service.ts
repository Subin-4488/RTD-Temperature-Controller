import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  //temperature provider
  temperatureFromSensor: any[] = [];


  public hubConnection: signalR.HubConnection;
  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:3000/temperatureHub',{ withCredentials: false })
      .build();

    this.hubConnection.start().then(() => {
      this.hubConnection.on('UpdateTemperature', (temperature: any) => {
        this.temperatureFromSensor.push(temperature)
        // console.log(temperature.time+" : "+temperature.temperature)
      });
    });    
  }

  dummy(){
    console.log()
  }

  close(){
    this.hubConnection.off('UpdateTemperature')
  }

}
