import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { AssetsApiService, AssetCategory, AssetItem, BuildingOption, RoomOption } from '../../assets-api.service';
import { AuthSessionService } from '../../../../core/auth/auth-session.service';

@Component({ selector: 'app-asset-list', imports: [FormsModule, RouterLink, ButtonModule, InputTextModule, SelectModule, TableModule], templateUrl: './asset-list.component.html', styleUrl: './asset-list.component.scss' })
export class AssetListComponent {
  private readonly api = inject(AssetsApiService);
  private readonly session = inject(AuthSessionService);
  readonly assets = signal<AssetItem[]>([]); readonly categories = signal<AssetCategory[]>([]); readonly buildings = signal<BuildingOption[]>([]); readonly rooms = signal<RoomOption[]>([]);
  readonly search = signal(''); readonly categoryId = signal<number | null>(null); readonly buildingId = signal<number | null>(null); readonly roomId = signal<number | null>(null);
  readonly loading = signal(false); readonly error = signal<string | null>(null);
  canManage(): boolean { return this.session.hasAnyRole('Admin', 'MaintenanceManager'); }
  constructor() {
    this.api.getCategories().subscribe({ next: x => this.categories.set(x), error: e => this.fail(e) });
    this.api.getBuildings().subscribe({ next: x => this.buildings.set(x), error: e => this.fail(e) });
    this.load();
  }
  changeBuilding(id: number | null): void { this.buildingId.set(id); this.roomId.set(null); this.api.getRooms(id).subscribe({ next: x => this.rooms.set(x), error: e => this.fail(e) }); this.load(); }
  load(): void { this.loading.set(true); this.error.set(null); this.api.getAssets(this.search(), this.categoryId(), this.roomId()).subscribe({ next: x => { this.assets.set(x); this.loading.set(false); }, error: e => { this.loading.set(false); this.fail(e); } }); }
  statusName(status: number): string { return ({ 1: 'Active', 2: 'In maintenance', 3: 'Under repair', 4: 'Retired', 5: 'Disposed', 6: 'Lost' } as Record<number, string>)[status] ?? `Status ${status}`; }
  criticalityName(level: number): string { return ({ 1: 'Low', 2: 'Medium', 3: 'High', 4: 'Critical' } as Record<number, string>)[level] ?? `Level ${level}`; }
  private fail(error: HttpErrorResponse): void { this.error.set(error.status === 0 ? 'The API could not be reached.' : error.error?.detail ?? 'Assets could not be loaded.'); }
}
