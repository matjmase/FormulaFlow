import { LineConnector } from '../../models/line-connector.model';
import { NetworkProgrammerCardInput } from '../../models/strategy-manager-card-input.model';
import { NetworkProgrammerCardOutput } from '../../models/strategy-manager-card-output.model';
import {
  ParentInfo,
  InputConnectorMap,
} from './strategy-manager-canvas-state-types';

export class StrategyManagerCanvasStateRelationship {
  private connections: Set<LineConnector> = new Set<LineConnector>();

  private parentCards: Map<NetworkProgrammerCardInput, ParentInfo> = new Map<
    NetworkProgrammerCardInput,
    ParentInfo
  >();

  private childCards: Map<NetworkProgrammerCardOutput, InputConnectorMap> =
    new Map<NetworkProgrammerCardOutput, InputConnectorMap>();

  public addConnection(
    input: NetworkProgrammerCardInput,
    output: NetworkProgrammerCardOutput,
    connector: LineConnector,
  ): void {
    this.connections.add(connector);
    this.parentCards.set(input, { output, connector });

    if (!this.childCards.has(output)) {
      this.childCards.set(
        output,
        new Map<NetworkProgrammerCardInput, LineConnector>(),
      );
    }

    this.childCards.get(output)!.set(input, connector);
  }

  public removeConnectionForInput(input: NetworkProgrammerCardInput): void {
    const info = this.parentCards.get(input);
    if (!info) return;
    const { output, connector } = info;

    this.parentCards.delete(input);
    this.childCards.get(output)!.delete(input);
    this.connections.delete(connector);
  }

  public removeConnectionsForOutput(output: NetworkProgrammerCardOutput): void {
    if (this.childCards.has(output)) {
      const inputsAndConnects = Array.from(
        this.childCards.get(output)!.entries(),
      );
      this.childCards.delete(output);

      for (let [inputModel] of inputsAndConnects) {
        this.removeConnectionForInput(inputModel);
      }
    }
  }

  public getConnectorsToParents(
    inputs: NetworkProgrammerCardInput[],
  ): LineConnector[] {
    const output: LineConnector[] = [];

    for (let inputModel of inputs) {
      const info = this.parentCards.get(inputModel);
      if (info) output.push(info.connector);
    }

    return output;
  }

  public getConnectorsToChildren(
    outputModel: NetworkProgrammerCardOutput,
  ): LineConnector[] {
    const output: LineConnector[] = [];

    if (this.childCards.has(outputModel)) {
      for (let [, connector] of this.childCards.get(outputModel)!) {
        output.push(connector);
      }
    }

    return output;
  }

  public getOutputOfParent(
    input: NetworkProgrammerCardInput,
  ): NetworkProgrammerCardOutput | undefined {
    const info = this.parentCards.get(input);
    return info ? info.output : undefined;
  }

  public getConnectors(): Set<LineConnector> {
    return this.connections;
  }

  public inputHasParent(input: NetworkProgrammerCardInput): boolean {
    return this.parentCards.has(input);
  }
}
