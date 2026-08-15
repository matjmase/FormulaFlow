import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { finalize } from 'rxjs';
import { StockSymbolDto } from '../../models/stock-symbol-dto.model';
import { StockSymbolApiService } from '../../services/api/stock-symbol-api.service';

@Component({
  selector: 'app-stock-manager-stock-symbol-create',
  standalone: false,
  templateUrl: './stock-manager-stock-symbol-create.component.html',
  styleUrl: './stock-manager-stock-symbol-create.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StockManagerStockSymbolCreateComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly stockSymbolApiService = inject(StockSymbolApiService);
  private readonly snackBar = inject(MatSnackBar);

  public isCreating = false;
  public createdStockSymbol: StockSymbolDto | null = null;

  public createForm = this.formBuilder.nonNullable.group({
    symbol: ['', [Validators.required, Validators.maxLength(20)]],
  });

  public createStockSymbol(): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    const symbol = this.createForm.controls.symbol.value.trim().toUpperCase();

    this.isCreating = true;
    this.stockSymbolApiService
      .create({ symbol })
      .pipe(finalize(() => (this.isCreating = false)))
      .subscribe({
        next: (createdStockSymbol) => {
          this.createdStockSymbol = createdStockSymbol;
          this.createForm.reset();
          this.snackBar.open('Stock symbol created.', 'Close', {
            duration: 4000,
          });
        },
        error: () => {
          this.snackBar.open('Unable to create stock symbol.', 'Close', {
            duration: 4000,
          });
        },
      });
  }
}
