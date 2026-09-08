import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  OnInit,
} from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EMPTY, expand, finalize, reduce } from 'rxjs';
import { StockSymbolDto } from '../../models/stock-symbol-dto.model';
import { StockSymbolApiService } from '../../services/api/stock-symbol-api.service';
import { PagedData } from '../../models/paged-data.model';
import { StockDataEntryDto } from '../../models/stock-data-entry-dto.model';
import { StockDataEntryApiService } from '../../services/api/stock-data-entry-api.service';

@Component({
  selector: 'app-stock-manager-stock-data-read',
  standalone: false,
  templateUrl: './stock-manager-stock-data-read.component.html',
  styleUrl: './stock-manager-stock-data-read.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StockManagerStockDataReadComponent implements OnInit {
  private readonly stockDataEntryApiService = inject(StockDataEntryApiService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly stockSymbolApiService = inject(StockSymbolApiService);
  private readonly destroyRef = inject(DestroyRef);

  public stockSymbolId: string | undefined;
  public startDate: Date | undefined;
  public endDate: Date | undefined;
  public stockSymbols: StockSymbolDto[] = [];
  public isLoadingSymbols = false;
  private filters: { stockSymbolId?: string; startDate?: Date; endDate?: Date } = {};

  public isReading = false;
  public pageIndex = 0;
  public pageSize = 10;
  public readonly pageSizeOptions = [5, 10, 25, 50];
  public stockDataEntries: PagedData<StockDataEntryDto> | null = null;

  public ngOnInit(): void {
    this.readStockSymbols();
    this.readStockDataEntries();
  }

  public get invalidDateRange(): boolean {
    return !!(this.startDate && this.endDate &&
      new Date(this.startDate) > new Date(this.endDate));
  }

  public applyFilters(): void {
    if (this.invalidDateRange || this.isReading) return;

    this.filters = {
      stockSymbolId: this.stockSymbolId,
      startDate: this.startDate ? new Date(this.startDate) : undefined,
      endDate: this.endDate ? new Date(this.endDate) : undefined,
    };
    this.readStockDataEntries(0);
  }

  private readStockSymbols(): void {
    this.isLoadingSymbols = true;
    this.stockSymbolApiService.getPaged(0, 100).pipe(
      expand((page) => page.page + 1 < page.totalPages
        ? this.stockSymbolApiService.getPaged(page.page + 1, page.pageSize)
        : EMPTY),
      reduce((symbols, page) => symbols.concat(page.record), [] as StockSymbolDto[]),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => (this.isLoadingSymbols = false)),
    ).subscribe({
      next: (symbols) => { this.stockSymbols = symbols; },
      error: () => {
        this.snackBar.open('Unable to load stock symbols.', 'Close', { duration: 4000 });
      },
    });
  }

  public onPageChange(event: PageEvent): void {
    this.readStockDataEntries(event.pageIndex, event.pageSize);
  }

  public readStockDataEntries(
    pageIndex = this.pageIndex,
    pageSize = this.pageSize,
  ): void {
    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.isReading = true;

    this.stockDataEntryApiService
      .getPaged({
        ...this.filters,
        pageIndex,
        pageSize,
      })
      .pipe(finalize(() => (this.isReading = false)))
      .subscribe({
        next: (stockDataEntries) => {
          this.stockDataEntries = stockDataEntries;
          this.pageIndex = stockDataEntries.page;
          this.pageSize = stockDataEntries.pageSize;
        },
        error: () => {
          this.snackBar.open('Unable to load stock data.', 'Close', {
            duration: 4000,
          });
        },
      });
  }
}
