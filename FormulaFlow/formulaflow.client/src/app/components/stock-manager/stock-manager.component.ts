import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-stock-manager',
  standalone: false,
  templateUrl: './stock-manager.component.html',
  styleUrl: './stock-manager.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StockManagerComponent {}
