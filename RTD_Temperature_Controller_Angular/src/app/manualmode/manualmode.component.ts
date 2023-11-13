import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ManualmodeService } from '../services/manualmode.service';
import { Command } from '../models/Command';

@Component({
  selector: 'app-manualmode',
  templateUrl: './manualmode.component.html',
  styleUrls: ['./manualmode.component.scss']
})
export class ManualmodeComponent {

  pwmFormGroup: FormGroup;
  submitted: boolean = false
  newCommand:Command = new Command('','')

  constructor(fb: FormBuilder,private manualmodeService:ManualmodeService){
    this.pwmFormGroup = fb.group({
      pwmCycleControl: ['', [Validators.required, Validators.pattern("^0*(?:[0-9][0-9]?|100)$")]]
    })
  }

  get getPWMFormControls(){
    return this.pwmFormGroup.controls;
  }
  

  setPWMDutyCycle(){
    this.submitted = true;
    if (this.pwmFormGroup.invalid) return;

    console.log("submitted: "+this.pwmFormGroup.value.pwmCycleControl);
  }

  setLed1(){
    const ele = document.getElementById('led1') as HTMLInputElement
    console.log(ele.checked)
    if(ele.checked){
      this.newCommand.Value="SET LED ON 1\r"
    }
    else{
      console.log("gdfsg")
      this.newCommand.Value="SET LED OFF 1\r"
    }
    this.newCommand.Command="Set"
    //this.newCommand.Value="SET LED ON 1\r"
    this.manualmodeService.sendCommand(this.newCommand).subscribe(data=>{
      console.log(data)
    })
  }
  setLed2(){
    const ele = document.getElementById('led2') as HTMLInputElement
    console.log(ele.checked)
    if(ele.checked){
      this.newCommand.Value="SET LED ON 2\r"
    }
    else{
      console.log("gdfsg")
      this.newCommand.Value="SET LED OFF 2\r"
    }
    this.newCommand.Command="Set"
    //this.newCommand.Value="SET LED ON 2\r"
    this.manualmodeService.sendCommand(this.newCommand).subscribe(data=>{
      console.log(data)
    })
  }
  setLed3(){
    const ele = document.getElementById('led3') as HTMLInputElement
    console.log(ele.checked)
    if(ele.checked){
      this.newCommand.Value="SET LED ON 3\r"
    }
    else{
      console.log("gdfsg")
      this.newCommand.Value="SET LED OFF 3\r"
    }
    this.newCommand.Command="Set"
    //this.newCommand.Value="SET LED ON 3\r"
    this.manualmodeService.sendCommand(this.newCommand).subscribe(data=>{
      console.log(data)
    })
  }


}
