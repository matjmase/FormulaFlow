import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SessionLocalStorageService } from '../services/session-local-storage.service';

export const authenticationInterceptor: HttpInterceptorFn = (req, next) => {
  const sessionStorage = inject(SessionLocalStorageService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        sessionStorage.clearSession();
        void router.navigate(['/']);
      }

      return throwError(() => error);
    }),
  );
};
