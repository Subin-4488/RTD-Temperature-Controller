import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ConnectionService } from '../services/connection.service';
import { Connection } from '../models/Connection';

/**
 * The component shall deal with the connection and disconnection between the software and hardware.
 */

@Component({
  selector: 'app-connection',
  templateUrl: './connection.component.html',
  styleUrls: ['./connection.component.scss'],
})
export class ConnectionComponent {
  ConnectionForm: FormGroup;
  PortNames: string[] = [];
  ConnectionStatus: boolean = false;

  constructor(
    fb: FormBuilder,
    private router: Router,
    private connectionService: ConnectionService
  ) {
    localStorage.setItem("connected","false")
    this.ConnectionForm = fb.group({
      portname: [],
      bps: [9600],
      databits: [8],
      parity: ['None'],
      stopbits: [1],
    });
    connectionService.getportNames().subscribe((data) => {
      this.PortNames = data;
    });
  }

  /**
   * The OnSubmit method tries to create a connection with hardware if successful, the user is navigated to dashboard screen otherwise an error message is displayed.
   * @param value It represents a string object containing the current values of the controls within the form group(ConnectionForm)
   */

  onSubmit(val: string) {
    let newConnection = new Connection(
      this.ConnectionForm.value.portname,
      this.ConnectionForm.value.bps,
      this.ConnectionForm.value.databits,
      this.ConnectionForm.value.parity,
      this.ConnectionForm.value.stopbits
    );
    this.connectionService.createConnection(newConnection).subscribe((data) => {
      this.ConnectionStatus = data;
      if (this.ConnectionStatus == true){
        localStorage.setItem("connected","true")
        this.router.navigateByUrl('dashboard/(navRoute:home)');
      }
      else {
        alert('Connection error. Try changing the values.');
      }
    });
  }

  /**
   * The ResetValues method used to reset the connection parameters in the screen to its default values.
   */

  resetValues() {
    this.ConnectionForm.patchValue({
      bps: 9600,
      databits: 8,
      parity: 'None',
      stopbits: 1,
    });
  }
}
