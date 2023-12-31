export interface Pagination {
  currentPage: number;
  itemPerPage: number;
  totalPages: number;
  totalItems: number;
}

export class PaginatedResult<T> {
    result?: T;
    pagination?: Pagination;
}
