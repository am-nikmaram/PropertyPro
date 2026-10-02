import { Injectable, signal } from '@angular/core';
import { AuthResponse } from '../../features/auth/models/auth.models';

const AUTH_SESSION_KEY = 'propertypro.auth';

@Injectable({ providedIn: 'root' })
export class AuthSessionService {
  readonly currentUser = signal<AuthResponse | null>(this.readSession());

  save(response: AuthResponse): void {
    sessionStorage.setItem(AUTH_SESSION_KEY, JSON.stringify(response));
    this.currentUser.set(response);
  }

  clear(): void {
    sessionStorage.removeItem(AUTH_SESSION_KEY);
    this.currentUser.set(null);
  }

  hasValidSession(): boolean {
    const user = this.currentUser();
    const expiresAt = user ? Date.parse(user.expiresAt) : Number.NaN;
    if (!user || !Number.isFinite(expiresAt) || expiresAt <= Date.now()) {
      this.clear();
      return false;
    }

    return true;
  }

  private readSession(): AuthResponse | null {
    try {
      const stored = sessionStorage.getItem(AUTH_SESSION_KEY);
      return stored ? (JSON.parse(stored) as AuthResponse) : null;
    } catch {
      return null;
    }
  }
}
