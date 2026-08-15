import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedData } from '../../models/paged-data.model';
import { StockSymbolDto } from '../../models/stock-symbol-dto.model';

@Injectable({
  providedIn: 'root',
})
export class StockSymbolApiService {
  private readonly apiUrl = '/api/StockSymbol';

  public constructor(private readonly http: HttpClient) {}

  public getById(id: string): Observable<StockSymbolDto> {
    return this.http.get<StockSymbolDto>(`${this.apiUrl}/${id}`, {
      withCredentials: true,
    });
  }

  public getPaged(
    page: number,
    pageSize: number,
  ): Observable<PagedData<StockSymbolDto>> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<PagedData<StockSymbolDto>>(this.apiUrl, {
      params,
      withCredentials: true,
    });
  }

  public create(dto: StockSymbolDto): Observable<StockSymbolDto> {
    return this.http.post<StockSymbolDto>(this.apiUrl, dto, {
      withCredentials: true,
    });
  }

  public update(id: string, dto: StockSymbolDto): Observable<StockSymbolDto> {
    return this.http.put<StockSymbolDto>(`${this.apiUrl}/${id}`, dto, {
      withCredentials: true,
    });
  }

  public delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      withCredentials: true,
    });
  }
}
