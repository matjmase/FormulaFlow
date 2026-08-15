import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCanvasSideToolbarComponent } from './strategy-manager-canvas-side-toolbar.component';

describe('StrategyManagerCanvasSideToolbarComponent', () => {
  let component: StrategyManagerCanvasSideToolbarComponent;
  let fixture: ComponentFixture<StrategyManagerCanvasSideToolbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCanvasSideToolbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCanvasSideToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
