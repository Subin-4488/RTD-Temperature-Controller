import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ConnectionService } from '../services/connection.service';
import { HubService } from '../services/hub.service';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.scss']
})
export class BannerComponent {

  constructor(private router_service: Router
    ,private connectionService:ConnectionService
    ,private hub_service: HubService){
  }

  disconnect(){
    //write the disconnecting code
    let ifDisconnected
    this.connectionService.disconnectConnection().subscribe(data=>{
      
      ifDisconnected = data
      console.log(ifDisconnected)
      if(ifDisconnected==true)
        this.router_service.navigate(["/"]);
    })

    this.hub_service.close()
    
  }
}