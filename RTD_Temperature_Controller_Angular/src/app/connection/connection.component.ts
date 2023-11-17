import { Component } from '@angular/core';
import { FormBuilder, FormGroup  } from '@angular/forms';
import { Router } from '@angular/router';
import { ConnectionService } from '../services/connection.service';
import { Connection } from '../models/Connection';

@Component({
  selector: 'app-connection',
  templateUrl: './connection.component.html',
  styleUrls: ['./connection.component.scss']
})

export class ConnectionComponent {
  connectionForm:FormGroup
  portNames:string[] = []
  connectionStatus:boolean=false

  constructor(fb:FormBuilder,private router:Router,private connectionService:ConnectionService){
    this.connectionForm=fb.group({
      portname:[],
      bps:[9600],
      databits:[8],
      parity:['None'],
      stopbits:[1]
    })
    connectionService.getportNames().subscribe(data=>{
      this.portNames = data
    })
  }

  onSubmit(value:string){
    let newConnection = new Connection(this.connectionForm.value.portname,this.connectionForm.value.bps,this.connectionForm.value.databits,this.connectionForm.value.parity,this.connectionForm.value.stopbits)
    this.connectionService.createConnection(newConnection).subscribe(data=>{
      this.connectionStatus=data;
      if(this.connectionStatus == true)
        this.router.navigateByUrl('dashboard/(navRoute:home)')
      else{
        alert("Connection error. Try changing the values.")
      }
    })
  }

  resetValues(){
    this.connectionForm.patchValue({
      bps:9600,
      databits:8,
      parity:'None',
      stopbits:1
    })
  }
}
