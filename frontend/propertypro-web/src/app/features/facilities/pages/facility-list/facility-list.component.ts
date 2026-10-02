import { HttpErrorResponse } from '@angular/common/http';
import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { toObservable } from '@angular/core/rxjs-interop';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import type { TableLazyLoadEvent } from 'primeng/types/table';
import { FacilitiesApiService } from '../../data-access/facilities-api.service';
import { FacilityListItem } from '../../models/facility.models';

@Component({
  selector: 'app-facility-list',
  imports: [ButtonModule, InputTextModule, TableModule, TagModule],
  templateUrl: './facility-list.component.html',
  styleUrl: './facility-list.component.scss',
})
export class FacilityListComponent {
  private readonly api = inject(FacilitiesApiService);
  private readonly destroyRef = inject(DestroyRef);

  readonly facilities = signal<FacilityListItem[]>([]);
  readonly totalRecords = signal(0);
  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);
  readonly firstRow = signal(0);
  readonly pageSize = signal(10);
  readonly searchTerm = signal('');

  constructor() {
    toObservable(this.searchTerm)
      .pipe(skip(1), debounceTime(350), distinctUntilChanged(), takeUntilDestroyed(this.destroyRef))
      .subscribe((search) => {
        this.firstRow.set(0);
        this.load(1, this.pageSize(), search.trim(), 'name', false);
      });
  }

  onSearch(event: Event): void {
    this.searchTerm.set((event.target as HTMLInputElement).value);
  }

  onLazyLoad(event: TableLazyLoadEvent): void {
    const rows = event.rows ?? this.pageSize();
    const first = event.first ?? 0;
    const sortField = typeof event.sortField === 'string' ? event.sortField : 'name';
    this.firstRow.set(first);
    this.pageSize.set(rows);
    this.load(
      Math.floor(first / rows) + 1,
      rows,
      this.searchTerm().trim(),
      sortField,
      event.sortOrder === -1,
    );
  }

  retry(): void {
    this.load(
      Math.floor(this.firstRow() / this.pageSize()) + 1,
      this.pageSize(),
      this.searchTerm().trim(),
      'name',
      false,
    );
  }

  private load(pageNumber: number, pageSize: number, search: string, sortBy: string, sortDescending: boolean): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.api.getFacilities({ pageNumber, pageSize, search, sortBy, sortDescending }).subscribe({
      next: (result) => {
        this.facilities.set([...result.items]);
        this.totalRecords.set(result.totalCount);
        this.isLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading.set(false);
        this.errorMessage.set(
          error.status === 0
            ? 'The API could not be reached. Check that PropertyPro.Api is running.'
            : error.error?.detail ?? 'Facilities could not be loaded.',
        );
      },
    });
  }
}
