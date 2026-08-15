import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { StockDataEntryApiService } from './stock-data-entry-api.service';
import { PagedData } from '../../models/paged-data.model';
import { StockDataEntryDto } from '../../models/stock-data-entry-dto.model';
import {
  UploadFileModelDto,
  UploadFileModelDtoCollisionBehavior,
} from '../../models/upload-file-model-dto.model';

describe('StockDataEntryApiService', () => {
  let service: StockDataEntryApiService;
  let httpMock: HttpTestingController;

  const stockDataEntry: StockDataEntryDto = {
    id: '11111111-1111-1111-1111-111111111111',
    stockSymbolId: '22222222-2222-2222-2222-222222222222',
    date: '2026-07-06T00:00:00',
    amount: 123.45,
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });

    service = TestBed.inject(StockDataEntryApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get a stock data entry by id', () => {
    service.getById(stockDataEntry.id as string).subscribe((result) => {
      expect(result).toEqual(stockDataEntry);
    });

    const req = httpMock.expectOne(`/api/StockDataEntry/${stockDataEntry.id}`);
    expect(req.request.method).toBe('GET');
    expect(req.request.withCredentials).toBeTrue();
    req.flush(stockDataEntry);
  });

  it('should get a filtered page of stock data entries', () => {
    const paged: PagedData<StockDataEntryDto> = {
      record: [stockDataEntry],
      page: 0,
      pageSize: 20,
      recordCount: 1,
      totalPages: 1,
    };

    service
      .getPaged({
        page: 0,
        pageSize: 20,
        stockSymbolId: stockDataEntry.stockSymbolId,
        startDate: '2026-01-01T00:00:00.000Z',
        endDate: '2026-12-31T00:00:00.000Z',
      })
      .subscribe((result) => {
        expect(result).toEqual(paged);
      });

    const req = httpMock.expectOne(
      (request) =>
        request.url === '/api/StockDataEntry' &&
        request.params.get('page') === '1' &&
        request.params.get('pageSize') === '20' &&
        request.params.get('stockSymbolId') === stockDataEntry.stockSymbolId &&
        request.params.get('startDate') === '2026-01-01T00:00:00.000Z' &&
        request.params.get('endDate') === '2026-12-31T00:00:00.000Z',
    );
    expect(req.request.method).toBe('GET');
    expect(req.request.withCredentials).toBeTrue();
    req.flush(paged);
  });

  it('should upload a stock data file', () => {
    const file = new File(['date,amount'], 'stock.csv', { type: 'text/csv' });
    const model: UploadFileModelDto = {
      skipHeader: true,
      dateColumnIndex: 0,
      valueColumnIndex: 1,
      collisionBehavior: UploadFileModelDtoCollisionBehavior.SkipExisting,
    };

    service
      .uploadFile(stockDataEntry.stockSymbolId, file, model)
      .subscribe((result) => {
        expect(result).toBeNull();
      });

    const req = httpMock.expectOne(
      `/api/StockDataEntry/${stockDataEntry.stockSymbolId}`,
    );
    expect(req.request.method).toBe('POST');
    expect(req.request.body instanceof FormData).toBeTrue();
    expect(req.request.body.get('file')).toBe(file);
    expect(req.request.body.get('strModel')).toBe(JSON.stringify(model));
    expect(req.request.withCredentials).toBeTrue();
    req.flush(null);
  });
});
