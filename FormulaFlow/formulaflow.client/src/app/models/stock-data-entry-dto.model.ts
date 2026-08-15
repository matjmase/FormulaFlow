import { BaseIdDto } from './base-id-dto.model';

export interface StockDataEntryDto extends BaseIdDto {
  stockSymbolId: string;
  date: string;
  amount: number;
}
