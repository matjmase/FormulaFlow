import { StockCardDto } from './stock-card-dto.model';
import { NetworkProgrammerCardInput } from './strategy-manager-card-input.model';

export interface CardAndInput {
  card: StockCardDto;
  input: NetworkProgrammerCardInput;
}
