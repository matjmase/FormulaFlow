import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { HttpErrorResponse, HttpInterceptorFn, HttpRequest, HttpResponse } from '@angular/common/http';
import { firstValueFrom, of, throwError } from 'rxjs';
import { SessionLocalStorageService } from '../services/session-local-storage.service';

import { authenticationInterceptor } from './authentication.interceptor';

describe('authenticationInterceptor', () => {
  const clearSession = vi.fn();
  const navigate = vi.fn().mockResolvedValue(true);
  const interceptor: HttpInterceptorFn = (req, next) => 
    TestBed.runInInjectionContext(() => authenticationInterceptor(req, next));

  beforeEach(() => {
    clearSession.mockReset();
    navigate.mockClear();
    TestBed.configureTestingModule({
      providers: [
        { provide: SessionLocalStorageService, useValue: { clearSession } },
        { provide: Router, useValue: { navigate } },
      ],
    });
  });

  it('should be created', () => {
    expect(interceptor).toBeTruthy();
  });

  for (const status of [403, 401, 500]) {
    it(`should ${status === 401 ? '' : 'not '}clear the session and redirect for HTTP ${status} and preserve the error`, async () => {
      const error = new HttpErrorResponse({ status });
      const result = interceptor(new HttpRequest('GET', '/api/test'), () =>
        throwError(() => error),
      );

      await expect(firstValueFrom(result)).rejects.toBe(error);
      expect(clearSession).toHaveBeenCalledTimes(status === 401 ? 1 : 0);
      expect(navigate).toHaveBeenCalledTimes(status === 401 ? 1 : 0);
      if (status === 401) {
        expect(navigate).toHaveBeenCalledWith(['/']);
      }
    });
  }

  it('should pass successful responses through without clearing the session', async () => {
    const response = new HttpResponse({ status: 200 });
    const result = interceptor(new HttpRequest('GET', '/api/test'), () => of(response));

    await expect(firstValueFrom(result)).resolves.toBe(response);
    expect(clearSession).not.toHaveBeenCalled();
    expect(navigate).not.toHaveBeenCalled();
  });
});
