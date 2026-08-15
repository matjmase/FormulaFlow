import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BackTestApiService {
  private readonly apiUrl = '/api/BackTest';

  public constructor(private readonly http: HttpClient) {}

  public run(
    id: string,
    start: Date | string,
    end: Date | string,
  ): Observable<Blob> {
    const params = new HttpParams()
      .set('start', this.toDateQueryValue(start))
      .set('end', this.toDateQueryValue(end));

    return this.http.post(`${this.apiUrl}/${id}`, null, {
      params,
      responseType: 'blob',
      withCredentials: true,
    });
  }

  private toDateQueryValue(value: Date | string): string {
    return value instanceof Date ? value.toISOString() : value;
  }
}
