import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SettingsService } from '../services/settings.service';
import { Settings } from '../models/Settings';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})

export class SettingsComponent {
  
  settingsForm:FormGroup

  constructor(fb:FormBuilder,private settingsService:SettingsService){
    this.settingsForm=fb.group({
      threshold:[,Validators.required],
      dar:[,Validators.required],
      current4:[,Validators.required],
      current20:[,Validators.required],
      c1:[,Validators.required],
      c2:[,Validators.required],
      c3:[,Validators.required]
    })
  }
  onSubmit(value:string){
    //console.log(value)
    let newSettings = new Settings(this.settingsForm.value.threshold,this.settingsForm.value.dar,
      this.settingsForm.value.current4,this.settingsForm.value.current20,
      this.settingsForm.value.c1,this.settingsForm.value.c2,
      this.settingsForm.value.c3)
    
      this.settingsService.updateSettings(newSettings).subscribe(data=>{

      })
    console.log(newSettings)
  }

  resetSettings(){
    
  }

}
