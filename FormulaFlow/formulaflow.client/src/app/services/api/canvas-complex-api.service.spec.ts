import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { CanvasComplexApiService } from './canvas-complex-api.service';
import { StockCanvasDto } from '../../models/stock-canvas-dto.model';

describe('CanvasComplexApiService', () => {
  let service: CanvasComplexApiService;
  let httpMock: HttpTestingController;

  const canvas: StockCanvasDto = {
    id: '11111111-1111-1111-1111-111111111111',
    name: 'Main canvas',
    scale: 1,
    height: 1080,
    width: 1920,
    cards: [],
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(CanvasComplexApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create a complex canvas', () => {
    service.create(canvas).subscribe((result) => {
      expect(result).toEqual(canvas);
    });

    const req = httpMock.expectOne('/api/CanvasComplex');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(canvas);
    expect(req.request.withCredentials).toBeTrue();
    req.flush(canvas);
  });

  it('should get a complex canvas by id', () => {
    service.getById(canvas.id as string).subscribe((result) => {
      expect(result).toEqual(canvas);
    });

    const req = httpMock.expectOne(`/api/CanvasComplex/${canvas.id}`);
    expect(req.request.method).toBe('GET');
    expect(req.request.withCredentials).toBeTrue();
    req.flush(canvas);
  });

  it('should update a complex canvas', () => {
    service.update(canvas.id as string, canvas).subscribe((result) => {
      expect(result).toEqual(canvas);
    });

    const req = httpMock.expectOne(`/api/CanvasComplex/${canvas.id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(canvas);
    expect(req.request.withCredentials).toBeTrue();
    req.flush(canvas);
  });
});
