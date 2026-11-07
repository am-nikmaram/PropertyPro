import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/config/api.config';

export interface AssetItem { id: number; assetNumber: string; name: string; assetCategoryCode: string; assetCategoryName: string; roomNumber: string; roomName: string; criticality: number; status: number; purchaseCost: number | null; isActive: boolean; }
export interface AssetCategory { id: number; code: string; name: string; }
export interface BuildingOption { id: number; facilityId: number; code: string; name: string; }
export interface RoomOption { id: number; unitId: number; roomTypeId: number; roomNumber: string; name: string; }

@Injectable({ providedIn: 'root' })
export class AssetsApiService {
  private readonly http = inject(HttpClient);
  getAssets(name: string, assetCategoryId: number | null, roomId: number | null): Observable<AssetItem[]> {
    let params = new HttpParams().set('isActive', true);
    if (name.trim()) params = params.set('name', name.trim());
    if (assetCategoryId) params = params.set('assetCategoryId', assetCategoryId);
    if (roomId) params = params.set('roomId', roomId);
    return this.http.get<AssetItem[]>(`${API_BASE_URL}/Assets`, { params });
  }
  getCategories(): Observable<AssetCategory[]> { return this.http.get<AssetCategory[]>(`${API_BASE_URL}/AssetCategories/lookup`); }
  getBuildings(): Observable<BuildingOption[]> { return this.http.get<BuildingOption[]>(`${API_BASE_URL}/Buildings/lookup`); }
  getRooms(buildingId: number | null): Observable<RoomOption[]> {
    let params = new HttpParams(); if (buildingId) params = params.set('buildingId', buildingId);
    return this.http.get<RoomOption[]>(`${API_BASE_URL}/Rooms/lookup`, { params });
  }
}
