import { PACKAGE_ROOT_URL } from "@angular/core"

export class Connection{
    BitsPerSecond:number
    DataBits:number
    Parity:string
    StopBits:number

    constructor(bitsPerSecond:number,dataBits:number,parity:string,stopBits:number){
        this.BitsPerSecond = bitsPerSecond
        this.DataBits = dataBits
        this.Parity = parity
        this.StopBits = stopBits
    }
}