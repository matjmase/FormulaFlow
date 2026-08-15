import { BaseIdDto } from './base-id-dto.model';

export interface StockSymbolDto extends BaseIdDto {
  symbol: string;
}
