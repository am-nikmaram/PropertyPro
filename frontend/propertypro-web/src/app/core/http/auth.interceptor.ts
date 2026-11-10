import { HttpErrorResponse, HttpEventType, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { HttpContextToken, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError, filter, finalize, map, shareReplay, switchMap, throwError } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import { AuthResponse } from '../../features/auth/models/auth.models';
import { AuthSessionService } from '../auth/auth-session.service';

const SESSION_RETRIED = new HttpContextToken<boolean>(() => false);
let refreshRequest$: Observable<AuthResponse> | null = null;

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const session = inject(AuthSessionService);
  const router = inject(Router);
  const user = session.currentUser();

  if (!request.url.startsWith(`${API_BASE_URL}/`) || !user?.token) {
    return next(request);
  }

  return next(withBearer(request, user.token)).pipe(
    catchError((error: unknown) => {
      if (!(error instanceof HttpErrorResponse) || error.status !== 401 || request.context.get(SESSION_RETRIED)) {
        return throwError(() => error);
      }

      return refreshSession(next, session).pipe(
        switchMap((response) => next(withBearer(request, response.token, true))),
        catchError((refreshError: unknown) => {
          session.clear();
          void router.navigateByUrl('/login');
          return throwError(() => refreshError);
        }),
      );
    }),
  );
};

function withBearer(request: HttpRequest<unknown>, token: string, retried = false): HttpRequest<unknown> {
  return request.clone({
    setHeaders: { Authorization: `Bearer ${token}` },
    context: retried ? request.context.set(SESSION_RETRIED, true) : request.context,
  });
}

function refreshSession(
  next: Parameters<HttpInterceptorFn>[1],
  session: AuthSessionService,
): Observable<AuthResponse> {
  if (!refreshRequest$) {
    const refreshToken = session.currentUser()?.refreshToken;
    if (!refreshToken) return throwError(() => new Error('No refresh token is available.'));

    const request = new HttpRequest('POST', `${API_BASE_URL}/Auth/refresh`, { refreshToken });
    refreshRequest$ = next(request).pipe(
      filter((event): event is HttpResponse<AuthResponse> => event.type === HttpEventType.Response),
      map((response) => {
        if (!response.body) throw new Error('The refresh endpoint returned an empty response.');
        return response.body;
      }),
      map((response) => {
        session.save(response);
        return response;
      }),
      finalize(() => { refreshRequest$ = null; }),
      shareReplay({ bufferSize: 1, refCount: false }),
    );
  }

  return refreshRequest$;
}
