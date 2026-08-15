import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { StockSymbolApiService } from './stock-symbol-api.service';
import { StockSymbolDto } from '../../models/stock-symbol-dto.model';
import { PagedData } from '../../models/paged-data.model';

describe('StockSymbolApiService', () => {
  let service: StockSymbolApiService;
  let httpMock: HttpTestingController;

  const stockSymbol: StockSymbolDto = {
    id: '11111111-1111-1111-1111-111111111111',
    symbol: 'MSFT',
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });

    service = TestBed.inject(StockSymbolApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get a stock symbol by id', () => {
    service.getById(stockSymbol.id as string).subscribe((result) => {
      expect(result).toEqual(stockSymbol);
    });

    const req = httpMock.expectOne(`/api/StockSymbol/${stockSymbol.id}`);
    expect(req.request.method).toBe('GET');
    expect(req.request.withCredentials).toBeTrue();
    req.flush(stockSymbol);
  });

  it('should get a page of stock symbols', () => {
    const paged: PagedData<StockSymbolDto> = {
      record: [stockSymbol],
      page: 0,
      pageSize: 10,
      recordCount: 1,
      totalPages: 1,
    };

    service.getPaged(0, 10).subscribe((result) => {
      expect(result).toEqual(paged);
    });

    const req = httpMock.expectOne(
      (request) =>
        request.url === '/api/StockSymbol' &&
        request.params.get('page') === '1' &&
        request.params.get('pageSize') === '10',
    );
    expect(req.request.method).toBe('GET');
    expect(req.request.withCredentials).toBeTrue();
    req.flush(paged);
  });

  it('should create a stock symbol', () => {
    service.create(stockSymbol).subscribe((result) => {
      expect(result).toEqual(stockSymbol);
    });

    const req = httpMock.expectOne('/api/StockSymbol');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(stockSymbol);
    expect(req.request.withCredentials).toBeTrue();
    req.flush(stockSymbol);
  });

  it('should update a stock symbol', () => {
    service
      .update(stockSymbol.id as string, stockSymbol)
      .subscribe((result) => {
        expect(result).toEqual(stockSymbol);
      });

    const req = httpMock.expectOne(`/api/StockSymbol/${stockSymbol.id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(stockSymbol);
    expect(req.request.withCredentials).toBeTrue();
    req.flush(stockSymbol);
  });

  it('should delete a stock symbol', () => {
    service.delete(stockSymbol.id as string).subscribe((result) => {
      expect(result).toBeNull();
    });

    const req = httpMock.expectOne(`/api/StockSymbol/${stockSymbol.id}`);
    expect(req.request.method).toBe('DELETE');
    expect(req.request.withCredentials).toBeTrue();
    req.flush(null);
  });
});
