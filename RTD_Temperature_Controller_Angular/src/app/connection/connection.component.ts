import { Component } from '@angular/core';
import { FormBuilder, FormGroup  } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-connection',
  templateUrl: './connection.component.html',
  styleUrls: ['./connection.component.scss']
})

export class ConnectionComponent {
  connectionForm:FormGroup


  constructor(fb:FormBuilder,private router:Router){
    this.connectionForm=fb.group({
      bps:[9600],
      databits:[8],
      parity:['None'],
      stopbits:[1],
    })
  }

  onSubmit(value:string){
    console.log(value)
    this.router.navigate(['dashboard'])
  }
}
