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

  //private hubConnection: signalR.HubConnection;
  color_0_15: string = "green";
  color_16_30: string = "blue";
  color_31_45: string = "red";
  current_selection = "green";

  constructor(private http : HttpClient,private hubService:HubService ) {}

 
  dataPoints:any[] = [];
  timeout:any = null;
  xValue:number = 0;
  yValue:number = 10;
  newDataCount:number = 6;
  chart: any;
  
  danger: boolean = false;
 
  chartOptions = {
    theme: "light2",
    title: {
      text: "RTD Sensed Data"
    },
    axisX: {
      title: "Time (seconds)",
      valueFormatString: "HH:mm:ss",
      interval: 1,
      intervalType: "second",
      type: 'dateTime',
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
    // this.http.get("https://canvasjs.com/services/data/datapoints.php?xstart="
    // +this.xValue
    // +"&ystart="
    // +this.yValue
    // +"&length="
    // +this.newDataCount
    // +"type=json", { responseType: 'json' })
    // .subscribe(this.addData);
    
    let temperatureData: number[] = [];
    this.hubService.temperatureFromSensor.forEach(d => {
      temperatureData.push(d)
      console.log(`Received temperature update: ${d}`)
      this.addData(temperatureData)
    })  

    //call addData(temperatureData)
  }
 
  addData = (data:any) => {
    
    console.log("Data to plot in AddData(): "+data)
    
    let time = new Date()
    //console.log("time: "+time)

    if(this.newDataCount != 1) {
      data.forEach( (val:number) => {
        // console.log("1, VAL:"+val+" xVal:"+ this.xValue+" yVal:"+this.yValue)
        this.dataPoints.push({x: time, y: val});
        this.yValue = val;  
        this.xValue++;
      })
    } else {
      //console.log("1, VAL:"+data[0]+" xVal:"+ this.xValue+" yVal:"+this.yValue)
      this.dataPoints.shift();
      this.dataPoints.push({x: time, y: data[0], lineColor: this.getColor(data[0])});
      this.current_selection = this.getColor(data[0])
      this.yValue = data[0];  
      this.xValue++;
      
    }
    this.newDataCount = 1;
    this.chart.render();
    this.timeout = setTimeout(this.updateData, 3000);  //data acquisition rate
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