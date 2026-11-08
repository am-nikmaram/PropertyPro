import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/config/api.config';

export interface InventoryFacility { id: number; code: string; name: string; }
export interface BuildingItem { id: number; facilityId: number; name: string; code: string; yearBuilt: number | null; totalArea: number | null; isActive: boolean; }
export interface Building extends BuildingItem { description: string | null; }
export interface BuildingWriteRequest { facilityId: number; name: string; code: string; description: string | null; yearBuilt: number | null; totalArea: number | null; }
export interface Page<T> { items: T[]; pageNumber: number; pageSize: number; totalCount: number; totalPages: number; }

@Injectable({ providedIn: 'root' })
export class BuildingsApiService {
  private readonly http = inject(HttpClient);
  getFacilities(): Observable<InventoryFacility[]> { return this.http.get<InventoryFacility[]>(`${API_BASE_URL}/Facilities/lookup`); }
  getBuildings(facilityId: number, pageNumber = 1, pageSize = 10, search = ''): Observable<Page<BuildingItem>> {
    const params = new HttpParams().set('facilityId', facilityId).set('pageNumber', pageNumber).set('pageSize', pageSize).set('search', search);
    return this.http.get<Page<BuildingItem>>(`${API_BASE_URL}/Buildings`, { params });
  }
  getBuilding(id: number): Observable<Building> { return this.http.get<Building>(`${API_BASE_URL}/Buildings/${id}`); }
  createBuilding(request: BuildingWriteRequest): Observable<Building> { return this.http.post<Building>(`${API_BASE_URL}/Buildings`, request); }
  updateBuilding(id: number, request: BuildingWriteRequest): Observable<Building> { return this.http.put<Building>(`${API_BASE_URL}/Buildings/${id}`, { ...request, id }); }
  deleteBuilding(id: number): Observable<void> { return this.http.delete<void>(`${API_BASE_URL}/Buildings/${id}`); }
}
