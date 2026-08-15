import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
} from '@angular/core';
import {
  FormBuilder,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, startWith, map, finalize } from 'rxjs';
import { StockSymbolDto } from '../../models/stock-symbol-dto.model';
import { StockSymbolApiService } from '../../services/api/stock-symbol-api.service';

@Component({
  selector: 'app-stock-manager-stock-symbol-delete',
  standalone: false,
  templateUrl: './stock-manager-stock-symbol-delete.component.html',
  styleUrl: './stock-manager-stock-symbol-delete.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StockManagerStockSymbolDeleteComponent implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly stockSymbolApiService = inject(StockSymbolApiService);
  private readonly snackBar = inject(MatSnackBar);

  public deletedStockSymbolId: string | null = null;
  public deletedStockSymbol: StockSymbolDto | null = null;
  public filteredStockSymbols$: Observable<StockSymbolDto[]> | null = null;
  public isDeleting = false;
  public isLoadingStockSymbols = false;
  public stockSymbols: StockSymbolDto[] = [];

  public deleteForm = this.formBuilder.group({
    stockSymbol: [
      null as StockSymbolDto | string | null,
      [Validators.required, this.stockSymbolSelectionValidator],
    ],
  });

  public ngOnInit(): void {
    this.filteredStockSymbols$ =
      this.deleteForm.controls.stockSymbol.valueChanges.pipe(
        startWith(this.deleteForm.controls.stockSymbol.value),
        map((value) => this.filterStockSymbols(value)),
      );

    this.loadStockSymbols();
  }

  public displayStockSymbol(
    stockSymbol: StockSymbolDto | string | null,
  ): string {
    return typeof stockSymbol === 'string'
      ? stockSymbol
      : (stockSymbol?.symbol ?? '');
  }

  public deleteStockSymbol(): void {
    if (this.deleteForm.invalid) {
      this.deleteForm.markAllAsTouched();
      return;
    }

    const stockSymbol = this.deleteForm.controls.stockSymbol.value;
    if (!stockSymbol || typeof stockSymbol === 'string') {
      return;
    }

    const stockSymbolId = stockSymbol.id?.trim();
    if (!stockSymbolId) {
      return;
    }

    this.isDeleting = true;
    this.stockSymbolApiService
      .delete(stockSymbolId)
      .pipe(finalize(() => (this.isDeleting = false)))
      .subscribe({
        next: () => {
          this.deletedStockSymbolId = stockSymbolId;
          this.deletedStockSymbol = stockSymbol;
          this.stockSymbols = this.stockSymbols.filter(
            (existingStockSymbol) => existingStockSymbol.id !== stockSymbolId,
          );
          this.deleteForm.reset();
          this.snackBar.open('Stock symbol deleted.', 'Close', {
            duration: 4000,
          });
        },
        error: () => {
          this.snackBar.open('Unable to delete stock symbol.', 'Close', {
            duration: 4000,
          });
        },
      });
  }

  private loadStockSymbols(): void {
    this.isLoadingStockSymbols = true;
    this.stockSymbolApiService
      .getPaged(0, 1000)
      .pipe(finalize(() => (this.isLoadingStockSymbols = false)))
      .subscribe({
        next: (stockSymbols) => {
          this.stockSymbols = stockSymbols.record;
          this.deleteForm.controls.stockSymbol.setValue(
            this.deleteForm.controls.stockSymbol.value,
          );
        },
        error: () => {
          this.snackBar.open('Unable to load stock symbols.', 'Close', {
            duration: 4000,
          });
        },
      });
  }

  private filterStockSymbols(
    value: StockSymbolDto | string | null,
  ): StockSymbolDto[] {
    const filterValue =
      typeof value === 'string'
        ? value.toLowerCase()
        : (value?.symbol.toLowerCase() ?? '');

    return this.stockSymbols.filter((stockSymbol) =>
      stockSymbol.symbol.toLowerCase().includes(filterValue),
    );
  }

  private stockSymbolSelectionValidator(
    control: AbstractControl,
  ): ValidationErrors | null {
    const value = control.value;
    return value &&
      typeof value === 'object' &&
      'id' in value &&
      typeof value.id === 'string' &&
      value.id.trim()
      ? null
      : { stockSymbolSelection: true };
  }
}
