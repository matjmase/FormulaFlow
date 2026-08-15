import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { LineConnector } from '../../models/line-connector.model';

@Component({
  selector: 'app-strategy-manager-canvas-arrow',
  standalone: false,
  templateUrl: './strategy-manager-canvas-arrow.component.html',
  styleUrl: './strategy-manager-canvas-arrow.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCanvasArrowComponent {
  @Input() public proposedConnection: LineConnector | undefined;
  @Input() public connections!: Set<LineConnector>;

  private buffer = 300;

  getLineString(connector: LineConnector): string {
    return `M ${connector.Start.X} ${connector.Start.Y} C ${
      connector.Start.X + this.buffer
    } ${connector.Start.Y}, ${connector.End.X - this.buffer} ${
      connector.End.Y
    }, ${connector.End.X} ${connector.End.Y}`;
  }

  getArrowLines(connector: LineConnector): string {
    return `M${connector.End.X - 20},${connector.End.Y + 0} L${
      connector.End.X - 20
    },${connector.End.Y - 20} L${connector.End.X + 0},${connector.End.Y + 0} L${
      connector.End.X - 20
    },${connector.End.Y + 20} L${connector.End.X - 20},${connector.End.Y + 0}`;
  }
}
