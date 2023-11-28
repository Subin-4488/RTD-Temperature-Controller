import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HubService } from '../services/hub.service';
import { ConnectionService } from '../services/connection.service';

//////////////////////////////////////////////////////////////////////////
/// <summary>
/// The component  acts as the parent for Banner and Content(different pages) components
/// </summary>
/// <remarks>
/// To capture the "Device Disconnected" interruption from any screen, the Dashboard component initializes a listener for the 'DeviceError' event on the SignalR hub connection from the server. 
/// This listener, set up through hub_service.hubConnection.on('DeviceError', ...), ensures that the Dashboard component is responsive to such events, allowing it to handle 'Device Disconnected' interruptions regardless of the current screen. 
/// </remarks>
//////////////////////////////////////////////////////////////////////////

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent {
  constructor(
    private hubService: HubService,
    private connectionService: ConnectionService,
    private routerService: Router
  ) {
    hubService.hubConnection.on('DeviceError', (data) => {
      if (data != null && data.error == 'Device disconnected') {
        alert('Connection Interupted!!\nExiting...');
        connectionService.disconnectConnection().subscribe((d) => {
          this.routerService.navigate(['/']).then(() => {
            window.location.reload();
          });
        });
      }
    });
  }
}
