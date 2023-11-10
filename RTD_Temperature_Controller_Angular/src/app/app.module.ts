import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BannerComponent } from './banner/banner.component';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ConnectionComponent } from './connection/connection.component';
import { SettingsComponent } from './settings/settings.component';
import { ManualmodeComponent } from './manualmode/manualmode.component';

@NgModule({
  declarations: [
    AppComponent,
    ConnectionComponent,
    BannerComponent,
    HomeComponent,
    DashboardComponent,
    SettingsComponent,
    ManualmodeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
