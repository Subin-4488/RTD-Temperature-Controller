import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  public hubConnection: signalR.HubConnection;
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
  end(){
    this.hubConnection.stop()
  }

}
