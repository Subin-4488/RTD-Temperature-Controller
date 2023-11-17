import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  //temperature provider
  temperatureFromSensor: number[] = [];


  private hubConnection: signalR.HubConnection;
  constructor() {
    console.log("sadfasfasfsdfasfas")
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:3000/temperatureHub',{ withCredentials: false })
      .build();

    this.hubConnection.start().then(() => { 
      console.log("READ SOCKET....")
      this.hubConnection.on('UpdateTemperature', (temperature: number) => {
        //this.temperature = temperature;
        this.temperatureFromSensor.push(temperature);
      })
    });
  }

  close(){
    this.hubConnection.off('UpdateTemperature')
  }

}
