import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ConnectionComponent } from './connection/connection.component';
import { HomeComponent } from './home/home.component';
import { SettingsComponent } from './settings/settings.component';
import { ManualmodeComponent } from './manualmode/manualmode.component';
import { connectionGuard } from './guards/connection.guard';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
const routes: Routes = [
  {path:"",component:ConnectionComponent},
  {path: "dashboard", component: DashboardComponent, children:[
    {path:"home",component:HomeComponent, outlet:'navRoute' },
    {path:"settings",component:SettingsComponent, outlet:'navRoute' },
    {path:"manualmode",component:ManualmodeComponent, outlet:'navRoute' },
    {path:"**",component:PageNotFoundComponent}
  ],
  canActivate: [connectionGuard]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
