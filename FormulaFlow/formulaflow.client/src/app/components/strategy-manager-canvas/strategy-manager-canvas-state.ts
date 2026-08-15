import { LineConnector } from '../../models/line-connector.model';
import { PositionCoord } from '../../models/position-coord.model';
import { StockCanvasDto } from '../../models/stock-canvas-dto.model';
import { StockCardDto } from '../../models/stock-card-dto.model';
import { NetworkProgrammerCardInput } from '../../models/strategy-manager-card-input.model';
import { NetworkProgrammerCardOutput } from '../../models/strategy-manager-card-output.model';
import { StrategyManagerCanvasStateOwnership } from './strategy-manager-canvas-state-ownership';
import { StrategyManagerCanvasStateRelationship } from './strategy-manager-canvas-state-relationship';
import { IoModel } from './strategy-manager-canvas-state-types';

export class StrategyManagerCanvasState {
  // Connection proposal
  public proposedConnectionSource: IoModel | undefined;
  public proposedConnection: LineConnector | undefined;

  // ownership
  private ownership = new StrategyManagerCanvasStateOwnership();

  // Relationship
  private relationship = new StrategyManagerCanvasStateRelationship();

  // Methods
  public getConnectors(): Set<LineConnector> {
    return this.relationship.getConnectors();
  }

  private addConnection(
    input: NetworkProgrammerCardInput,
    output: NetworkProgrammerCardOutput,
    connector: LineConnector,
  ): void {
    this.relationship.addConnection(input, output, connector);
  }

  public getCardInputs(
    card: StockCardDto,
  ): NetworkProgrammerCardInput[] | undefined {
    return this.ownership.getCardInputs(card);
  }

  public getCardOutput(
    card: StockCardDto,
  ): NetworkProgrammerCardOutput | undefined {
    return this.ownership.getCardOutput(card);
  }

  public getCardForIoModel(ioModel: IoModel): StockCardDto | undefined {
    return this.ownership.getCardForIoModel(ioModel);
  }

  public getOutputOfParent(
    input: NetworkProgrammerCardInput,
  ): NetworkProgrammerCardOutput | undefined {
    return this.relationship.getOutputOfParent(input);
  }

  public addInput(card: StockCardDto, input: NetworkProgrammerCardInput): void {
    this.ownership.addInput(card, input);
  }

  public removeInput(
    card: StockCardDto,
    input: NetworkProgrammerCardInput,
  ): void {
    this.ownership.removeInput(card, input);

    // Remove any relationships associated with this input
    this.relationship.removeConnectionForInput(input);
  }

  public addOutput(
    card: StockCardDto,
    output: NetworkProgrammerCardOutput,
  ): void {
    this.ownership.addOutput(card, output);
  }

  public removeOutput(
    card: StockCardDto,
    output: NetworkProgrammerCardOutput,
  ): void {
    this.ownership.removeOutput(card, output);

    // Remove any relationships associated with this output
    this.relationship.removeConnectionsForOutput(output);
  }

  public removeInputRelationships(input: NetworkProgrammerCardInput): void {
    this.relationship.removeConnectionForInput(input);
  }

  public removeOutputRelationships(output: NetworkProgrammerCardOutput) {
    this.relationship.removeConnectionsForOutput(output);
  }

  public getConnectorsToParents(card: StockCardDto): LineConnector[] {
    const inputs = this.ownership.getCardInputs(card);

    if (inputs) {
      return this.relationship.getConnectorsToParents(inputs);
    } else {
      return [];
    }
  }

  public getConnectorsToChildren(card: StockCardDto): LineConnector[] {
    const outputModel = this.ownership.getCardOutput(card);

    if (!outputModel) {
      throw new Error('Card has no output, cannot get connectors to children');
    }

    return this.relationship.getConnectorsToChildren(outputModel);
  }

  public proposeConnection(
    card: StockCardDto,
    cardInputOrOutput: IoModel,
    canvas: StockCanvasDto,
  ): void {
    // Begin with no current proposed
    if (!this.proposedConnectionSource) {
      if (
        cardInputOrOutput instanceof NetworkProgrammerCardInput ||
        cardInputOrOutput instanceof NetworkProgrammerCardOutput
      ) {
        this.proposedConnectionSource = cardInputOrOutput;
      } else {
        throw new Error('Not Implemented connector type');
      }

      this.proposedConnection = this.makeConnector(cardInputOrOutput, canvas);
    }
    // You have a current proposed
    else {
      if (cardInputOrOutput instanceof NetworkProgrammerCardInput) {
        const input = cardInputOrOutput;
        if (
          this.proposedConnectionSource instanceof NetworkProgrammerCardOutput
        ) {
          const output = this.proposedConnectionSource;

          if (card === this.ownership.getCardForIoModel(output)) {
            throw new Error('Cannot connect card to itself.');
          }

          this.processInputOutputLink(input, output, canvas);
        } else {
          throw new Error('Card input can only be linked to card output');
        }
      } else if (cardInputOrOutput instanceof NetworkProgrammerCardOutput) {
        const output = cardInputOrOutput;
        if (
          this.proposedConnectionSource instanceof NetworkProgrammerCardInput
        ) {
          const input = this.proposedConnectionSource;

          if (card === this.ownership.getCardForIoModel(input)) {
            throw new Error('Cannot connect card to itself.');
          }

          this.processInputOutputLink(input, output, canvas);
        } else {
          throw new Error('Card output can only be linked to card input');
        }
      } else {
        throw new Error('Not Implemented connector type');
      }
    }
  }

  private processInputOutputLink(
    input: NetworkProgrammerCardInput,
    output: NetworkProgrammerCardOutput,
    canvas: StockCanvasDto,
  ) {
    if (input.inputType !== output.outputType) {
      throw new Error('Card data types are not compatible.');
    } else if (this.relationship.inputHasParent(input)) {
      throw new Error('Input can only have one link');
    }
    const cardPos = this.getCenterButtonPosition(input, canvas);
    const optionPos = this.getCenterButtonPosition(output, canvas);

    const connector = new LineConnector(optionPos, cardPos);

    this.addConnection(input, output, connector);

    this.proposedConnection = undefined;
    this.proposedConnectionSource = undefined;
  }

  private getCenterButtonPosition(
    container: NetworkProgrammerCardInput | NetworkProgrammerCardOutput,
    canvas: StockCanvasDto,
  ): PositionCoord {
    const elem = container.connectButton?._elementRef?.nativeElement as
      | HTMLElement
      | undefined;
    if (!elem) {
      throw new Error('connectButton element not available');
    }
    const bound = elem.getBoundingClientRect();
    const origX = bound.left + bound.width / 2;
    const origY = bound.top + bound.height / 2;

    let parent = elem.parentElement;

    while (parent && !parent.classList.contains('display-canvas')) {
      parent = parent.parentElement;
    }

    if (parent === null) {
      throw new Error('parent could not be found');
    }

    const boundParent = parent.getBoundingClientRect();
    const parentX = boundParent.x;
    const parentY = boundParent.y;

    const diffX = origX - parentX;
    const diffY = origY - parentY;

    const scaledX = diffX / canvas.scale!;
    const scaledY = diffY / canvas.scale!;

    return {
      X: scaledX,
      Y: scaledY,
    };
  }

  private makeConnector(
    container: NetworkProgrammerCardInput | NetworkProgrammerCardOutput,
    canvas: StockCanvasDto,
  ): LineConnector {
    const posCoord = this.getCenterButtonPosition(container, canvas);

    return new LineConnector(
      {
        X: posCoord.X,
        Y: posCoord.Y,
      },
      {
        X: posCoord.X,
        Y: posCoord.Y,
      },
    );
  }
}
