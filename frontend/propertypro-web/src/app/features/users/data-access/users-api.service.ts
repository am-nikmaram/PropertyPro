import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/config/api.config';

export interface ManagedUser {
  id: string; email: string; firstName: string; lastName: string;
  emailConfirmed: boolean; lockoutEnabled: boolean; isLockedOut: boolean;
  organizationId: number | null; roles: string[];
}
export interface OrganizationOption { id: number; name: string; code: string; }

@Injectable({ providedIn: 'root' })
export class UsersApiService {
  private readonly http = inject(HttpClient);
  getUsers(): Observable<ManagedUser[]> { return this.http.get<ManagedUser[]>(`${API_BASE_URL}/Users`); }
  getOrganizations(): Observable<OrganizationOption[]> { return this.http.get<OrganizationOption[]>(`${API_BASE_URL}/Users/organizations`); }
  assignRole(userId: string, role: string): Observable<void> { return this.http.put<void>(`${API_BASE_URL}/Users/${encodeURIComponent(userId)}/roles`, { role }); }
  removeRole(userId: string, role: string): Observable<void> { return this.http.delete<void>(`${API_BASE_URL}/Users/${encodeURIComponent(userId)}/roles/${encodeURIComponent(role)}`); }
  assignOrganization(userId: string, organizationId: number): Observable<void> { return this.http.put<void>(`${API_BASE_URL}/Users/${encodeURIComponent(userId)}/organization`, { organizationId }); }
}
