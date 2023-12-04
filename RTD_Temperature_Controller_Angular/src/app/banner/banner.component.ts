import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ConnectionService } from '../services/connection.service';

/**
 * The component shall serve as the placeholder for the Home, Settings, ManualMode pages and Disconnect button
 */

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

  /**
   * The Disconnect method used to trigger the disconnection between software and hardware through “ConnectionService” and navigates to the homepage on success.
   */

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
