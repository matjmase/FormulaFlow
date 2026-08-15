import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCardIoLabelComponent } from './strategy-manager-card-io-label.component';

describe('StrategyManagerCardIoLabelComponent', () => {
  let component: StrategyManagerCardIoLabelComponent;
  let fixture: ComponentFixture<StrategyManagerCardIoLabelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCardIoLabelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCardIoLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
