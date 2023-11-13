import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-manualmode',
  templateUrl: './manualmode.component.html',
  styleUrls: ['./manualmode.component.scss']
})
export class ManualmodeComponent {

  pwmFormGroup: FormGroup;
  submitted: boolean = false

  constructor(fb: FormBuilder){
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
}
