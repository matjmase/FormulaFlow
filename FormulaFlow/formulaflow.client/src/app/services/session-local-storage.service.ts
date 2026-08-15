import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SessionDto } from '../models/session-dto.model';

@Injectable({
  providedIn: 'root',
})
export class SessionLocalStorageService {
  private sessionVariableName = 'session';

  public sessionChanged = new BehaviorSubject<SessionDto | null>(
    this.getSession(),
  );

  public setSession(session: SessionDto): void {
    localStorage.setItem(this.sessionVariableName, JSON.stringify(session));
    this.sessionChanged.next(session);
  }

  public getSession(): SessionDto | null {
    const sessionJson = localStorage.getItem(this.sessionVariableName);
    if (sessionJson) {
      return JSON.parse(sessionJson) as SessionDto;
    }
    return null;
  }

  public clearSession(): void {
    localStorage.removeItem(this.sessionVariableName);
    this.sessionChanged.next(null);
  }
}
