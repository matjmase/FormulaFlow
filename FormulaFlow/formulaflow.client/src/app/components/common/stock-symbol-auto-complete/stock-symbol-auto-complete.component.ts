import {
  Component,
  EventEmitter,
  forwardRef,
  OnInit,
  Output,
} from '@angular/core';
import {
  NG_VALUE_ACCESSOR,
  ControlValueAccessor,
  FormControl,
} from '@angular/forms';
import { Observable, map } from 'rxjs';
import { StockSymbolDto } from '../../../models/stock-symbol-dto.model';
import { StockSymbolApiService } from '../../../services/api/stock-symbol-api.service';
import { AutoCompleteComplexObject } from '../../../validators/auto-complete-complext-object';

@Component({
  selector: 'app-stock-symbol-auto-complete',
  standalone: false,
  templateUrl: './stock-symbol-auto-complete.component.html',
  styleUrl: './stock-symbol-auto-complete.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => StockSymbolAutoCompleteComponent),
      multi: true,
    },
  ],
})
export class StockSymbolAutoCompleteComponent
  implements OnInit, ControlValueAccessor
{
  @Output() valueChanged = new EventEmitter<StockSymbolDto | null>();

  @Output() symboldIdChanged = new EventEmitter<string | null>();
  private lastSymbolId: string | null = null;

  private _options: StockSymbolDto[] = [];

  public get options(): StockSymbolDto[] {
    return this._options;
  }
  public set options(value: StockSymbolDto[]) {
    this._options = value;
  }

  public myControl = new FormControl<StockSymbolDto | string | null>(
    null,
    AutoCompleteComplexObject,
  );

  public filteredOptions: Observable<StockSymbolDto[]> | undefined;

  private onChangeFunc: ((value: StockSymbolDto | null) => void) | undefined;

  constructor(private symbolService: StockSymbolApiService) {}

  ngOnInit(): void {
    this.filteredOptions = this.myControl.valueChanges.pipe(
      map((value) => this.filter(value || '')),
    );

    this.getStockSymbols();
  }

  private getStockSymbols() {
    this.symbolService.getPaged(1, 100).subscribe((values) => {
      this.options = values.record;
    });
  }

  writeValue(obj: StockSymbolDto | null): void {
    this.myControl.setValue(obj);
    this.lastSymbolId = obj?.id ?? null;
  }
  registerOnChange(fn: (value: StockSymbolDto | null) => void): void {
    this.onChangeFunc = fn;
  }
  registerOnTouched(fn: () => void): void {}
  setDisabledState?(isDisabled: boolean): void {}

  public displayForStockSymbol(value: StockSymbolDto): string {
    return value?.symbol ?? '';
  }

  public filter(value: string | null | StockSymbolDto): StockSymbolDto[] {
    let filterValue = '';

    if (value === null) {
      this.valueChanged.emit(null);
      if (this.onChangeFunc) {
        this.onChangeFunc(value);
      }
      if (this.lastSymbolId !== null) {
        this.lastSymbolId = null;
        this.symboldIdChanged.emit(null);
      }
    } else if (typeof value === 'string') {
      filterValue = value.toLowerCase();
      this.valueChanged.emit(null);
      if (this.onChangeFunc) {
        this.onChangeFunc(null);
      }
      if (this.lastSymbolId !== null) {
        this.lastSymbolId = null;
        this.symboldIdChanged.emit(null);
      }
    } else {
      filterValue = (<StockSymbolDto>value).symbol?.toLowerCase() ?? '';

      this.valueChanged.emit(value);
      if (this.onChangeFunc) {
        this.onChangeFunc(value);
      }
      if (this.lastSymbolId !== value?.id) {
        this.lastSymbolId = value?.id ?? null;
        this.symboldIdChanged.emit(value?.id ?? null);
      }
    }

    return this.options.filter((option) =>
      option?.symbol?.toLowerCase().includes(filterValue),
    );
  }
}
