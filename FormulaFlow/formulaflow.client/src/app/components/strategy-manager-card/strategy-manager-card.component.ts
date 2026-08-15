import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CardAndInput } from '../../models/card-and-input.model';
import { CardAndOutput } from '../../models/card-and-output.model';
import { CardIoDataType } from '../../models/card-io-data-type.model';
import { StockCardDto } from '../../models/stock-card-dto.model';
import { NetworkProgrammerCardInput } from '../../models/strategy-manager-card-input.model';
import { NetworkProgrammerCardOutput } from '../../models/strategy-manager-card-output.model';

@Component({
  selector: 'app-strategy-manager-card',
  standalone: false,
  templateUrl: './strategy-manager-card.component.html',
  styleUrl: './strategy-manager-card.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCardComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  @Input() initialized!: BehaviorSubject<number>;

  @Input() public model!: StockCardDto;
  @Input() public isClosable: boolean = true;

  @Output() public connectEvent = new EventEmitter<
    NetworkProgrammerCardInput | NetworkProgrammerCardOutput
  >();
  @Output() public removeConnectEvent = new EventEmitter<
    NetworkProgrammerCardInput | NetworkProgrammerCardOutput
  >();

  @Output() public closeEvent = new EventEmitter<StockCardDto>();

  @Output() public backTestEvent = new EventEmitter<StockCardDto>();

  @Output() public dirtyEvent = new EventEmitter<void>();

  // mouse events
  @Output() public mouseUpEvent = new EventEmitter<void>();
  @Output() public mouseLeaveEvent = new EventEmitter<MouseEvent>();
  @Output() public mouseMoveDragEvent = new EventEmitter<MouseEvent>();
  @Output() public mouseDownEvent = new EventEmitter<MouseEvent>();

  @Output() public inputAdded = new EventEmitter<CardAndInput>();
  @Output() public inputRemoved = new EventEmitter<CardAndInput>();
  @Output() public outputAdded = new EventEmitter<CardAndOutput>();
  @Output() public outputRemoved = new EventEmitter<CardAndOutput>();

  public inputs: NetworkProgrammerCardInput[] = [];

  public output: NetworkProgrammerCardOutput | undefined;

  public noInput = CardIoDataType.None;

  constructor() {}

  ngOnInit(): void {
    // parameters
    this.model.parameters = this.model.parameters.sort(
      (a, b) => a.order - b.order,
    );

    // output
    if (this.model.output !== CardIoDataType.None) {
      this.output = new NetworkProgrammerCardOutput(this.model.output!);
      this.initialized.next(this.initialized.value + 1);
    }

    // input
    if (this.model.input !== this.noInput) {
      if (this.model.pointsFromCards) {
        const maxInput =
          Math.max(...this.model.pointsFromCards.map((pfc) => pfc.order!), -1) +
          1;

        const minInput = 1;

        const total = Math.max(maxInput, minInput);

        for (let i = 0; i < total; i++) {
          this.inputs.push(
            new NetworkProgrammerCardInput(this.model.input!, i),
          );
        }

        this.initialized.next(this.initialized.value + maxInput);
      } else {
        this.inputs.push(new NetworkProgrammerCardInput(this.model.input!, 0));
        this.initialized.next(this.initialized.value + 1);
      }
    }
  }

  ngAfterViewInit(): void {
    this.initialized.next(this.initialized.value - 1);
  }

  ngOnDestroy(): void {
    setTimeout(() => {
      this.initialized.next(this.initialized.value - 1);
    });
  }

  public connectCard(
    element: NetworkProgrammerCardInput | NetworkProgrammerCardOutput,
  ): void {
    this.connectEvent.emit(element);
  }

  public removeConnectCard(
    element: NetworkProgrammerCardInput | NetworkProgrammerCardOutput,
  ): void {
    this.removeConnectEvent.emit(element);
  }

  public close(): void {
    this.closeEvent.emit(this.model);
  }

  // relay mouse actions
  public mouseUp(): void {
    this.mouseUpEvent.emit();
  }

  public mouseLeave(event: MouseEvent): void {
    this.mouseLeaveEvent.emit(event);
  }

  public mouseMoveDrag(event: MouseEvent): void {
    this.mouseMoveDragEvent.emit(event);
  }

  public mouseDown(event: MouseEvent): void {
    this.mouseDownEvent.emit(event);
  }

  public addInput(): void {
    this.inputs.push(
      new NetworkProgrammerCardInput(this.model.input!, this.inputs.length),
    );
  }

  public removeInput(): void {
    this.inputs.splice(this.inputs.length - 1, 1);
  }

  public characterAdd(add: number) {
    return String.fromCharCode('a'.charCodeAt(0) + add);
  }

  public inputInit(element: NetworkProgrammerCardInput) {
    this.inputAdded.emit({
      card: this.model,
      input: element,
    });
  }
  public inputDestroy(element: NetworkProgrammerCardInput) {
    this.inputRemoved.emit({
      card: this.model,
      input: element,
    });
  }
  public outputInit(element: NetworkProgrammerCardOutput) {
    this.outputAdded.emit({
      card: this.model,
      output: element,
    });
  }
  public outputDestroy(element: NetworkProgrammerCardOutput) {
    this.outputRemoved.emit({
      card: this.model,
      output: element,
    });
  }

  public downloadData() {
    this.backTestEvent.emit(this.model);
  }

  public setDirty() {
    this.dirtyEvent.emit();
  }
}
