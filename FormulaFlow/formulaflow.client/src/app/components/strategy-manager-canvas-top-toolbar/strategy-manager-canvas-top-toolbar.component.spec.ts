import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCanvasTopToolbarComponent } from './strategy-manager-canvas-top-toolbar.component';

describe('StrategyManagerCanvasTopToolbarComponent', () => {
  let component: StrategyManagerCanvasTopToolbarComponent;
  let fixture: ComponentFixture<StrategyManagerCanvasTopToolbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCanvasTopToolbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCanvasTopToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
