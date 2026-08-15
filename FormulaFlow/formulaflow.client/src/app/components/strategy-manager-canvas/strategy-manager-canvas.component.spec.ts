import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCanvasComponent } from './strategy-manager-canvas.component';

describe('StrategyManagerCanvasComponent', () => {
  let component: StrategyManagerCanvasComponent;
  let fixture: ComponentFixture<StrategyManagerCanvasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCanvasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCanvasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
