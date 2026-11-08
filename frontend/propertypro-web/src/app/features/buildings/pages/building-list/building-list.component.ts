import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { BuildingsApiService, BuildingItem, InventoryFacility } from '../../buildings-api.service';
import { AuthSessionService } from '../../../../core/auth/auth-session.service';

@Component({ selector: 'app-building-list', imports: [FormsModule, RouterLink, ButtonModule, InputTextModule, SelectModule, TableModule], templateUrl: './building-list.component.html', styleUrl: './building-list.component.scss' })
export class BuildingListComponent {
  private readonly api = inject(BuildingsApiService);
  private readonly session = inject(AuthSessionService);
  readonly facilities = signal<InventoryFacility[]>([]);
  readonly facilityId = signal<number | null>(null);
  readonly buildings = signal<BuildingItem[]>([]);
  readonly search = signal('');
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly selectedLabel = () => this.facilities().find(x => x.id === this.facilityId())?.name ?? 'Select a facility';
  canManage(): boolean { return this.session.hasAnyRole('Admin', 'FacilityManager'); }
  constructor() { this.api.getFacilities().subscribe({ next: items => { this.facilities.set(items); if (items.length) { this.facilityId.set(items[0].id); this.load(); } }, error: e => this.fail(e) }); }
  load(): void { const id = this.facilityId(); if (!id) { this.buildings.set([]); return; } this.loading.set(true); this.error.set(null); this.api.getBuildings(id, 1, 100, this.search().trim()).subscribe({ next: page => { this.buildings.set(page.items); this.loading.set(false); }, error: e => { this.loading.set(false); this.fail(e); } }); }
  private fail(error: HttpErrorResponse): void { this.error.set(error.status === 0 ? 'The API could not be reached.' : error.error?.detail ?? 'Buildings could not be loaded.'); }
}
