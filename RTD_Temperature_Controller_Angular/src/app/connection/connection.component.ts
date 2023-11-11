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


  constructor(fb:FormBuilder,private router:Router,private connectionService:ConnectionService){
    this.connectionForm=fb.group({
      bps:[9600],
      databits:[8],
      parity:['None'],
      stopbits:[1],
    })
  }

  onSubmit(value:string){
    console.log(value)
    let newConnection = new Connection(this.connectionForm.value.bps,this.connectionForm.value.databits,this.connectionForm.value.parity,this.connectionForm.value.stopbits)
    this.connectionService.createConnection(newConnection).subscribe(data=>{})
    //this.router.navigateByUrl('dashboard/(navRoute:home)')
  }
}
