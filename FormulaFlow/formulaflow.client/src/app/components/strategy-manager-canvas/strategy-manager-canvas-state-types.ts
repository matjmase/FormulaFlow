import { LineConnector } from '../../models/line-connector.model';
import { NetworkProgrammerCardInput } from '../../models/strategy-manager-card-input.model';
import { NetworkProgrammerCardOutput } from '../../models/strategy-manager-card-output.model';

export type IoModel = NetworkProgrammerCardInput | NetworkProgrammerCardOutput;

export interface ParentInfo {
  output: NetworkProgrammerCardOutput;
  connector: LineConnector;
}

export type InputConnectorMap = Map<NetworkProgrammerCardInput, LineConnector>;
