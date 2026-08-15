import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SessionDto } from '../../models/session-dto.model';
import { LoginDto } from '../../models/login-dto.model';
import { RegisterDto } from '../../models/register-dto.model';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationApiService {
  private readonly apiUrl = '/api/Authentication';

  public constructor(private readonly http: HttpClient) {}

  public register(dto: RegisterDto): Observable<SessionDto> {
    return this.http.post<SessionDto>(`${this.apiUrl}/register`, dto, {
      withCredentials: true,
    });
  }

  public login(dto: LoginDto): Observable<SessionDto> {
    return this.http.post<SessionDto>(`${this.apiUrl}/login`, dto, {
      withCredentials: true,
    });
  }

  public logout(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/logout`, null, {
      withCredentials: true,
    });
  }
}
