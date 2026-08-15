import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CardIoDataType } from '../../models/card-io-data-type.model';

@Component({
  selector: 'app-strategy-manager-card-io-label',
  standalone: false,
  templateUrl: './strategy-manager-card-io-label.component.html',
  styleUrl: './strategy-manager-card-io-label.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCardIoLabelComponent {
  public noneInput = CardIoDataType.None;
  public boolInput = CardIoDataType.Boolean;
  public numberInput = CardIoDataType.Number;

  @Input() cardIoDataType!: CardIoDataType;
}
