import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockManagerStockSymbolDeleteComponent } from './stock-manager-stock-symbol-delete.component';

describe('StockManagerStockSymbolDeleteComponent', () => {
  let component: StockManagerStockSymbolDeleteComponent;
  let fixture: ComponentFixture<StockManagerStockSymbolDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StockManagerStockSymbolDeleteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockManagerStockSymbolDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
