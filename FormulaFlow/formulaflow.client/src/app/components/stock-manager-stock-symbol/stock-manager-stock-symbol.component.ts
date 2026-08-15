import { ChangeDetectionStrategy, Component, inject } from '@angular/core';

@Component({
  selector: 'app-stock-manager-stock-symbol',
  standalone: false,
  templateUrl: './stock-manager-stock-symbol.component.html',
  styleUrl: './stock-manager-stock-symbol.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StockManagerStockSymbolComponent {}
