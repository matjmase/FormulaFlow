import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockSymbolAutoCompleteComponent } from './stock-symbol-auto-complete.component';

describe('StockSymbolAutoCompleteComponent', () => {
  let component: StockSymbolAutoCompleteComponent;
  let fixture: ComponentFixture<StockSymbolAutoCompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StockSymbolAutoCompleteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StockSymbolAutoCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
