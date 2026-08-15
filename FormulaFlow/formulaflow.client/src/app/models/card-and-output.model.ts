import { StockCardDto } from './stock-card-dto.model';
import { NetworkProgrammerCardOutput } from './strategy-manager-card-output.model';

export interface CardAndOutput {
  card: StockCardDto;
  output: NetworkProgrammerCardOutput;
}
