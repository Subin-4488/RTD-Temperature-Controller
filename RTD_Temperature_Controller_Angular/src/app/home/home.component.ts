
import { Component, OnDestroy } from '@angular/core';
import { Command } from '../models/Command';
import { Colors, Settings } from '../models/Settings';
import { HomeService } from '../services/home.service';
import { HubService } from '../services/hub.service';
import { SettingsService } from '../services/settings.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnDestroy {

  settings: Settings = new Settings(0,0,0,0,'','','');
  current_selection = "green";
  dar=0


  constructor(private hubService:HubService
    ,private settings_service: SettingsService,
    private home_service: HomeService) {

      if (this.settings.threshold == 0)

      this.settings_service.resetSettings().subscribe(data=>{
        
        this.settings = data
        this.settings.color_0_15 = Colors[Number(data.color_0_15)]
        this.settings.color_16_30 = Colors[Number(data.color_16_30)]
        this.settings.color_31_45 = Colors[Number(data.color_31_45)]
      })  
      
  }

 
  dataPoints:any[] = [];
  timeout:any = null;
  chart: any;
  
  danger: boolean = false;
  sensor_status: boolean = false;


  chartOptions = {
    zoomEnabled: true,
    backgroundColor: "#edf5fc",
    theme: "light2",
    title: {
      text: "RTD Sensed Data",
    },
    toolTip:{             
      content: "{x}: {y}"
    },
    axisX: {
      type: Date,
      title: "Time (seconds)",
      valueFormatString: "HH:mm:ss",
      // interval: 1,  //data acquisition rate
      // intervalType: "second",
     

    },
    axisY: {
      title: "Temperature (°C)"
    },
    data: [{
      type: "line",
      lineThickness: 2,
      dataPoints: this.dataPoints,
      markerColor: "black",
      //lineColor:this.current_selection,
    }]
  }

  
 
  getChartInstance(chart: object) {
    this.chart = chart;
    this.settings_service.resetSettings().subscribe(data=>{
      this.settings = data
      this.settings.color_0_15 = Colors[Number(data.color_0_15)]
      this.settings.color_16_30 = Colors[Number(data.color_16_30)]
      this.settings.color_31_45 = Colors[Number(data.color_31_45)]
    })     
  }
  
  ngOnDestroy() {
    clearTimeout(this.timeout);
    this.hubService.hubConnection.off('UpdateTemperature')
  }
 
  // updateData = () => { 
  //   let temperatureData: number[] = [];
  //   this.hubService.temperatureFromSensor.forEach(d => {
  //     temperatureData.push(d)
  //   })  
  //   this.addData(temperatureData)
  //     this.hubService.temperatureFromSensor = []
  // }
  
  // addData = (data:any[]) => {

  //   if(data.length > 1) {
  //     data.forEach( (val:any) => {
        
  //       this.dataPoints.push({x: new Date(val.time), y: parseInt(val.temperature),  lineColor: this.getColor(parseInt(val.temperature))});
        
  //       var formattedTime = this.datePipe.transform(val.time, 'HH:mm:ss')
  //       //  console.log("Recur: "+formattedTime+":"+val.temperature+":"+this.getColor(parseInt(data[0].temperature)))
        
  //     })
  //     data = []

  //   } 
  //   else {
  //     var formattedTime = this.datePipe.transform(data[0].time, 'HH:mm:ss')
  //     // console.log("normal: "+formattedTime+":"+data[0].temperature)
        
  //     this.dataPoints.push({x: new Date(data[0].time), y: parseInt(data[0].temperature), lineColor: this.getColor(parseInt(data[0].temperature))});
  //     this.current_selection = this.getColor(parseInt(data[0].temperature))
      
  //   }
  //   this.chart.render();
  //   if (this.dataPoints.length>4){
  //     while (this.dataPoints.length>=4)
  //       this.dataPoints.shift();
  //   }
  //   this.timeout = setTimeout(this.updateData, this.settings.dataAcquisitionRate*1000);  //data acquisition rate

  // }


  getColor(temperature: number): string {
    
    if (temperature > this.settings.threshold){
      this.danger = true;
    }
    else{
      this.danger = false;
    }

    if (temperature >= 0 && temperature <= 15) {
      return this.settings.color_0_15; 
    } else if (temperature >= 16 && temperature <= 30) {
      return this.settings.color_16_30;
    } else if (temperature >= 31 && temperature <= 45) {
      return this.settings.color_31_45;
    } else {
      // Default color for values outside the specified ranges
      return "black";
    }
  }

  graphInitializer(){

    this.sensor_status = !this.sensor_status
    if(this.sensor_status){
      //console.log("hello")
      this.home_service.sendCommand(new Command("GET","GET TMPA\r")).subscribe(d=>{
        if(d==true){
          this.hubService.hubConnection.on('UpdateTemperature',(temperatureData) =>{
            console.log(this.getColor(parseInt(temperatureData.temperature)))
            //console.log(this.dataPoints)
            if(this.dar == 0)
              this.dataPoints.push({x: new Date(temperatureData.time), y: parseInt(temperatureData.temperature),markerColor:this.getColor(parseInt(temperatureData.temperature)),  lineColor: this.getColor(parseInt(temperatureData.temperature))});
            this.dar = (this.dar+1)%this.settings.dataAcquisitionRate
            
            //this.dataPoints.push({x: new Date(temperatureData.time), y: parseInt(temperatureData.temperature)});

            if(this.dataPoints.length>20)
              this.dataPoints.shift()
            this.chart.render()
          })
        }
      })
    }
    else{
      this.hubService.hubConnection.off('UpdateTemperature')
    }
  }
}                               