import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy } from '@angular/core';
import { Colors, Settings } from '../models/Settings';
import { SettingsService } from '../services/settings.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnDestroy {

  settings: Settings = new Settings(0,0,0,0,'','','');
  current_selection = "green";

  constructor(private http : HttpClient, private settings_service: SettingsService) {
    this.settings_service.resetSettings().subscribe(data=>{
      console.log(data)
      
      this.settings = data
      this.settings.color_0_15 = Colors[Number(data.color_0_15)]
      this.settings.color_16_30 = Colors[Number(data.color_16_30)]
      this.settings.color_31_45 = Colors[Number(data.color_31_45)]
      
    })  
  }
 
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
      interval: 0,  //data acquisition rate
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
    this.http.get("https://canvasjs.com/services/data/datapoints.php?xstart="
    +this.xValue
    +"&ystart="
    +this.yValue
    +"&length="
    +this.newDataCount
    +"type=json", { responseType: 'json' })
    .subscribe(this.addData);
    
  }
 
  addData = (data:any) => {
    
    let time = new Date()
    console.log("time: "+time)

    if(this.newDataCount != 1) {
      data.forEach( (val:any[]) => {
        console.log("1, VAL:"+val+" xVal:"+ this.xValue+" yVal:"+this.yValue)
        this.dataPoints.push({x: time, y: parseInt(val[1])});
        this.yValue = parseInt(val[1]);  
        this.xValue++;
      })
    } else {
      console.log("1, VAL:"+data[0]+" xVal:"+ this.xValue+" yVal:"+this.yValue)
      this.dataPoints.shift();
      this.dataPoints.push({x: time, y: parseInt(data[0][1]), lineColor: this.getColor(parseInt(data[0][1]))});
      this.current_selection = this.getColor(parseInt(data[0][1 ]))
      this.yValue = parseInt(data[0][1]);  
      this.xValue++;
      
    }
    this.newDataCount = 1;
    this.chart.render();
    this.timeout = setTimeout(this.updateData, this.settings.dataAcquisitionRate*100);  //data acquisition rate
  }

  getColor(temperature: number): string {
    if (temperature >= 0 && temperature <= 15) {
      this.danger = false;
      return this.settings.color_0_15;
    } else if (temperature >= 16 && temperature <= 30) {
      this.danger = false;
      return this.settings.color_16_30;
    } else if (temperature >= 31 && temperature <= 45) {
      this.danger = true;
      return this.settings.color_31_45;
    } else {
      // Default color for values outside the specified ranges
      return "black";
    }
  }
}                              