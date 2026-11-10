import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthSessionService } from './auth-session.service';

export const adminGuard: CanActivateFn = () =>
  inject(AuthSessionService).hasAnyRole('Admin') || inject(Router).createUrlTree(['/app/facilities']);
