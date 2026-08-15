import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockManagerStockDataUploadComponent } from './stock-manager-stock-data-upload.component';

describe('StockManagerStockDataUploadComponent', () => {
  let component: StockManagerStockDataUploadComponent;
  let fixture: ComponentFixture<StockManagerStockDataUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StockManagerStockDataUploadComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockManagerStockDataUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
