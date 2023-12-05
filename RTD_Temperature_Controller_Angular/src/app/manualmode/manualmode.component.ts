import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ManualmodeService } from '../services/manualmode.service';
import { Command } from '../models/Command';
import { HubService } from '../services/hub.service';
import { ConnectionService } from '../services/connection.service';
import { Router } from '@angular/router';
import { DEVICE_COMMANDS } from 'src/commands/commands';

/**
 * Component for manual mode functionality.
 */

@Component({
  selector: 'app-manualmode',
  templateUrl: './manualmode.component.html',
  styleUrls: ['./manualmode.component.scss']
})
export class ManualmodeComponent implements OnDestroy {

  PwmFormGroup: FormGroup;
  Submitted: boolean = false
  NewCommand:Command = new Command('','')
  Input: string = ""

  constructor(fb: FormBuilder,private router_service: Router,private connection_service: ConnectionService,private manualmodeService:ManualmodeService, private hub_service: HubService){
    this.NewCommand.Command="Set"
    this.NewCommand.Value = "SET MOD MAN\r"
    this.manualmodeService.sendCommand(this.NewCommand).subscribe(data=>{
    })
    this.PwmFormGroup = fb.group({
      pwmCycleControl: ['', [Validators.required, Validators.pattern("^0*(?:[0-9][0-9]?|100)$")]]  
    })
    hub_service.hubConnection.on('ManualModeData', (manualmodedata: any) => {
      this.Input = manualmodedata.value
    });

  }

  /**
   * Cleanup method executed when the component is destroyed.
   */

  ngOnDestroy(): void {
    this.hub_service.closeManualMode()
    this.NewCommand.Command="Set"
    this.NewCommand.Value = DEVICE_COMMANDS.SET_AUTOMATIC_MODE
    this.manualmodeService.sendCommand(this.NewCommand).subscribe(data=>{
    })
  }

  /**
   * Getter for accessing PWM form controls.
   */

  get getPWMFormControls(){
    return this.PwmFormGroup.controls;
  }

  /**
   * Method for setting LED 1 state.
   */

  setLed1(){
    const ele = document.getElementById('led1') as HTMLInputElement
    if(ele.checked){
      this.NewCommand.Value=DEVICE_COMMANDS.SET_RLED_ON
    }
    else{
      this.NewCommand.Value=DEVICE_COMMANDS.SET_RLED_OFF
    }
    this.NewCommand.Command="Set"
    this.manualmodeService.sendCommand(this.NewCommand).subscribe(data=>{
    })
  }

  /**
   * Method for setting LED 2 state.
   */

  setLed2(){
    const ele = document.getElementById('led2') as HTMLInputElement
    if(ele.checked){
      this.NewCommand.Value=DEVICE_COMMANDS.SET_GLED_ON
    }
    else{
      this.NewCommand.Value=DEVICE_COMMANDS.SET_GLED_OFF
    }
    this.NewCommand.Command="Set"
    this.manualmodeService.sendCommand(this.NewCommand).subscribe(data=>{
    })
  }

  /**
   * Method for setting LED 3 state.
   */

  setLed3(){
    const ele = document.getElementById('led3') as HTMLInputElement
    if(ele.checked){
      this.NewCommand.Value=DEVICE_COMMANDS.SET_BLED_ON
    }
    else{
      this.NewCommand.Value=DEVICE_COMMANDS.SET_BLED_OFF
    }
    this.NewCommand.Command="Set"
    this.manualmodeService.sendCommand(this.NewCommand).subscribe(data=>{
    })
  }

  /**
   * Method for setting PWM duty cycle.
   */

  setPWMDutyCycle(){
    this.Submitted = true;
    if (this.PwmFormGroup.invalid) return;

    this.manualmodeService.sendCommand(new Command("SET", DEVICE_COMMANDS.SET_PWM + this.PwmFormGroup.value.pwmCycleControl+"\r")).subscribe(d =>{})
  }

  /**
   * Method for retrieving temperature data.
   */

  getTemperature(){
    this.manualmodeService.sendCommand(new Command("GET", DEVICE_COMMANDS.GET_TEMPERATURE)).subscribe(d =>{})
  }

  /**
   * Method for retrieving resistance data.
   */

  getResistance(){
    this.manualmodeService.sendCommand(new Command("GET", DEVICE_COMMANDS.GET_RESISTANCE)).subscribe(d =>{})
  }

  /**
   * Method for retrieving EEPROM data.
   */

  getEEPROM(){
    this.manualmodeService.sendCommand(new Command("GET", DEVICE_COMMANDS.GET_EEPROM)).subscribe(d =>{})
  }

  /**
   * Method for retrieving "start button" status.
   */

  getStartButtonStatus(){
    this.manualmodeService.sendCommand(new Command("GET", DEVICE_COMMANDS.GET_START_BUTTON)).subscribe(d =>{})
  }

  /**
   * Method for retrieving "stop button" status.
   */

  getStopButtonStatus(){
    this.manualmodeService.sendCommand(new Command("GET", DEVICE_COMMANDS.GET_STOP_BUTTON)).subscribe(d =>{})
  }
}
