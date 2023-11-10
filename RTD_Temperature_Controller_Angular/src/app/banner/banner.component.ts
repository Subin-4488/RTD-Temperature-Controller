import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-banner',
  templateUrl: './banner.component.html',
  styleUrls: ['./banner.component.scss']
})
export class BannerComponent {

  constructor(private router_service: Router){
  }

  disconnect(){
    //write the disconnecting code
    this.router_service.navigate(["/"]);
  }
}
  