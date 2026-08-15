import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CardIoDataType } from '../../models/card-io-data-type.model';

@Component({
  selector: 'app-strategy-manager-card-io-icon',
  standalone: false,
  templateUrl: './strategy-manager-card-io-icon.component.html',
  styleUrl: './strategy-manager-card-io-icon.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCardIoIconComponent {
  public noneInput = CardIoDataType.None;
  public boolInput = CardIoDataType.Boolean;
  public numberInput = CardIoDataType.Number;

  @Input() CardIoDataType!: CardIoDataType;
}
