import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { CardIoDataType } from '../../models/card-io-data-type.model';
import { StockCardDto } from '../../models/stock-card-dto.model';
import { CardCatalogApiService } from '../../services/api/card-catalog-api.service';

@Component({
  selector: 'app-strategy-manager-canvas-side-toolbar',
  standalone: false,
  templateUrl: './strategy-manager-canvas-side-toolbar.component.html',
  styleUrl: './strategy-manager-canvas-side-toolbar.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCanvasSideToolbarComponent implements OnInit {
  @Output() cardSelected: EventEmitter<StockCardDto> =
    new EventEmitter<StockCardDto>();

  public inputFilter: CardIoDataType | undefined;
  public outputFilter: CardIoDataType | undefined;

  public logicModuleInputs: CardIoDataType[] = [
    CardIoDataType.None,
    CardIoDataType.Boolean,
    CardIoDataType.Number,
  ];

  public logicModuleOutputs: CardIoDataType[] = [
    CardIoDataType.None,
    CardIoDataType.Boolean,
    CardIoDataType.Number,
  ];

  @Input() public startDate: Date | undefined;
  @Output() public startDateChange = new EventEmitter<Date>();

  public get startDateField(): Date | undefined {
    return this.startDate;
  }
  public set startDateField(value: Date | undefined) {
    this.startDate = value;
    this.startDateChange.emit(value);
  }

  @Input() public endDate: Date | undefined;
  @Output() public endDateChange = new EventEmitter<Date>();

  public get endDateField(): Date | undefined {
    return this.endDate;
  }
  public set endDateField(value: Date | undefined) {
    this.endDate = value;
    this.endDateChange.emit(value);
  }

  public noneInput = CardIoDataType.None;
  public boolInput = CardIoDataType.Boolean;
  public numberInput = CardIoDataType.Number;

  public totalCardCatalog: StockCardDto[] = [];
  public cardCatalog: StockCardDto[] = [];

  constructor(private catalogService: CardCatalogApiService) {}

  ngOnInit(): void {
    this.catalogService.get().subscribe((cards) => {
      this.totalCardCatalog = cards;
      this.selectionFilterChanged();
    });
  }

  public selectionFilterChanged(): void {
    this.cardCatalog = this.totalCardCatalog.filter((card) => {
      if (this.inputFilter && this.inputFilter !== card.input) {
        return false;
      } else if (this.outputFilter && this.outputFilter !== card.output) {
        return false;
      }
      return true;
    });
  }

  public cardClicked(card: StockCardDto) {
    this.cardSelected.emit(card);
  }
}
