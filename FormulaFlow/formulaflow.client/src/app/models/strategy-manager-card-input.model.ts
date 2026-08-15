import { MatIconButton } from '@angular/material/button';
import { CardIoDataType } from './card-io-data-type.model';

export class NetworkProgrammerCardInput {
  public connectButton: MatIconButton | undefined;
  public inputType: CardIoDataType;
  public order: number;

  constructor(inputType: CardIoDataType, order: number) {
    this.inputType = inputType;
    this.order = order;
  }
}
