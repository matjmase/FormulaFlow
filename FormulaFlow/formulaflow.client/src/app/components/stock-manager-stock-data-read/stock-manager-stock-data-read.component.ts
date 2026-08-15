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

  public isReading = false;
  public pageIndex = 0;
  public pageSize = 10;
  public readonly pageSizeOptions = [5, 10, 25, 50];
  public stockDataEntries: PagedData<StockDataEntryDto> | null = null;

  public ngOnInit(): void {
    this.readStockDataEntries();
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
        page: pageIndex,
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
