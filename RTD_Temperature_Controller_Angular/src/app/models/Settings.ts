export enum Colors {
    'red'=0,
    'green'=1,
    'blue'=2
  }
export class Settings{
    threshold:number
    dataAcquisitionRate:number
    temperature_4mA:number
    temperature_20mA:number
    color_0_15:string
    color_16_30:string
    color_31_45:string

    constructor(threshold:number, dataAcquisitionRate:number, temperature_4mA:number, temperature_20mA:number, color_0_15:string, color_16_30:string, color_31_45:string){
        this.threshold = threshold
        this.dataAcquisitionRate = dataAcquisitionRate
        this.temperature_4mA = temperature_4mA
        this.temperature_20mA = temperature_20mA
        this.color_0_15 = color_0_15
        this.color_16_30 = color_16_30
        this.color_31_45 = color_31_45
    }


}