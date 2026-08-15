import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockManagerStockDataReadComponent } from './stock-manager-stock-data-read.component';

describe('StockManagerStockDataReadComponent', () => {
  let component: StockManagerStockDataReadComponent;
  let fixture: ComponentFixture<StockManagerStockDataReadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StockManagerStockDataReadComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockManagerStockDataReadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
