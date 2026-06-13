export interface ApiError {
  message?: string;
  details?: any;
  statusCode?: number;
}

export type ApiErrorResponse = ApiError;
