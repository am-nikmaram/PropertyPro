import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/config/api.config';
import { Facility, FacilityQuery, FacilityListItem, FacilityWriteRequest, PagedResult } from '../models/facility.models';

@Injectable({ providedIn: 'root' })
export class FacilitiesApiService {
  private readonly http = inject(HttpClient);

  getFacilities(query: FacilityQuery): Observable<PagedResult<FacilityListItem>> {
    const params = new HttpParams()
      .set('pageNumber', query.pageNumber)
      .set('pageSize', query.pageSize)
      .set('search', query.search)
      .set('sortBy', query.sortBy)
      .set('sortDescending', query.sortDescending);

    return this.http.get<PagedResult<FacilityListItem>>(`${API_BASE_URL}/Facilities`, { params });
  }

  getFacility(id: number): Observable<Facility> {
    return this.http.get<Facility>(`${API_BASE_URL}/Facilities/${id}`);
  }

  createFacility(request: FacilityWriteRequest): Observable<Facility> {
    return this.http.post<Facility>(`${API_BASE_URL}/Facilities`, request);
  }

  updateFacility(id: number, request: FacilityWriteRequest): Observable<Facility> {
    return this.http.put<Facility>(`${API_BASE_URL}/Facilities/${id}`, { ...request, id });
  }

  deleteFacility(id: number): Observable<void> {
    return this.http.delete<void>(`${API_BASE_URL}/Facilities/${id}`);
  }
}
