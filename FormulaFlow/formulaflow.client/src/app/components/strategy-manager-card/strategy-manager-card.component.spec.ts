import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCardComponent } from './strategy-manager-card.component';

describe('StrategyManagerCardComponent', () => {
  let component: StrategyManagerCardComponent;
  let fixture: ComponentFixture<StrategyManagerCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
