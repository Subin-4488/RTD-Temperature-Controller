import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SettingsService } from '../services/settings.service';
import { Colors, Settings } from '../models/Settings';

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
    let newSettings = new Settings(this.settingsForm.value.threshold,this.settingsForm.value.dar,
      this.settingsForm.value.current4,this.settingsForm.value.current20,
      this.settingsForm.value.c1,this.settingsForm.value.c2,
      this.settingsForm.value.c3)
    
      this.settingsService.updateSettings(newSettings).subscribe(data=>{

      })
  }

  resetSettings(){
    let newSettings =new Settings(0,0,0,0,'','','')
    this.settingsService.resetSettings().subscribe(data=>{
      
      //data.color_0_15 = Colors[data.color_0_15 as keyof typeof Colors];
      newSettings = data
      newSettings.color_0_15 = Colors[Number(data.color_0_15)]
      newSettings.color_16_30 = Colors[Number(data.color_16_30)]
      newSettings.color_31_45 = Colors[Number(data.color_31_45)]
      this.settingsForm.patchValue({
        threshold:newSettings.threshold,
        dar:newSettings.dataAcquisitionRate,
        current4:newSettings.temperature_4mA,
        current20:newSettings.temperature_20mA,
        c1:newSettings.color_0_15,
        c2:newSettings.color_16_30,
        c3:newSettings.color_31_45
      })
    })
    
  }

}
