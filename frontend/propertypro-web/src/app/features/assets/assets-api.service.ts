import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/config/api.config';

export interface AssetItem { id: number; assetNumber: string; name: string; assetCategoryCode: string; assetCategoryName: string; roomNumber: string; roomName: string; criticality: number; status: number; purchaseCost: number | null; isActive: boolean; }
export interface AssetDetail extends AssetItem { assetCategoryId:number; roomId:number; manufacturer:string|null; model:string|null; serialNumber:string|null; installationDate:string|null; warrantyStartDate:string|null; warrantyEndDate:string|null; description:string|null; }
export interface AssetWriteRequest { assetNumber?:string; name:string; assetCategoryId:number; roomId:number; manufacturer:string|null; model:string|null; serialNumber:string|null; installationDate:string|null; warrantyStartDate:string|null; warrantyEndDate:string|null; purchaseCost:number|null; criticality:number; status:number; description:string|null; }
export interface AssetCategory { id: number; code: string; name: string; }
export interface BuildingOption { id: number; facilityId: number; code: string; name: string; }
export interface RoomOption { id: number; unitId: number; roomTypeId: number; buildingId:number; facilityId:number; roomNumber: string; name: string; }

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
  getAsset(id:number):Observable<AssetDetail>{return this.http.get<AssetDetail>(`${API_BASE_URL}/Assets/${id}`)}
  createAsset(request:AssetWriteRequest):Observable<AssetDetail>{return this.http.post<AssetDetail>(`${API_BASE_URL}/Assets`,request)}
  updateAsset(id:number,request:AssetWriteRequest):Observable<AssetDetail>{const {assetNumber: _assetNumber,...body}=request;return this.http.put<AssetDetail>(`${API_BASE_URL}/Assets/${id}`,{...body,id})}
  deleteAsset(id:number):Observable<void>{return this.http.delete<void>(`${API_BASE_URL}/Assets/${id}`)}
  getCategories(): Observable<AssetCategory[]> { return this.http.get<AssetCategory[]>(`${API_BASE_URL}/AssetCategories/lookup`); }
  getBuildings(): Observable<BuildingOption[]> { return this.http.get<BuildingOption[]>(`${API_BASE_URL}/Buildings/lookup`); }
  getRooms(buildingId: number | null): Observable<RoomOption[]> {
    let params = new HttpParams(); if (buildingId) params = params.set('buildingId', buildingId);
    return this.http.get<RoomOption[]>(`${API_BASE_URL}/Rooms/lookup`, { params });
  }
}
