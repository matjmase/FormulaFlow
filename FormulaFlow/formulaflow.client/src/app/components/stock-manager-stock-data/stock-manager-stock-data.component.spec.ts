import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockManagerStockDataComponent } from './stock-manager-stock-data.component';

describe('StockManagerStockDataComponent', () => {
  let component: StockManagerStockDataComponent;
  let fixture: ComponentFixture<StockManagerStockDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StockManagerStockDataComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockManagerStockDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
