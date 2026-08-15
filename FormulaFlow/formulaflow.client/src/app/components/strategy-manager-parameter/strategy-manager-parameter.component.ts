import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { catchError, of } from 'rxjs';
import { NetworkParameterType } from '../../models/network-parameter-type.model';
import { StockParameterDto } from '../../models/stock-parameter-dto.model';
import { StockSymbolDto } from '../../models/stock-symbol-dto.model';
import { StockSymbolApiService } from '../../services/api/stock-symbol-api.service';
import { AggregateMethodIntermediateParameterType } from '../../models/aggregate-method-intermediate-parameter-type';

@Component({
  selector: 'app-strategy-manager-parameter',
  standalone: false,
  templateUrl: './strategy-manager-parameter.component.html',
  styleUrl: './strategy-manager-parameter.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerParameterComponent implements OnInit {
  @Input() public model!: StockParameterDto;
  @Output() public dirtyEvent = new EventEmitter<void>();

  public selectedSymbol: StockSymbolDto | null = null;

  public stockSourceType = NetworkParameterType.StockSource;
  public numberType = NetworkParameterType.Number;
  public transitionalType = NetworkParameterType.Transitional;
  public booleanType = NetworkParameterType.Boolean;
  public numericInput = NetworkParameterType.NumericInput;
  public aggregateMethodType = NetworkParameterType.AggregateMethod;
  public bufferInput = NetworkParameterType.BufferInput;

  public aggregateAverage = AggregateMethodIntermediateParameterType.Average;
  public aggregateMultiplicative =
    AggregateMethodIntermediateParameterType.Multiplicative;
  public aggregateSummation =
    AggregateMethodIntermediateParameterType.Summation;

  constructor(private symbolService: StockSymbolApiService) {}

  ngOnInit(): void {
    if (this.model.type === this.stockSourceType && this.model.value) {
      this.symbolService
        .getById(this.model.value)
        .pipe(catchError((error) => of(null)))
        .subscribe({
          next: (value) => {
            this.selectedSymbol = value;
          },
        });
    }
  }

  public stockSymbolConverter(value: StockSymbolDto | null): void {
    this.model.value = value?.id ?? '';
  }

  public stockSymbolIdConverter(value: string | null): void {
    this.model.value = value ?? '';
  }

  public setDirty() {
    this.dirtyEvent.emit();
  }
}
