import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockManagerStockSymbolCreateComponent } from './stock-manager-stock-symbol-create.component';

describe('StockManagerStockSymbolCreateComponent', () => {
  let component: StockManagerStockSymbolCreateComponent;
  let fixture: ComponentFixture<StockManagerStockSymbolCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StockManagerStockSymbolCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockManagerStockSymbolCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
