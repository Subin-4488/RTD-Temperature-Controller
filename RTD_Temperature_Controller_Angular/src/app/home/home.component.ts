import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { SettingsService } from '../services/settings.service';
import { HubService } from '../services/hub.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnDestroy {

  color_0_15: string = "green";
  color_16_30: string = "blue";
  color_31_45: string = "red";
  current_selection = "green";

  dataPoints:any[] = [];
  timeout:any = null;
  chart: any;
  
  danger: boolean = false;

  constructor(private http : HttpClient,private hubService:HubService ) {}

  chartOptions = {
    theme: "light2",
    title: {
      text: "RTD Sensed Data"
    },
    axisX: {
      type: Date,
      title: "Time (seconds)",
       valueFormatString: "HH:mm:ss",
       interval: 1,  //data acquisition rate
      intervalType: "second",
     
    },
    axisY: {
      title: "Temperature (°C)"
    },
    data: [{
      type: "line",
      lineThickness: 2,
      dataPoints: this.dataPoints,
      markerColor: "red",
      lineColor:this.current_selection,
    }]
  }

  
 
  getChartInstance(chart: object) {
    this.chart = chart;
    this.updateData();
  }
  
  ngOnDestroy() {
    clearTimeout(this.timeout);
  }
 
  updateData = () => { 
    
    let temperatureData: number[] = [];
    this.hubService.temperatureFromSensor.forEach(d => {
      temperatureData.push(d)
      console.log(`Received temperature update: ${d}`)
      this.addData(temperatureData)
      this.hubService.temperatureFromSensor = []
    })  
  }
  
  addData = (data:any) => {

    console.log("Data to plot in AddData(): "+data)
    console.log("LENGTH: "+this.dataPoints.length)


    if(data.length > 1) {
      data.forEach( (val:any) => {
        console.log("recur: "+val)
        this.dataPoints.push({x: new Date(), y: parseInt(val)});

      })
    } else {
      if (this.dataPoints.length>4){
        while (this.dataPoints.length!=4)
          this.dataPoints.shift();
      }
        
      this.dataPoints.push({x: new Date(), y: parseInt(data), lineColor: this.getColor(parseInt(data))});
      this.current_selection = this.getColor(parseInt(data))

    }
    this.chart.render();
    this.timeout = setTimeout(this.updateData, 1000);  //data acquisition rate
  }

  getColor(temperature: number): string {
    if (temperature >= 0 && temperature <= 15) {
      this.danger = false;
      return this.color_0_15;
    } else if (temperature >= 16 && temperature <= 30) {
      this.danger = false;
      return this.color_16_30;
    } else if (temperature >= 31 && temperature <= 45) {
      this.danger = true;
      return this.color_31_45;
    } else {
      // Default color for values outside the specified ranges
      return "black";
    }
  }
}                               