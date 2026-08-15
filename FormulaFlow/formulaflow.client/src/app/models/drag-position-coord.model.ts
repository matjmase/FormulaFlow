import { PositionCoord } from './position-coord.model';

export interface DragPositionCoord extends PositionCoord {
  roundOffX: number;
  roundOffY: number;

  div: HTMLDivElement;
}
