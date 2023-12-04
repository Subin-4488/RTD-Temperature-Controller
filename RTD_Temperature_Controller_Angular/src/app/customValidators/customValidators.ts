import {
  FormGroup,
} from '@angular/forms';

/**
 * Custom validator function to check if three form control values representing colors are the same.
 * 
 * If any two colors are the same, it sets the 'sameColor' error on those controls.
 * @param color1 - The key of the first color form control.
 * @param color2 - The key of the second color form control.
 * @param color3 - The key of the third color form control.
 * @returns A validation function to be used with Angular reactive forms.
 */

export function sameColor(color1: string, color2: string, color3: string) {
  return (form: FormGroup) => {
    const c1 = form.controls[color1];
    const c2 = form.controls[color2];
    const c3 = form.controls[color3];

    let flag = 0;
    if (c1.value ==null || c2.value == null || c3.value==null){
        flag = 1;
    }
    if (c1.value == c2.value) {
      c1.setErrors({ sameColor: true });
      c2.setErrors({ sameColor: true });
      flag = 1;
    }
    if (c1.value == c3.value) {
      c1.setErrors({ sameColor: true });
      c3.setErrors({ sameColor: true });
      flag = 1;
    }
    if (c2.value == c3.value) {
      c2.setErrors({ sameColor: true });
      c3.setErrors({ sameColor: true });
      flag = 1;
    }
    if (flag == 0) {
      c1.setErrors(null);
      c2.setErrors(null);
      c3.setErrors(null);
    }
  };
}

/**
 * Custom validator function to check if the value of a temperature at 20°C is greater than the value at 4°C.
 * 
 * If not, it sets the 'temperatureGreater' error on the control representing the temperature at 20°C.
 * @param current4 - The key of the form control representing the temperature at 4°C.
 * @param current20 - The key of the form control representing the temperature at 20°C.
 * @returns A validation function to be used with Angular reactive forms.
 */

export function temperatureValidation(current4:string,current20:string){
    return (form:FormGroup) =>{
        const current4Control = form.controls[current4]
        const current20Control = form.controls[current20]
        let flag = 0
        if (isNaN(Number(current20Control.value)) || isNaN(parseInt(current20Control.value))){
            flag = 1;
        }
        else if(parseInt(current20Control.value) <= parseInt(current4Control.value)){
            current20Control.setErrors({temperatureGreater:true})
            flag = 1
        }
        if(flag == 0){
            current20Control.setErrors(null)
        }
    }
}
