import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ManualmodeService } from '../services/manualmode.service';
import { Command } from '../models/Command';
import { HubService } from '../services/hub.service';

@Component({
  selector: 'app-manualmode',
  templateUrl: './manualmode.component.html',
  styleUrls: ['./manualmode.component.scss']
})
export class ManualmodeComponent implements OnDestroy {

  pwmFormGroup: FormGroup;
  submitted: boolean = false
  newCommand:Command = new Command('','')
  input: string = ""

  constructor(fb: FormBuilder,private manualmodeService:ManualmodeService, private hub_service: HubService){
    this.newCommand.Command="Set"
    this.newCommand.Value = "SET MOD MAN\r"
    this.manualmodeService.sendCommand(this.newCommand).subscribe(data=>{
      console.log("manual mode done")
    })
    this.pwmFormGroup = fb.group({
      pwmCycleControl: ['', [Validators.required, Validators.pattern("^0*(?:[0-9][0-9]?|100)$")]]  
    })
    hub_service.hubConnection.on('manualmodedata', (manualmodedata: any) => {
      console.log(manualmodedata)
      this.input = manualmodedata.value
    });
  }
  ngOnDestroy(): void {
    this.hub_service.close()
    this.newCommand.Command="Set"
    this.newCommand.Value = "SET MOD ATM\r"
    this.manualmodeService.sendCommand(this.newCommand).subscribe(data=>{
      console.log("manual mode done")
    })
  }


  get getPWMFormControls(){
    return this.pwmFormGroup.controls;
  }

  setLed1(){
    const ele = document.getElementById('led1') as HTMLInputElement
    if(ele.checked){
      this.newCommand.Value="SET LED ON 1\r"
    }
    else{
      this.newCommand.Value="SET LED OFF 1\r"
    }
    this.newCommand.Command="Set"
    //this.newCommand.Value="SET LED ON 1\r"
    this.manualmodeService.sendCommand(this.newCommand).subscribe(data=>{
    })
  }
  setLed2(){
    const ele = document.getElementById('led2') as HTMLInputElement
    if(ele.checked){
      this.newCommand.Value="SET LED ON 2\r"
    }
    else{
      this.newCommand.Value="SET LED OFF 2\r"
    }
    this.newCommand.Command="Set"
    //this.newCommand.Value="SET LED ON 2\r"
    this.manualmodeService.sendCommand(this.newCommand).subscribe(data=>{
    })
  }
  setLed3(){
    const ele = document.getElementById('led3') as HTMLInputElement
    if(ele.checked){
      this.newCommand.Value="SET LED ON 3\r"
    }
    else{
      this.newCommand.Value="SET LED OFF 3\r"
    }
    this.newCommand.Command="Set"
    //this.newCommand.Value="SET LED ON 3\r"
    this.manualmodeService.sendCommand(this.newCommand).subscribe(data=>{
    })
  }

  setPWMDutyCycle(){
    this.submitted = true;
    if (this.pwmFormGroup.invalid) return;

    this.manualmodeService.sendCommand(new Command("SET", "SET DTY "+this.pwmFormGroup.value.pwmCycleControl+"\r")).subscribe(d =>{})
  }

  getTemperature(){
    this.manualmodeService.sendCommand(new Command("GET", "GET TMP\r")).subscribe(d =>{})
  }

  getResistance(){
    this.manualmodeService.sendCommand(new Command("GET", "GET RES\r")).subscribe(d =>{})
  }

  getEPROM(){
    this.manualmodeService.sendCommand(new Command("GET", "GET EPR\r")).subscribe(d =>{})
  }
}
