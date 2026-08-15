import { MatIconButton } from '@angular/material/button';
import { CardIoDataType } from './card-io-data-type.model';

export class NetworkProgrammerCardOutput {
  public connectButton: MatIconButton | undefined;
  public outputType: CardIoDataType;

  constructor(outputType: CardIoDataType) {
    this.outputType = outputType;
  }
}
