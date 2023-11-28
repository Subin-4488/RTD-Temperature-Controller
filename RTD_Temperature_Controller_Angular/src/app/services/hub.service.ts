import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

//////////////////////////////////////////////////////////////////////////
/// <summary>
/// This service is used for creating and disposing
/// a new hub connection.
/// </summary>
/// <remarks>
/// This service listens to 3 events: 
///   UpdateTemperature: All the temperature reading in the automatic mode will be coming here
///   ManualModeData: All the return messages in the manual mode will be coming here
///   DeviceError: Any device errors will be coming here from the backend
/// </remarks>
//////////////////////////////////////////////////////////////////////////

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

  /// <summary>
  /// Function to stop listening to evnet UpdateTemperature
  /// </summary>
  closeAutomatic(){
    this.hubConnection.off('UpdateTemperature')
  }

  /// <summary>
  /// Function to stop listening to evnet ManualModeData
  /// </summary>
  closeManualMode(){
    this.hubConnection.off('ManualModeData')
  }

  /// <summary>
  /// Function to stop listening to evnet DeviceError
  /// </summary>
  closeError(){
    this.hubConnection.off('DeviceError') 
  }

  /// <summary>
  /// Function to stop the hub connection
  /// </summary>
  end(){
    this.closeError()
    this.hubConnection.stop()
  }
}
