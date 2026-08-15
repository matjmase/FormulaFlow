import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import {
  MatRippleModule,
  provideNativeDateAdapter,
} from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { StockManagerComponent } from './components/stock-manager/stock-manager.component';
import { StockManagerStockDataComponent } from './components/stock-manager-stock-data/stock-manager-stock-data.component';
import { StockManagerStockDataReadComponent } from './components/stock-manager-stock-data-read/stock-manager-stock-data-read.component';
import { StockManagerStockDataUploadComponent } from './components/stock-manager-stock-data-upload/stock-manager-stock-data-upload.component';
import { StockManagerStockSymbolComponent } from './components/stock-manager-stock-symbol/stock-manager-stock-symbol.component';
import { StockManagerStockSymbolCreateComponent } from './components/stock-manager-stock-symbol-create/stock-manager-stock-symbol-create.component';
import { StockManagerStockSymbolDeleteComponent } from './components/stock-manager-stock-symbol-delete/stock-manager-stock-symbol-delete.component';
import { StockManagerStockSymbolReadComponent } from './components/stock-manager-stock-symbol-read/stock-manager-stock-symbol-read.component';
import { StrategyManagerComponent } from './components/strategy-manager/strategy-manager.component';
import { StrategyManagerCanvasComponent } from './components/strategy-manager-canvas/strategy-manager-canvas.component';
import { StrategyManagerCanvasArrowComponent } from './components/strategy-manager-canvas-arrow/strategy-manager-canvas-arrow.component';
import { StrategyManagerCanvasSideToolbarComponent } from './components/strategy-manager-canvas-side-toolbar/strategy-manager-canvas-side-toolbar.component';
import { StrategyManagerCanvasTopToolbarComponent } from './components/strategy-manager-canvas-top-toolbar/strategy-manager-canvas-top-toolbar.component';
import { StrategyManagerCardInputComponent } from './components/strategy-manager-card-input/strategy-manager-card-input.component';
import { StrategyManagerCardIoIconComponent } from './components/strategy-manager-card-io-icon/strategy-manager-card-io-icon.component';
import { StrategyManagerCardIoLabelComponent } from './components/strategy-manager-card-io-label/strategy-manager-card-io-label.component';
import { StrategyManagerCardOutputComponent } from './components/strategy-manager-card-output/strategy-manager-card-output.component';
import { StrategyManagerCardComponent } from './components/strategy-manager-card/strategy-manager-card.component';
import { StrategyManagerParameterComponent } from './components/strategy-manager-parameter/strategy-manager-parameter.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthenticatedIfDirective } from './directives/authenticated-if.directive';
import { AuthorizedIfDirective } from './directives/authorized-if.directive';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    StockManagerComponent,
    StockManagerStockDataComponent,
    StockManagerStockDataReadComponent,
    StockManagerStockDataUploadComponent,
    StockManagerStockSymbolComponent,
    StockManagerStockSymbolCreateComponent,
    StockManagerStockSymbolDeleteComponent,
    StockManagerStockSymbolReadComponent,
    StrategyManagerComponent,
    StrategyManagerCanvasComponent,
    StrategyManagerCanvasArrowComponent,
    StrategyManagerCanvasSideToolbarComponent,
    StrategyManagerCanvasTopToolbarComponent,
    StrategyManagerCardComponent,
    StrategyManagerCardInputComponent,
    StrategyManagerCardOutputComponent,
    StrategyManagerCardIoIconComponent,
    StrategyManagerCardIoLabelComponent,
    StrategyManagerParameterComponent,
    AuthenticatedIfDirective,
    AuthorizedIfDirective,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MatSnackBarModule,
    MatIconModule,
    MatSidenavModule,
    MatToolbarModule,
    MatRippleModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatTabsModule,
    MatExpansionModule,
    MatPaginatorModule,
    MatAutocompleteModule,
    MatSelectModule,
    MatCheckboxModule,
    MatListModule,
    MatDividerModule,
    MatDatepickerModule,
    MatTooltipModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
