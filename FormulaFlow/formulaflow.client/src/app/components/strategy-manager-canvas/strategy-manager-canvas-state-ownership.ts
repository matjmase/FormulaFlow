import { StockCardDto } from '../../models/stock-card-dto.model';
import { NetworkProgrammerCardInput } from '../../models/strategy-manager-card-input.model';
import { NetworkProgrammerCardOutput } from '../../models/strategy-manager-card-output.model';
import { IoModel } from './strategy-manager-canvas-state-types';

export class StrategyManagerCanvasStateOwnership {
  private cardInputs = new Map<StockCardDto, NetworkProgrammerCardInput[]>();
  private cardOutputs = new Map<StockCardDto, NetworkProgrammerCardOutput>();

  private ioCard = new Map<IoModel, StockCardDto>();

  public addInput(card: StockCardDto, input: NetworkProgrammerCardInput): void {
    // Ensure the card is in the map
    if (!this.cardInputs.has(card)) {
      this.cardInputs.set(card, []);
    }

    this.cardInputs.get(card)!.push(input);
    this.ioCard.set(input, card);
  }

  public removeInput(
    card: StockCardDto,
    input: NetworkProgrammerCardInput,
  ): void {
    this.ioCard.delete(input);
    const inputs = this.cardInputs.get(card);
    if (!inputs) return;

    const index = inputs.indexOf(input);
    if (index === -1) return;

    inputs.splice(index, 1);

    // Cleanup if no inputs remain for the card
    if (inputs.length === 0) {
      this.cardInputs.delete(card);
    }
  }

  public addOutput(
    card: StockCardDto,
    output: NetworkProgrammerCardOutput,
  ): void {
    this.cardOutputs.set(card, output);
    this.ioCard.set(output, card);
  }

  public removeOutput(
    card: StockCardDto,
    output: NetworkProgrammerCardOutput,
  ): void {
    this.ioCard.delete(output);
    this.cardOutputs.delete(card);
  }

  public getCardInputs(
    card: StockCardDto,
  ): NetworkProgrammerCardInput[] | undefined {
    return this.cardInputs.get(card);
  }

  public getCardOutput(
    card: StockCardDto,
  ): NetworkProgrammerCardOutput | undefined {
    return this.cardOutputs.get(card);
  }

  public getCardForIoModel(ioModel: IoModel): StockCardDto | undefined {
    return this.ioCard.get(ioModel);
  }
}
