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
import {
  UploadFileModelDtoCollisionBehavior,
  UploadFileModelDto,
} from '../../models/upload-file-model-dto.model';
import { StockDataEntryApiService } from '../../services/api/stock-data-entry-api.service';
import { StockSymbolApiService } from '../../services/api/stock-symbol-api.service';

@Component({
  selector: 'app-stock-manager-stock-data-upload',
  standalone: false,
  templateUrl: './stock-manager-stock-data-upload.component.html',
  styleUrl: './stock-manager-stock-data-upload.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StockManagerStockDataUploadComponent implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly stockDataEntryApiService = inject(StockDataEntryApiService);
  private readonly stockSymbolApiService = inject(StockSymbolApiService);

  public readonly collisionBehaviorOptions = [
    {
      label: 'Skip existing',
      value: UploadFileModelDtoCollisionBehavior.SkipExisting,
    },
    {
      label: 'Overwrite existing',
      value: UploadFileModelDtoCollisionBehavior.OverwriteExisting,
    },
    {
      label: 'Create new entry',
      value: UploadFileModelDtoCollisionBehavior.CreateNewEntry,
    },
  ];

  public filteredStockSymbols$: Observable<StockSymbolDto[]> | null = null;
  public isLoadingStockSymbols = false;
  public isUploading = false;
  public selectedFile: File | null = null;
  public stockSymbols: StockSymbolDto[] = [];
  public uploadedFileName: string | null = null;

  public uploadForm = this.formBuilder.nonNullable.group({
    stockSymbol: [
      null as StockSymbolDto | string | null,
      [Validators.required, this.stockSymbolSelectionValidator],
    ],
    skipHeader: [true],
    dateColumnIndex: [0, [Validators.required, Validators.min(0)]],
    valueColumnIndex: [1, [Validators.required, Validators.min(0)]],
    collisionBehavior: [
      UploadFileModelDtoCollisionBehavior.SkipExisting,
      [Validators.required],
    ],
  });

  public ngOnInit(): void {
    this.filteredStockSymbols$ =
      this.uploadForm.controls.stockSymbol.valueChanges.pipe(
        startWith(this.uploadForm.controls.stockSymbol.value),
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

  public onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.item(0) ?? null;
    this.uploadedFileName = null;
  }

  public uploadStockData(): void {
    if (this.uploadForm.invalid || !this.selectedFile) {
      this.uploadForm.markAllAsTouched();
      return;
    }

    const stockSymbol = this.uploadForm.controls.stockSymbol.value;
    if (!stockSymbol || typeof stockSymbol === 'string') {
      return;
    }

    const stockSymbolId = stockSymbol.id?.trim();
    if (!stockSymbolId) {
      return;
    }

    const model: UploadFileModelDto = {
      skipHeader: this.uploadForm.controls.skipHeader.value,
      dateColumnIndex: this.uploadForm.controls.dateColumnIndex.value,
      valueColumnIndex: this.uploadForm.controls.valueColumnIndex.value,
      collisionBehavior: this.uploadForm.controls.collisionBehavior.value,
    };

    this.isUploading = true;
    this.stockDataEntryApiService
      .uploadFile(stockSymbolId, this.selectedFile, model)
      .pipe(finalize(() => (this.isUploading = false)))
      .subscribe({
        next: () => {
          this.uploadedFileName = this.selectedFile?.name ?? null;
          this.selectedFile = null;
          this.snackBar.open('Stock data uploaded.', 'Close', {
            duration: 4000,
          });
        },
        error: () => {
          this.snackBar.open('Unable to upload stock data.', 'Close', {
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
          this.uploadForm.controls.stockSymbol.setValue(
            this.uploadForm.controls.stockSymbol.value,
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
