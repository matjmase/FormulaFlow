import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { CardCatalogApiService } from './card-catalog-api.service';
import { CardIoDataType } from '../../models/card-io-data-type.model';
import { NetworkCardType } from '../../models/network-card-type.model';
import { StockCardDto } from '../../models/stock-card-dto.model';

describe('CardCatalogApiService', () => {
  let service: CardCatalogApiService;
  let httpMock: HttpTestingController;

  const card: StockCardDto = {
    canvasId: '11111111-1111-1111-1111-111111111111',
    label: 'Stock source',
    description: 'Provides stock data',
    defaultName: 'Stock source',
    name: 'Stock source',
    top: 0,
    left: 0,
    type: NetworkCardType.DataSource,
    multiInput: false,
    input: CardIoDataType.None,
    output: CardIoDataType.Number,
    naiveId: 0,
    parameters: [],
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(CardCatalogApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get the card catalog', () => {
    service.get().subscribe((result) => {
      expect(result).toEqual([card]);
    });

    const req = httpMock.expectOne('/api/CardCatalog');
    expect(req.request.method).toBe('GET');
    expect(req.request.withCredentials).toBeTrue();
    req.flush([card]);
  });
});
