export interface PagedData<T> {
  record: T[];
  page: number;
  pageSize: number;
  recordCount: number;
  totalPages: number;
}
