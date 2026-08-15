import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StrategyManagerCardInputComponent } from './strategy-manager-card-input.component';

describe('StrategyManagerCardInputComponent', () => {
  let component: StrategyManagerCardInputComponent;
  let fixture: ComponentFixture<StrategyManagerCardInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StrategyManagerCardInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StrategyManagerCardInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
