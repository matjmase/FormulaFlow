import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockManagerStockSymbolComponent } from './stock-manager-stock-symbol.component';

describe('StockManagerStockSymbolComponent', () => {
  let component: StockManagerStockSymbolComponent;
  let fixture: ComponentFixture<StockManagerStockSymbolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StockManagerStockSymbolComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockManagerStockSymbolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
