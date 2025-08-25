import axios from 'axios';
import { ShortLinkRequestDto, ShortLinkResponseDto } from '../types/api';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';
const SHORT_URL_BASE = process.env.REACT_APP_SHORT_URL_BASE || 'http://localhost:5000';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add request interceptor for error handling
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('API Error:', error);
    return Promise.reject(error);
  }
);

export class ApiService {
  // Get all short links
  static async getAllShortLinks(): Promise<ShortLinkResponseDto[]> {
    const response = await apiClient.get<ShortLinkResponseDto[]>('/admin/shortlinks');
    return response.data;
  }

  // Get a specific short link by ID
  static async getShortLink(id: number): Promise<ShortLinkResponseDto> {
    const response = await apiClient.get<ShortLinkResponseDto>(`/admin/shortlinks/${id}`);
    return response.data;
  }

  // Create a new short link
  static async createShortLink(shortLink: ShortLinkRequestDto): Promise<ShortLinkResponseDto> {
    const response = await apiClient.post<ShortLinkResponseDto>('/admin/shortlinks', shortLink);
    return response.data;
  }

  // Update an existing short link
  static async updateShortLink(id: number, shortLink: ShortLinkRequestDto): Promise<ShortLinkResponseDto> {
    const response = await apiClient.put<ShortLinkResponseDto>(`/admin/shortlinks/${id}`, shortLink);
    return response.data;
  }

  // Delete a short link
  static async deleteShortLink(id: number): Promise<void> {
    await apiClient.delete(`/admin/shortlinks/${id}`);
  }

  // Toggle publish status
  static async togglePublishStatus(id: number): Promise<ShortLinkResponseDto> {
    const response = await apiClient.post<ShortLinkResponseDto>(`/admin/shortlinks/${id}/toggle-publish`);
    return response.data;
  }

  // Get the backend domain for testing links
  static getBackendDomain(): string {
    return SHORT_URL_BASE;
  }

  // Build the full short URL for testing
  static buildShortUrl(token: string): string {
    return `${this.getBackendDomain()}/${token}`;
  }
}
