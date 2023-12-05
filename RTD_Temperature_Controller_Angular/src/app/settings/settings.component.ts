import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SettingsService } from '../services/settings.service';
import { Colors, Settings } from '../models/Settings';
import {
  sameColor,
  temperatureValidation,
} from '../customValidators/customValidators';
import { MatSnackBar } from '@angular/material/snack-bar';

/**
 * This component gives the user ability to change the configuration of the device like colors for different temperature range, temperature value for 4mA and 20mA current, and change thethreshold value.
 */

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
})
export class SettingsComponent {
  settingsForm: FormGroup;
  submitted: boolean = false;

  constructor(fb: FormBuilder, private settingsService: SettingsService, private _snackBar: MatSnackBar) {
    this.settingsForm = fb.group(
      {
        threshold: [, [Validators.required, Validators.pattern('[0-9]*')]],
        dar: [, Validators.required],
        current4: [, [Validators.required, Validators.pattern('[0-9]*')]],
        current20: [, [Validators.required, Validators.pattern('[0-9]*')]],
        c1: [, Validators.required],
        c2: [, Validators.required],
        c3: [, Validators.required],
      },
      {
        validators: [
          sameColor('c1', 'c2', 'c3'),
          temperatureValidation('current4', 'current20'),
        ],
      }
    );
    this.resetSettings();
  }

  /**
   * This is a getter used for settingsForm controls
   */

  get getsettingsFormControls() {
    return this.settingsForm.controls;
  }
  
  /**
   * This functions is triggered when user submits the settings form. 
   * This in turn call the updateSettings function from the settingsService to post the changes to the backend
   * @param value Holds all the values for the form controls
   */

  onSubmit(value: string) {
    console.log(value);
    this.submitted = true;
    let newSettings = new Settings(
      this.settingsForm.value.threshold,
      this.settingsForm.value.dar,
      this.settingsForm.value.current4,
      this.settingsForm.value.current20,
      this.settingsForm.value.c1,
      this.settingsForm.value.c2,
      this.settingsForm.value.c3
    );
    this.settingsService.updateSettings(newSettings).subscribe((data) => {
      this._snackBar.open(data.message,'Got it', {
        duration: 4000
      });
    });
  }

  /**
   * This functions is triggered when user presses the reset button in the UI.
   * 
   * This in turn call the resetSettings function from the settingsService to get the last stored settings values from the JSON file to update the form in the UI.
   */

  resetSettings() {
    console.log(
      this.getsettingsFormControls['current20'].hasError('temperatureGreater')
    );
    let newSettings = new Settings(0, 0, 0, 0, '', '', '');
    this.settingsService.resetSettings().subscribe((data) => {
      newSettings = data;
      newSettings.color_0_15 = Colors[Number(data.color_0_15)];
      newSettings.color_16_30 = Colors[Number(data.color_16_30)];
      newSettings.color_31_45 = Colors[Number(data.color_31_45)];

      this.settingsForm.patchValue({
        threshold: newSettings.threshold,
        dar: newSettings.dataAcquisitionRate,
        current4: newSettings.temperature_4mA,
        current20: newSettings.temperature_20mA,
        c1: newSettings.color_0_15,
        c2: newSettings.color_16_30,
        c3: newSettings.color_31_45,
      });
    });
  }
}
