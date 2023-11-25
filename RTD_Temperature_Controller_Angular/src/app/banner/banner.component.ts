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
    this.hub_service.end()
    // this.connectionService.setMode().subscribe(d=>{
      
    // })
    this.connectionService.disconnectConnection().subscribe(data=>{
      
      ifDisconnected = data
      if(ifDisconnected==true){
        this.router_service.navigate(["/"]).then(() => {
          window.location.reload()
        })
      }
    })

    
  
  }
}