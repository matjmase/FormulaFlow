import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerParameterComponent } from './strategy-manager-parameter.component';

describe('StrategyManagerParameterComponent', () => {
  let component: StrategyManagerParameterComponent;
  let fixture: ComponentFixture<StrategyManagerParameterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerParameterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerParameterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
