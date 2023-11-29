import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';


/**
 * This service is used for creating and disposing a new hub connection.
 * 
 * This service listens to 3 events: 
 * 
 * UpdateTemperature: All the temperature reading in the automatic mode will be coming here
 * 
 * ManualModeData: All the return messages in the manual mode will be coming here
 * 
 * DeviceError: Any device errors will be coming here from the backend
 */

@Injectable({
  providedIn: 'root'
})
export class HubService {
  hubConnection: signalR.HubConnection;
  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:3000/temperatureHub',{ withCredentials: false })
      .build();
    this.hubConnection.start()
  }

  /**
   * Function to stop listening to evnet UpdateTemperature
   */
  closeAutomatic(){
    this.hubConnection.off('UpdateTemperature')
  }

  /**
   * Function to stop listening to evnet ManualModeData
   */
  closeManualMode(){
    this.hubConnection.off('ManualModeData')
  }

  /**
   * Function to stop listening to evnet DeviceError
   */
  closeError(){
    this.hubConnection.off('DeviceError') 
  }

  /**
   * Function to stop the hub connection
   */
  end(){
    this.closeError()
    this.hubConnection.stop()
  }
}
