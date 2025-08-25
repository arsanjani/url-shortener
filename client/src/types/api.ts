export interface ShortLinkRequestDto {
  title?: string;
  token?: string;
  originLink: string;
  isPublish: boolean;
}

export interface ShortLinkDetailDto {
  id: number;
  shortLinkID: number;
  visitDate: string;
  country?: string;
  os?: string;
  browser?: string;
}

export interface ShortLinkResponseDto {
  id: number;
  title?: string;
  token: string;
  originLink: string;
  isPublish: boolean;
  createAdminDate: string;
  editAdminDate?: string;
  clickCount: number;
  recentClicks: ShortLinkDetailDto[];
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}
