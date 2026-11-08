import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { FacilitiesApiService } from '../../data-access/facilities-api.service';
import { Facility } from '../../models/facility.models';
import { AuthSessionService } from '../../../../core/auth/auth-session.service';

@Component({ selector: 'app-facility-detail', imports: [RouterLink, ButtonModule, TagModule], templateUrl: './facility-detail.component.html', styleUrl: './facility-detail.component.scss' })
export class FacilityDetailComponent {
  private readonly api = inject(FacilitiesApiService);
  private readonly router = inject(Router);
  private readonly session = inject(AuthSessionService);
  private readonly id = Number(inject(ActivatedRoute).snapshot.paramMap.get('id'));
  readonly facility = signal<Facility | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  canManage(): boolean { return this.session.hasAnyRole('Admin', 'FacilityManager'); }
  constructor() { this.load(); }
  load(): void { this.loading.set(true); this.api.getFacility(this.id).subscribe({ next: item => { this.facility.set(item); this.loading.set(false); }, error: error => { this.error.set(error.status === 0 ? 'The API could not be reached.' : error.error?.detail ?? 'Facility could not be loaded.'); this.loading.set(false); } }); }
  deactivate(): void { if (!this.facility() || !window.confirm(`Deactivate ${this.facility()!.name}?`)) return; this.api.deleteFacility(this.id).subscribe({ next: () => void this.router.navigateByUrl('/app/facilities'), error: (error: HttpErrorResponse) => this.error.set(error.error?.detail ?? 'Facility could not be deactivated.') }); }
}
