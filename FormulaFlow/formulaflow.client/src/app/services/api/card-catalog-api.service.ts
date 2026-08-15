import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StockCardDto } from '../../models/stock-card-dto.model';

@Injectable({
  providedIn: 'root',
})
export class CardCatalogApiService {
  private readonly apiUrl = '/api/CardCatalog';

  public constructor(private readonly http: HttpClient) {}

  public get(): Observable<StockCardDto[]> {
    return this.http.get<StockCardDto[]>(this.apiUrl, {
      withCredentials: true,
    });
  }
}
