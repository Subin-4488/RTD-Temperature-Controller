import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class HubService {
  //write about varoius  event names as a way for the server to broadcast specific information to all connected clients or to target messages to specific groups of clients.
  hubConnection: signalR.HubConnection;
  constructor() {
    console.log("hi")
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:3000/temperatureHub',{ withCredentials: false })
      .build();
    this.hubConnection.start()

  }

  closeAutomatic(){
    this.hubConnection.off('UpdateTemperature')
  }
  closeManualMode(){
    this.hubConnection.off('manualmodedata')
  }
  closeError(){
    this.hubConnection.off('DeviceError') 
  }
  end(){
    this.closeError()
    this.hubConnection.stop()
  }
}
