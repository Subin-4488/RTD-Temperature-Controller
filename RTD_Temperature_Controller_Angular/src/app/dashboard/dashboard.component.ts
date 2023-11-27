import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HubService } from '../services/hub.service';
import { ConnectionService } from '../services/connection.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {


  constructor(private hub_service: HubService, private connection_service: ConnectionService,
    private router_service:Router){  //Initializing error hub

    hub_service.hubConnection.on('DeviceError', (data)=>{
      if (data!=null && data.Error  == "Device disconnected"){
        alert("Connection Interupted!!\nExiting...")
      console.log("NOWWWW")
        connection_service.disconnectConnection().subscribe(d=>{
          this.router_service.navigate(["/"]).then(() => {
            window.location.reload()
          })
        })
      }
    })
  }
}
