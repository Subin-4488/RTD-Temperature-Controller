export class Settings{
    Threshold:number
    DataAcquisitionRate:number
    Temperature_4mA:number
    Temperature_20mA:number
    Color_0_15:string
    Color_16_30:string
    Color_31_45:string

    constructor(threshold:number, dataAcquisitionRate:number, temperature_4mA:number, temperature_20mA:number, color_0_15:string, color_16_30:string, color_31_45:string){
        this.Threshold = threshold
        this.DataAcquisitionRate = dataAcquisitionRate
        this.Temperature_4mA = temperature_4mA
        this.Temperature_20mA = temperature_20mA
        this.Color_0_15 = color_0_15
        this.Color_16_30 = color_16_30
        this.Color_31_45 = color_31_45
    }


}