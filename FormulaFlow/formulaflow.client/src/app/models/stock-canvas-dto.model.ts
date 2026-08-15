import { StockCanvasSimpleDto } from './stock-canvas-simple-dto.model';
import { StockCardDto } from './stock-card-dto.model';

export interface StockCanvasDto extends StockCanvasSimpleDto {
  cards: StockCardDto[];
}
