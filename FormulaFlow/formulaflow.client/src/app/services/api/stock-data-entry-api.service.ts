import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagedData } from '../../models/paged-data.model';
import { StockDataEntryDto } from '../../models/stock-data-entry-dto.model';
import { UploadFileModelDto } from '../../models/upload-file-model-dto.model';

@Injectable({
  providedIn: 'root',
})
export class StockDataEntryApiService {
  private readonly apiUrl = '/api/StockDataEntry';

  public constructor(private readonly http: HttpClient) {}

  public getById(id: string): Observable<StockDataEntryDto> {
    return this.http.get<StockDataEntryDto>(`${this.apiUrl}/${id}`, {
      withCredentials: true,
    });
  }

  public getPaged(options: StockDataEntryPagedOptions): Observable<PagedData<StockDataEntryDto>> {
    let params = new HttpParams()
      .set('page', options.page)
      .set('pageSize', options.pageSize);

    if (options.stockSymbolId) {
      params = params.set('stockSymbolId', options.stockSymbolId);
    }

    if (options.startDate) {
      params = params.set('startDate', this.toDateQueryValue(options.startDate));
    }

    if (options.endDate) {
      params = params.set('endDate', this.toDateQueryValue(options.endDate));
    }

    return this.http.get<PagedData<StockDataEntryDto>>(this.apiUrl, {
      params,
      withCredentials: true,
    });
  }

  public uploadFile(
    stockSymbolId: string,
    file: File,
    model: UploadFileModelDto,
  ): Observable<void> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('strModel', JSON.stringify(model));

    return this.http.post<void>(`${this.apiUrl}/${stockSymbolId}`, formData, {
      withCredentials: true,
    });
  }

  private toDateQueryValue(value: Date | string): string {
    return value instanceof Date ? value.toISOString() : value;
  }
}

export interface StockDataEntryPagedOptions {
  page: number;
  pageSize: number;
  stockSymbolId?: string;
  startDate?: Date | string;
  endDate?: Date | string;
}
