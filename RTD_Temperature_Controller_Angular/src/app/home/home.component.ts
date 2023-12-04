import { Component, OnDestroy } from '@angular/core';
import { Command } from '../models/Command';
import { Colors, Settings } from '../models/Settings';
import { HomeService } from '../services/home.service';
import { HubService } from '../services/hub.service';
import { SettingsService } from '../services/settings.service';

/**
 * The component shall render the live plot of Temperature v/s Time and shall display the threshold and system-status indicator data.
 */

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnDestroy {

  Settings: Settings = new Settings(0, 0, 0, 0, '', '', '');
  CurrentSelection = 'green';
  DataAcquisitionRate = 0;
  DataPoints: any[] = [];
  DangerAlarm: boolean = false;
  SensorStatus: boolean = false;
  Chart: any;
  ChartOptions = {
    zoomEnabled: true,
    showInLegend: true,
    backgroundColor: '#edf5fc',
    theme: 'light2',
    title: {
      text: 'RTD Sensed Data',
    },
    toolTip: {
      content: '{x}: {y}',
    },
    axisX: {
      type: Date,
      title: 'Time (seconds)',
      valueFormatString: 'HH:mm:ss',
      // interval: 1,  //data acquisition rate
      // intervalType: "second",
    },
    axisY: {
      title: 'Temperature (°C)',
    },
    data: [
      {
        type: 'line',
        lineThickness: 2,
        dataPoints: this.DataPoints,
        markerColor: 'black',
        //lineColor:this.current_selection,
      },
    ],
  };

  constructor(
    private hubService: HubService,
    private settingsService: SettingsService,
    private homeService: HomeService
  ) {
    // if (this.Settings.threshold == 0)
    //   this.settingsService.resetSettings().subscribe((data) => {
    //     this.Settings = data;
    //     this.Settings.color_0_15 = Colors[Number(data.color_0_15)];
    //     this.Settings.color_16_30 = Colors[Number(data.color_16_30)];
    //     this.Settings.color_31_45 = Colors[Number(data.color_31_45)];
    //   });
  }

  /**
   * A method used to obtain a reference to the chart instance for further interaction.
   * @param chart It is the chart instance passed when the (chartInstance) event occurs on the <canvasjs-chart> component
   */

  getChartInstance(chart: object) {
    this.Chart = chart;
    this.settingsService.resetSettings().subscribe((data) => {
      this.Settings = data;
      this.Settings.color_0_15 = Colors[Number(data.color_0_15)];
      this.Settings.color_16_30 = Colors[Number(data.color_16_30)];
      this.Settings.color_31_45 = Colors[Number(data.color_31_45)];
    });
  }

  /**
   * A callback method that performs custom clean-up, invoked immediately before a directive, pipe, or service instance is destroyed.
   */

  ngOnDestroy() {
    this.hubService.closeAutomatic();
  }

  /**
   * The GetColor method sets the chart color based on temperature ranges and triggers the DangerAlarm indication if the sensed temperature surpasses the threshold
   * @param temperature It is the latest temperature sensed by hardware
   * @returns A color value
   */

  getColor(temperature: number): string {
    if (temperature > this.Settings.threshold) {
      this.DangerAlarm = true;
    } else {
      this.DangerAlarm = false;
    }

    if (temperature >= 0 && temperature <= 15) {
      return this.Settings.color_0_15;
    } else if (temperature > 15 && temperature <= 30) {
      return this.Settings.color_16_30;
    } else {
      return this.Settings.color_31_45;
    }
  }

  /**
   * Initializes the graph by updating sensor status and subscribing to temperature data.
   * 
   * 'GET TMPA' command is sent to the hardware when the user interact with start button following which a listener for the 
   * 
   * 'UpdateTemperature' event on the SignalR hub connection is initialized. Based on the data obtained from the server the points are pushed into a list to inorder get plotted on the graph
   */

  graphInitializer() {
    this.SensorStatus = !this.SensorStatus;
    if (this.SensorStatus) {
      this.homeService
        .sendCommand(new Command('GET', 'GET TMPA\r'))
        .subscribe((d) => {
          if (d == true) {
            this.hubService.hubConnection.on(
              'UpdateTemperature',
              (temperatureData) => {
                if (this.DataAcquisitionRate == 0) {
                  var pointColor = this.getColor(
                    parseFloat(temperatureData.temperature)
                  );
                  this.DataPoints.push({
                    x: new Date(temperatureData.time),
                    y: parseFloat(temperatureData.temperature),
                    markerColor: pointColor,
                    lineColor: pointColor,
                  });
                }
                this.DataAcquisitionRate =
                  (this.DataAcquisitionRate + 1) %
                  this.Settings.dataAcquisitionRate;

                if (this.DataPoints.length > 20) this.DataPoints.shift();
                this.Chart.render();
              }
            );
          }
        });
    } else {
      this.hubService.closeAutomatic();
    }
  }
}
