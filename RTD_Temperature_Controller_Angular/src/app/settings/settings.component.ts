import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})

export class SettingsComponent {
  
  settingsForm:FormGroup

  constructor(fb:FormBuilder){
    this.settingsForm=fb.group({
      threshold:[],
      dar:[],
      current4:[],
      current20:[],
      c1:[],
      c2:[],
      c3:[]
    })
  }
  onSubmit(value:string){
    console.log(value)
  }

}
