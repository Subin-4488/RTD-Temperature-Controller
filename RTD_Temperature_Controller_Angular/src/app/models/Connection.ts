import { PACKAGE_ROOT_URL } from "@angular/core"

export class Connection{
    PortName:string
    BitsPerSecond:number
    DataBits:number
    Parity:string
    StopBits:number

    constructor(portName:string,bitsPerSecond:number,dataBits:number,parity:string,stopBits:number){
        this.PortName = portName
        this.BitsPerSecond = bitsPerSecond
        this.DataBits = dataBits
        this.Parity = parity
        this.StopBits = stopBits
    }
}