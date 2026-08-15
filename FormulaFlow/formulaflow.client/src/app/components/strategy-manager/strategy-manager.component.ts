import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { StockCanvasSimpleDto } from '../../models/stock-canvas-simple-dto.model';
import { CanvasApiService } from '../../services/api/canvas-api.service';

@Component({
  selector: 'app-strategy-manager',
  standalone: false,
  templateUrl: './strategy-manager.component.html',
  styleUrl: './strategy-manager.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerComponent implements OnInit {
  items: StockCanvasSimpleDto[] = [];
  page = 1;
  pageSize = 10;
  length = 0;
  loading = false;

  constructor(
    private api: CanvasApiService,
    private router: Router,
    private snackBar: MatSnackBar,
  ) {}

  ngOnInit(): void {
    this.loadPage();
  }

  loadPage(): void {
    this.loading = true;
    this.api.getPaged(this.page, this.pageSize).subscribe({
      next: (res) => {
        this.items = res.record || [];
        this.length = res.recordCount || 0;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  pageChange(event: PageEvent): void {
    this.page = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadPage();
  }

  delete(item: StockCanvasSimpleDto): void {
    if (!confirm(`Delete "${item.name}"?`)) return;
    this.api.delete(item).subscribe({
      next: () => {
        this.loadPage();
        this.snackBar.open(`Deleted "${item.name}"`, 'Close', {
          duration: 3000,
        });
      },
      error: () =>
        this.snackBar.open(`Failed to delete "${item.name}"`, 'Close', {
          duration: 3000,
        }),
    });
  }

  addNew(): void {
    this.router.navigate(['/strategy-manager/canvas']);
  }
}
