import {
  FormGroup,
} from '@angular/forms';

export function sameColor(color1: string, color2: string, color3: string) {
  return (form: FormGroup) => {
    const c1 = form.controls[color1];
    const c2 = form.controls[color2];
    const c3 = form.controls[color3];
    //const passwdc=form.controls[passwdcname]
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

export function temperatureValidation(current4:string,current20:string){
    return (form:FormGroup) =>{
        const current4Control = form.controls[current4]
        const current20Control = form.controls[current20]
        let flag = 0
        console.log(current20Control.value)
        if (isNaN(parseInt(current20Control.value))){
            flag = 1;
        }
        if(parseInt(current20Control.value) <= parseInt(current4Control.value)){
            current20Control.setErrors({temperatureGreater:true})
            flag = 1
        }
        else if(flag == 0){
            current20Control.setErrors(null)
        }
        
    }
}
