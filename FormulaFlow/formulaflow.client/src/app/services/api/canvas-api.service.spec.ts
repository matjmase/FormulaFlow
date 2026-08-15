import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { CanvasApiService } from './canvas-api.service';
import { PagedData } from '../../models/paged-data.model';
import { StockCanvasSimpleDto } from '../../models/stock-canvas-simple-dto.model';

describe('CanvasApiService', () => {
  let service: CanvasApiService;
  let httpMock: HttpTestingController;

  const canvas: StockCanvasSimpleDto = {
    id: '11111111-1111-1111-1111-111111111111',
    name: 'Main canvas',
    scale: 1,
    height: 1080,
    width: 1920,
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(CanvasApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get a canvas by id', () => {
    service.getById(canvas.id as string).subscribe((result) => {
      expect(result).toEqual(canvas);
    });

    const req = httpMock.expectOne(`/api/Canvas/${canvas.id}`);
    expect(req.request.method).toBe('GET');
    expect(req.request.withCredentials).toBeTrue();
    req.flush(canvas);
  });

  it('should get a page of canvases', () => {
    const paged: PagedData<StockCanvasSimpleDto> = {
      record: [canvas],
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
        request.url === '/api/Canvas' &&
        request.params.get('page') === '1' &&
        request.params.get('pageSize') === '10',
    );
    expect(req.request.method).toBe('GET');
    expect(req.request.withCredentials).toBeTrue();
    req.flush(paged);
  });

  it('should delete a canvas', () => {
    service.delete(canvas).subscribe((result) => {
      expect(result).toBeNull();
    });

    const req = httpMock.expectOne('/api/Canvas');
    expect(req.request.method).toBe('DELETE');
    expect(req.request.body).toEqual(canvas);
    expect(req.request.withCredentials).toBeTrue();
    req.flush(null);
  });
});
