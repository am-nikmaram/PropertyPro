import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthSessionService } from './auth-session.service';

export const authGuard: CanActivateFn = () => {
  const session = inject(AuthSessionService);

  return session.hasValidSession() || inject(Router).createUrlTree(['/login']);
};
