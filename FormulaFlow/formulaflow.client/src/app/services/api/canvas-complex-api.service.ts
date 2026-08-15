import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StockCanvasDto } from '../../models/stock-canvas-dto.model';

@Injectable({
  providedIn: 'root',
})
export class CanvasComplexApiService {
  private readonly apiUrl = '/api/CanvasComplex';

  public constructor(private readonly http: HttpClient) {}

  public create(dto: StockCanvasDto): Observable<StockCanvasDto> {
    return this.http.post<StockCanvasDto>(this.apiUrl, dto, {
      withCredentials: true,
    });
  }

  public getById(id: string): Observable<StockCanvasDto> {
    return this.http.get<StockCanvasDto>(`${this.apiUrl}/${id}`, {
      withCredentials: true,
    });
  }

  public update(id: string, dto: StockCanvasDto): Observable<StockCanvasDto> {
    return this.http.put<StockCanvasDto>(`${this.apiUrl}/${id}`, dto, {
      withCredentials: true,
    });
  }
}
