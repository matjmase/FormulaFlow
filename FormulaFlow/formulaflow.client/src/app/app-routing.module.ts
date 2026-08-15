import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { StockManagerComponent } from './components/stock-manager/stock-manager.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'stock-manager', component: StockManagerComponent },
  // { path: "strategy-manager", component: StrategyManagerComponent },
  // {
  //   path: "strategy-manager/canvas",
  //   component: StrategyManagerCanvasComponent,
  // },
  // {
  //   path: "strategy-manager/canvas/:canvasId",
  //   component: StrategyManagerCanvasComponent,
  // },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
