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
import { NetworkProgrammerCardInput } from '../../models/strategy-manager-card-input.model';

@Component({
  selector: 'app-strategy-manager-card-input',
  standalone: false,
  templateUrl: './strategy-manager-card-input.component.html',
  styleUrl: './strategy-manager-card-input.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCardInputComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  @Input() initialized!: BehaviorSubject<number>;

  @ViewChild('connectButton')
  public buttonTarget!: MatIconButton;

  @Output() public initializeEvent =
    new EventEmitter<NetworkProgrammerCardInput>();

  @Output() public connectEvent =
    new EventEmitter<NetworkProgrammerCardInput>();

  @Output() public removeConnectEvent =
    new EventEmitter<NetworkProgrammerCardInput>();

  @Output() public destroyEvent =
    new EventEmitter<NetworkProgrammerCardInput>();

  @Input()
  public model!: NetworkProgrammerCardInput;

  public noInput = CardIoDataType.None;

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
