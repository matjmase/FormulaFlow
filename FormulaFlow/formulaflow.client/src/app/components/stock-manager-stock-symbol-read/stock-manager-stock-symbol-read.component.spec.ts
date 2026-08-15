import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockManagerStockSymbolReadComponent } from './stock-manager-stock-symbol-read.component';

describe('StockManagerStockSymbolReadComponent', () => {
  let component: StockManagerStockSymbolReadComponent;
  let fixture: ComponentFixture<StockManagerStockSymbolReadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StockManagerStockSymbolReadComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockManagerStockSymbolReadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
