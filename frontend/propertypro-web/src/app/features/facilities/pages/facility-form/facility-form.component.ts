import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FacilitiesApiService } from '../../data-access/facilities-api.service';
import { Facility, FacilityWriteRequest } from '../../models/facility.models';

@Component({ selector: 'app-facility-form', imports: [FormsModule, RouterLink, ButtonModule, InputTextModule], templateUrl: './facility-form.component.html', styleUrl: './facility-form.component.scss' })
export class FacilityFormComponent {
  private readonly api = inject(FacilitiesApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  readonly id = Number(this.route.snapshot.paramMap.get('id')) || null;
  readonly loading = signal(!!this.id);
  readonly saving = signal(false);
  readonly error = signal<string | null>(null);
  form: FacilityWriteRequest = { name: '', code: '', address: null, city: null, country: null, postalCode: null };

  constructor() {
    if (this.id) this.api.getFacility(this.id).subscribe({ next: facility => { this.form = this.toRequest(facility); this.loading.set(false); }, error: error => { this.loading.set(false); this.showError(error, 'Facility could not be loaded.'); } });
  }

  save(): void {
    this.saving.set(true); this.error.set(null);
    const request: FacilityWriteRequest = {
      name: this.form.name.trim(), code: this.form.code.trim(),
      address: this.clean(this.form.address), city: this.clean(this.form.city),
      country: this.clean(this.form.country), postalCode: this.clean(this.form.postalCode),
    };
    const action = this.id ? this.api.updateFacility(this.id, request) : this.api.createFacility(request);
    action.subscribe({ next: facility => { this.saving.set(false); void this.router.navigate(['/app/facilities', facility.id]); }, error: error => { this.saving.set(false); this.showError(error, 'Facility could not be saved.'); } });
  }

  private toRequest(facility: Facility): FacilityWriteRequest {
    return { name: facility.name, code: facility.code, address: facility.address, city: facility.city, country: facility.country, postalCode: facility.postalCode };
  }
  private clean(value: string | null): string | null { const cleaned = value?.trim(); return cleaned || null; }
  private showError(error: HttpErrorResponse, fallback: string): void { this.error.set(error.status === 0 ? 'The API could not be reached.' : error.error?.detail ?? fallback); }
}
