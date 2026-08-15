import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
} from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { finalize } from 'rxjs';
import { PagedData } from '../../models/paged-data.model';
import { StockSymbolDto } from '../../models/stock-symbol-dto.model';
import { StockSymbolApiService } from '../../services/api/stock-symbol-api.service';

@Component({
  selector: 'app-stock-manager-stock-symbol-read',
  standalone: false,
  templateUrl: './stock-manager-stock-symbol-read.component.html',
  styleUrl: './stock-manager-stock-symbol-read.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StockManagerStockSymbolReadComponent implements OnInit {
  private readonly stockSymbolApiService = inject(StockSymbolApiService);
  private readonly snackBar = inject(MatSnackBar);

  public isReading = false;
  public pageIndex = 0;
  public pageSize = 10;
  public readonly pageSizeOptions = [5, 10, 25, 50];
  public stockSymbols: PagedData<StockSymbolDto> | null = null;

  public ngOnInit(): void {
    this.readStockSymbols();
  }

  public onPageChange(event: PageEvent): void {
    this.readStockSymbols(event.pageIndex, event.pageSize);
  }

  public readStockSymbols(
    pageIndex = this.pageIndex,
    pageSize = this.pageSize,
  ): void {
    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.isReading = true;

    this.stockSymbolApiService
      .getPaged(pageIndex, pageSize)
      .pipe(finalize(() => (this.isReading = false)))
      .subscribe({
        next: (stockSymbols) => {
          this.stockSymbols = stockSymbols;
          this.pageIndex = stockSymbols.page;
          this.pageSize = stockSymbols.pageSize;
        },
        error: () => {
          this.snackBar.open('Unable to load stock symbols.', 'Close', {
            duration: 4000,
          });
        },
      });
  }
}
