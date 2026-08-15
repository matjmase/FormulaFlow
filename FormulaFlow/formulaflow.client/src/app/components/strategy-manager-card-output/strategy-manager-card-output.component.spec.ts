import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCardOutputComponent } from './strategy-manager-card-output.component';

describe('StrategyManagerCardOutputComponent', () => {
  let component: StrategyManagerCardOutputComponent;
  let fixture: ComponentFixture<StrategyManagerCardOutputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCardOutputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCardOutputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
