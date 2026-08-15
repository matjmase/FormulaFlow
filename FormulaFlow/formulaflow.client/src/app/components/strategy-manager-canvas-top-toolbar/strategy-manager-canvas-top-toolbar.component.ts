import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';

@Component({
  selector: 'app-strategy-manager-canvas-top-toolbar',
  standalone: false,
  templateUrl: './strategy-manager-canvas-top-toolbar.component.html',
  styleUrl: './strategy-manager-canvas-top-toolbar.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCanvasTopToolbarComponent {
  @Input() public canvasName = '';
  @Output() public canvasNameChange = new EventEmitter<string>();

  public get canvasNameField(): string {
    return this.canvasName;
  }
  public set canvasNameField(value: string) {
    this.canvasName = value;
    this.canvasNameChange.emit(value);
  }

  @Output() public enlargeYCallback = new EventEmitter<void>();
  @Output() public shrinkYCallback = new EventEmitter<void>();
  @Output() public enlargeXCallback = new EventEmitter<void>();
  @Output() public shrinkXCallback = new EventEmitter<void>();
  @Output() public zoomInCallback = new EventEmitter<void>();
  @Output() public zoomOutCallback = new EventEmitter<void>();
  @Output() public submitCallback = new EventEmitter<void>();

  enlargeY() {
    this.enlargeYCallback.emit();
  }
  shrinkY() {
    this.shrinkYCallback.emit();
  }
  enlargeX() {
    this.enlargeXCallback.emit();
  }
  shrinkX() {
    this.shrinkXCallback.emit();
  }
  zoomIn() {
    this.zoomInCallback.emit();
  }
  zoomOut() {
    this.zoomOutCallback.emit();
  }
  onSubmit() {
    this.submitCallback.emit();
  }
}
