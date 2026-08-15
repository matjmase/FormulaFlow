import { BaseIdDto } from './base-id-dto.model';
import { NetworkParameterType } from './network-parameter-type.model';

export interface StockParameterDto extends BaseIdDto {
  cardId: string;
  order: number;
  type: NetworkParameterType;
  description: string;
  toolTip?: string | null;
  value: string;
}
