import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { MatIconButton } from '@angular/material/button';
import { BehaviorSubject } from 'rxjs';
import { CardIoDataType } from '../../models/card-io-data-type.model';
import { NetworkProgrammerCardOutput } from '../../models/strategy-manager-card-output.model';

@Component({
  selector: 'app-strategy-manager-card-output',
  standalone: false,
  templateUrl: './strategy-manager-card-output.component.html',
  styleUrl: './strategy-manager-card-output.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCardOutputComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  @Input() initialized!: BehaviorSubject<number>;

  @ViewChild('connectButton')
  public buttonTarget!: MatIconButton;

  @Output() public initializeEvent =
    new EventEmitter<NetworkProgrammerCardOutput>();

  @Output() public connectEvent =
    new EventEmitter<NetworkProgrammerCardOutput>();

  @Output() public removeConnectEvent =
    new EventEmitter<NetworkProgrammerCardOutput>();

  @Output() public destroyEvent =
    new EventEmitter<NetworkProgrammerCardOutput>();

  @Input()
  public model!: NetworkProgrammerCardOutput;

  public noOutput = CardIoDataType.None;

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    this.model.connectButton = this.buttonTarget;

    this.initializeEvent.emit(this.model);

    this.initialized.next(this.initialized.value - 1);
  }

  ngOnDestroy(): void {
    this.destroyEvent.emit(this.model);
  }

  public connectInput(): void {
    this.connectEvent.emit(this.model);
  }

  public removeConnectInput(): void {
    this.removeConnectEvent.emit(this.model);
  }
}
