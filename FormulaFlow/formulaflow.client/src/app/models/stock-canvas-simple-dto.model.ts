import { BaseIdDto } from './base-id-dto.model';

export interface StockCanvasSimpleDto extends BaseIdDto {
  name: string;
  scale: number;
  height: number;
  width: number;
}
