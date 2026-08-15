import { BaseIdDto } from './base-id-dto.model';
import { CardIoDataType } from './card-io-data-type.model';
import { NetworkCardType } from './network-card-type.model';
import { OrderedLinkDto } from './ordered-link-dto.model';
import { OrderedLinkNaiveDto } from './ordered-link-naive-dto.model';
import { StockParameterDto } from './stock-parameter-dto.model';

export interface StockCardDto extends BaseIdDto {
  canvasId: string;
  label: string;
  description: string;
  defaultName: string;
  name: string;
  top: number;
  left: number;
  type: NetworkCardType;
  multiInput: boolean;
  input: CardIoDataType;
  output: CardIoDataType;
  pointsFromCards?: OrderedLinkDto[] | null;
  naiveId: number;
  naivePointsToCardNaiveId?: OrderedLinkNaiveDto[] | null;
  parameters: StockParameterDto[];
}
