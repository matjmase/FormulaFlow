import { HttpResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import {
  BehaviorSubject,
  Subscription,
  Observable,
  of,
  concatMap,
  tap,
  catchError,
  throwError,
  map,
  switchMap,
  debounceTime,
  distinctUntilChanged,
} from 'rxjs';
import { CardAndInput } from '../../models/card-and-input.model';
import { CardAndOutput } from '../../models/card-and-output.model';
import { DragPositionCoord } from '../../models/drag-position-coord.model';
import { OrderedLinkNaiveDto } from '../../models/ordered-link-naive-dto.model';
import { StockCanvasDto } from '../../models/stock-canvas-dto.model';
import { StockCardDto } from '../../models/stock-card-dto.model';
import { NetworkProgrammerCardInput } from '../../models/strategy-manager-card-input.model';
import { NetworkProgrammerCardOutput } from '../../models/strategy-manager-card-output.model';
import { CanvasComplexApiService } from '../../services/api/canvas-complex-api.service';
import { StrategyManagerCanvasState } from './strategy-manager-canvas-state';
import { BackTestApiService } from '../../services/api/back-test-api.service';

@Component({
  selector: 'app-strategy-manager-canvas',
  standalone: false,
  templateUrl: './strategy-manager-canvas.component.html',
  styleUrl: './strategy-manager-canvas.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class StrategyManagerCanvasComponent implements OnInit {
  public readonly zoomNumber: number = 0.2;
  public readonly sizeDiff: number = 200;

  private isDirty: boolean = false;

  public initialized = new BehaviorSubject<number>(1);
  public initSub: Subscription | undefined;

  public dragPosition: DragPositionCoord | undefined;

  public startDate: Date | undefined;
  public endDate: Date | undefined;

  private scrollX: number = 0;
  private scrollY: number = 0;
  private canvasWidth: number = 0;
  private canvasHeight: number = 0;

  public canvas: StockCanvasDto = {
    height: 1000,
    width: 1000,
    name: 'Unnamed',
    scale: 1,
    cards: [],
  };

  private urlParam: string | null = null;

  public model: StrategyManagerCanvasState = new StrategyManagerCanvasState();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private canvasService: CanvasComplexApiService,
    private backTestService: BackTestApiService,
    private snackBar: MatSnackBar,
  ) {}

  ngOnInit(): void {
    this.endDate = this.getTodaysUtc();
    this.startDate = this.addDaysUTC(this.endDate!, -30 * 12 * 10);

    this.urlParam = this.route.snapshot.paramMap.get('canvasId');

    if (this.urlParam) {
      this.canvasService.getById(this.urlParam).subscribe((canvas) => {
        this.initializeView(canvas);
      });
    }
  }
  private initializeView(canvas: StockCanvasDto): void {
    this.model = new StrategyManagerCanvasState();

    this.canvas = canvas;

    const guidCardMap = new Map<string, StockCardDto>();

    for (let card of this.canvas.cards) {
      guidCardMap.set(card.id!, card);
    }

    this.initialized.next(0);
    this.initialized.next(this.canvas.cards.length);

    this.initSub = this.initialized.subscribe((value) => {
      if (value === 0) {
        this.initSub?.unsubscribe();
        for (let card of this.canvas.cards) {
          if (card.pointsFromCards) {
            for (let pointer of card.pointsFromCards) {
              const otherCard = guidCardMap.get(pointer.from!)!;

              const output = this.model.getCardOutput(otherCard);
              const input = this.model
                .getCardInputs(card)
                ?.find((input) => input.order === pointer.order);

              if (output && input) {
                this.model.proposeConnection(otherCard, output, this.canvas);
                this.model.proposeConnection(card, input, this.canvas);
              } else {
                throw new Error(`Could not find output or input for card.`);
              }
            }
          }
        }
      }
    });
  }

  getBackTest(card: StockCardDto) {
    let observable: Observable<HttpResponse<Blob>>;

    if (this.isDirty) {
      observable = of(card).pipe(
        concatMap(() => this.getSaveCanvas()),
        tap((canvas) => this.processNewCanvas(canvas)),
        concatMap(() => this.getBackTestObservable(card)),
        catchError((error) => {
          this.snackBar.open('The date ranges where not set', 'Close', {
            duration: 3000,
          });

          return throwError(() => new Error('The date ranges where not set'));
        }),
      );
    } else {
      observable = this.getBackTestObservable(card);
    }

    observable.subscribe((response) => {
      this.processBlobResponse(response, card);
    });
  }

  private processBlobResponse(
    response: HttpResponse<Blob>,
    card: StockCardDto,
  ) {
    const name = card.name ?? '';

    let filename = name + '.csv';

    const blob = response.body!;
    const url = window.URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();

    window.URL.revokeObjectURL(url);
  }

  private getBackTestObservable(
    card: StockCardDto,
  ): Observable<HttpResponse<Blob>> {
    if (card.id && this.startDate && this.endDate) {
      return this.backTestService
        .run(card.id, this.startDate, this.endDate)
        .pipe(map((val) => new HttpResponse({ body: val })));
    }
    return of(new HttpResponse<Blob>());
  }

  outputRemoved(model: CardAndOutput) {
    this.model.removeOutput(model.card, model.output);
  }
  outputAdded(model: CardAndOutput) {
    this.model.addOutput(model.card, model.output);
  }
  inputRemoved(model: CardAndInput) {
    this.model.removeInput(model.card, model.input);
  }
  inputAdded(model: CardAndInput) {
    this.model.addInput(model.card, model.input);
  }
  mouseMoveDrag(event: MouseEvent, div: HTMLDivElement, model: StockCardDto) {
    this.updateDragPosition(event, div, model);
  }
  mouseLeave(event: MouseEvent, div: HTMLDivElement, model: StockCardDto) {
    this.updateDragPosition(event, div, model);
  }
  mouseUp() {
    this.dragPosition = undefined;
  }
  mouseDown(event: MouseEvent, div: HTMLDivElement) {
    if (event.buttons === 1) {
      const rawX = this.getOffsetPositionX(event, div);
      const rawY = this.getOffsetPositionY(event, div);

      this.dragPosition = {
        X: this.getIntegerAmount(rawX),
        Y: this.getIntegerAmount(rawY),

        roundOffX: this.getRemainderAmount(rawX),
        roundOffY: this.getRemainderAmount(rawY),

        div: div,
      };
    }
  }
  removeConnectItem(
    card: StockCardDto,
    $event: NetworkProgrammerCardInput | NetworkProgrammerCardOutput,
  ) {
    if ($event instanceof NetworkProgrammerCardInput) {
      this.model.removeInputRelationships($event);
    } else if ($event instanceof NetworkProgrammerCardOutput) {
      this.model.removeOutputRelationships($event);
    }
  }
  connectItem(
    card: StockCardDto,
    $event: NetworkProgrammerCardInput | NetworkProgrammerCardOutput,
  ) {
    this.model.proposeConnection(card, $event, this.canvas);
  }
  closeCard($event: StockCardDto) {
    const index = this.canvas.cards.indexOf($event);
    this.canvas.cards.splice(index, 1);
  }

  // Canvas events
  onContextMenu($event: PointerEvent) {
    $event.preventDefault();
  }
  adjustedPositionY() {
    return ((this.canvas.scale! - 1) * this.canvas.height!) / 2;
  }
  adjustedPositionX() {
    return ((this.canvas.scale - 1) * this.canvas.width) / 2;
  }
  canvasMouseDown($event: MouseEvent) {
    if ($event.buttons === 2) {
      this.model.proposedConnectionSource = undefined;
      this.model.proposedConnection = undefined;
    }
  }
  canvasMouseMove($event: MouseEvent, div: HTMLDivElement) {
    if (this.model.proposedConnectionSource) {
      const bound = div.getBoundingClientRect();
      const offsetX = $event.clientX - bound.x;
      const offsetY = $event.clientY - bound.y;
      const scaledX = offsetX / this.canvas!.scale!;
      const scaledY = offsetY / this.canvas!.scale!;
      const proposed = this.model.proposedConnection;
      if (!proposed) return;

      if (
        this.model.proposedConnectionSource instanceof
        NetworkProgrammerCardInput
      ) {
        proposed.Start = {
          X: scaledX,
          Y: scaledY,
        };
      } else if (
        this.model.proposedConnectionSource instanceof
        NetworkProgrammerCardOutput
      ) {
        proposed.End = {
          X: scaledX,
          Y: scaledY,
        };
      } else {
        throw Error('Not Implemented connector type');
      }
    }
  }
  instantiateCard($event: StockCardDto) {
    const clone = <StockCardDto>JSON.parse(JSON.stringify($event));

    clone.name = clone.defaultName;

    clone.top = Math.round(
      (this.scrollY + this.canvasHeight / 2) / this.canvas.scale!,
    );
    clone.left = Math.round(
      (this.scrollX + this.canvasWidth / 2) / this.canvas.scale!,
    );

    this.canvas.cards = [...this.canvas.cards, clone];
  }
  onSubmit() {
    this.getSaveCanvas()
      .pipe(
        debounceTime(300),
        switchMap((canvas) => {
          this.processNewCanvas(canvas);
          return of(canvas);
        }),
      )
      .subscribe();
  }

  processNewCanvas(canvas: StockCanvasDto): void {
    this.isDirty = false;
    if (this.urlParam) {
      this.canvas = {
        height: 1000,
        width: 1000,
        name: 'Unnamed',
        scale: 1,
        cards: [],
      };

      this.canvas.cards = [];

      this.initializeView(canvas);
    } else {
      this.router.navigate(['/strategy-manager/canvas', canvas.id]);
    }
  }

  private getSaveCanvas(): Observable<StockCanvasDto> {
    const cardIdMap = new Map<StockCardDto, number>();

    let index = 0;
    for (let card of this.canvas.cards) {
      card.naiveId = index;

      cardIdMap.set(card, index);

      index++;
    }

    for (let card of this.canvas.cards) {
      const orderedLinks = this.model
        .getCardInputs(card)
        ?.map((input) => {
          const parentOutput = this.model.getOutputOfParent(input);

          if (parentOutput) {
            const outCard = this.model.getCardForIoModel(parentOutput)!;
            const outId = cardIdMap.get(outCard);

            return {
              link: outId,
              order: input.order,
            } as OrderedLinkNaiveDto;
          } else {
            return undefined;
          }
        })
        .filter((orderedLink) => orderedLink !== undefined);

      card.naivePointsToCardNaiveId = orderedLinks;
    }

    return this.canvasService.create(this.canvas);
  }

  // View adjustments
  zoomIn() {
    this.canvas.scale! *= 1 + this.zoomNumber;
  }
  zoomOut() {
    this.canvas.scale! *= 1 - this.zoomNumber;
  }
  enlargeX() {
    this.canvas.width += this.sizeDiff;
  }
  shrinkX() {
    this.canvas.width -= this.sizeDiff;

    this.canvas.width =
      this.canvas.width < this.sizeDiff ? this.sizeDiff : this.canvas.width;
  }
  enlargeY() {
    this.canvas!.height! += this.sizeDiff;
  }
  shrinkY() {
    this.canvas.height -= this.sizeDiff;

    this.canvas.height =
      this.canvas.height < this.sizeDiff ? this.sizeDiff : this.canvas.height;
  }

  // Private methods

  private getIntegerAmount(value: number): number {
    return Math.round(value);
  }

  private getRemainderAmount(value: number): number {
    const intAmt = this.getIntegerAmount(value);

    return value - intAmt;
  }

  private getOffsetPositionX(event: MouseEvent, div: HTMLDivElement): number {
    const parent = div.parentElement;
    if (!parent) return 0;

    const bounding = parent.getBoundingClientRect();

    const offsetX = event.pageX - bounding.left;

    return offsetX / this.canvas!.scale!;
  }

  private getOffsetPositionY(event: MouseEvent, div: HTMLDivElement): number {
    const parent = div.parentElement;
    if (!parent) return 0;

    const bounding = parent.getBoundingClientRect();

    const offsetY = event.pageY - bounding.top;

    return offsetY / this.canvas!.scale!;
  }

  private updateDragPosition(
    event: MouseEvent,
    div: HTMLDivElement,
    model: StockCardDto,
  ): void {
    if (
      this.dragPosition &&
      this.dragPosition.div === div &&
      event.buttons === 1
    ) {
      const offsetX = this.getOffsetPositionX(event, div);
      const offsetY = this.getOffsetPositionY(event, div);

      const diffX = offsetX - this.dragPosition.X + this.dragPosition.roundOffX;
      const diffY = offsetY - this.dragPosition.Y + this.dragPosition.roundOffY;

      this.dragPosition.X = offsetX;
      this.dragPosition.Y = offsetY;

      const intDiffX = this.getIntegerAmount(diffX);
      const intDiffY = this.getIntegerAmount(diffY);

      this.dragPosition.roundOffX = this.getRemainderAmount(diffX);
      this.dragPosition.roundOffY = this.getRemainderAmount(diffY);

      model.left = model.left! + intDiffX;
      model.top = model.top! + intDiffY;

      // other cards
      for (let connector of this.model.getConnectorsToParents(model)) {
        connector.End.X += intDiffX;
        connector.End.Y += intDiffY;
      }
      for (let connector of this.model.getConnectorsToChildren(model)) {
        connector.Start.X += intDiffX;
        connector.Start.Y += intDiffY;
      }

      // proposed
      const inputMap = this.model.getCardInputs(model);

      if (
        inputMap !== undefined &&
        inputMap.some((input) => input === this.model.proposedConnectionSource)
      ) {
        const pc = this.model.proposedConnection;
        if (pc && pc.End) {
          pc.End = {
            X: pc.End.X + intDiffX,
            Y: pc.End.Y + intDiffY,
          };
        }
      } else if (
        this.model.getCardOutput(model) === this.model.proposedConnectionSource
      ) {
        const pc = this.model.proposedConnection;
        if (pc && pc.Start) {
          pc.Start = {
            X: pc.Start.X + intDiffX,
            Y: pc.Start.Y + intDiffY,
          };
        }
      }
    } else {
      this.dragPosition = undefined;
    }
  }

  private convertToUtcDate(date: Date): Date {
    return new Date(
      Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()),
    );
  }

  public captureScrollPosition(event: Event): void {
    const target = event.target as HTMLElement;
    this.scrollX = target.scrollLeft;
    this.scrollY = target.scrollTop;
  }

  public captureCanvasResize(event: Event): void {
    const target = event.target as HTMLElement;
    this.canvasWidth = target.clientWidth;
    this.canvasHeight = target.clientHeight;
  }

  public setDirty() {
    if (this.initialized.value <= 0) {
      this.isDirty = true;
    }
  }

  private getTodaysUtc(): Date {
    const now = new Date();
    return new Date(
      Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate()),
    );
  }

  addDaysUTC(date: Date, days: number): Date {
    const result = new Date(date);
    result.setUTCDate(result.getUTCDate() + days);
    return result;
  }
}
