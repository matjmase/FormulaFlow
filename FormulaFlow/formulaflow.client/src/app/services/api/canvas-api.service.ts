import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedData } from '../../models/paged-data.model';
import { StockCanvasSimpleDto } from '../../models/stock-canvas-simple-dto.model';

@Injectable({
  providedIn: 'root',
})
export class CanvasApiService {
  private readonly apiUrl = '/api/Canvas';

  public constructor(private readonly http: HttpClient) {}

  public getById(id: string): Observable<StockCanvasSimpleDto> {
    return this.http.get<StockCanvasSimpleDto>(`${this.apiUrl}/${id}`, {
      withCredentials: true,
    });
  }

  public getPaged(
    page: number,
    pageSize: number,
  ): Observable<PagedData<StockCanvasSimpleDto>> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<PagedData<StockCanvasSimpleDto>>(this.apiUrl, {
      params,
      withCredentials: true,
    });
  }

  public delete(dto: StockCanvasSimpleDto): Observable<void> {
    return this.http.delete<void>(this.apiUrl, {
      body: dto,
      withCredentials: true,
    });
  }
}
