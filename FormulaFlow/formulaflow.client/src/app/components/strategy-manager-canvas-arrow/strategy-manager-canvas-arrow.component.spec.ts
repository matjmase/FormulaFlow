import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCanvasArrowComponent } from './strategy-manager-canvas-arrow.component';

describe('StrategyManagerCanvasArrowComponent', () => {
  let component: StrategyManagerCanvasArrowComponent;
  let fixture: ComponentFixture<StrategyManagerCanvasArrowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCanvasArrowComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCanvasArrowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
