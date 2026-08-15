import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCardIoIconComponent } from './strategy-manager-card-io-icon.component';

describe('StrategyManagerCardIoIconComponent', () => {
  let component: StrategyManagerCardIoIconComponent;
  let fixture: ComponentFixture<StrategyManagerCardIoIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCardIoIconComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCardIoIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
