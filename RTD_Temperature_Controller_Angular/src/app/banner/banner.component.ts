import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ConnectionService } from '../services/connection.service';

//////////////////////////////////////////////////////////////////////////
/// <summary>
/// The component shall serve as the placeholder for the Home, Settings, ManualMode pages and Disconnect button
/// </summary>
//////////////////////////////////////////////////////////////////////////

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.scss'],
})
export class BannerComponent {
  constructor(
    private routerService: Router,
    private connectionService: ConnectionService
  ) {}

  /// <summary>
  /// The Disconnect method used to trigger the disconnection between software and hardware through “ConnectionService” and navigates to the homepage if it is successful.
  /// </summary>
  /// <returns>
  /// NIL
  /// </returns
  
  disconnect() {
    let ifDisconnected;
    this.connectionService.disconnectConnection().subscribe((data) => {
      ifDisconnected = data;
      if (ifDisconnected == true) {
        this.routerService.navigate(['/']).then(() => {
          window.location.reload();
        });
      }
    });
  }
}