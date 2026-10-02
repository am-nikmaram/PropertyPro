export interface FacilityListItem {
  id: number;
  name: string;
  code: string;
  city: string | null;
  country: string | null;
  isActive: boolean;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface FacilityQuery {
  pageNumber: number;
  pageSize: number;
  search: string;
  sortBy: string;
  sortDescending: boolean;
}
