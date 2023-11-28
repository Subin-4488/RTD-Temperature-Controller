
//////////////////////////////////////////////////////////////////////////
/// <summary>
/// Settings representing the configuration of the system.
/// </summary>
/// <remarks>
/// 1. color_x_y : Represents the color value for temperature range x to y, where x and y are numerals.
/// 2. temperature_TmA: Represents the temperature value for TmA current, where T is either 4 or 20
/// </remarks>
//////////////////////////////////////////////////////////////////////////

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

export enum Colors {
    'red'=82,
    'green'=71,
    'blue'=66
}