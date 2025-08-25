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

export interface StatisticsResponseDto {
  shortLink: ShortLinkResponseDto;
  totalClicks: number;
  uniqueCountries: number;
  dailyStats: Array<{ date: string; clicks: number }>;
  countryStats: Array<{ country: string; clicks: number }>;
  osStats: Array<{ os: string; clicks: number }>;
  browserStats: Array<{ browser: string; clicks: number }>;
  hourlyStats: Array<{ hour: number; clicks: number }>;
  firstClick?: string;
  lastClick?: string;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}
