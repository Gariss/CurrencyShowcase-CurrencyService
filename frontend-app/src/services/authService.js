import { httpService } from './httpService';
import API_CONFIG from '../config/api';

class AuthService {
  async register(userData) {
    return httpService.post(API_CONFIG.ENDPOINTS.REGISTER, userData);
  }

  async login(credentials) {
    const url = `${API_CONFIG.BASE_URL}${API_CONFIG.ENDPOINTS.LOGIN}`;
    
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(credentials),
    });

    if (!response.ok) {
      throw new Error(`Login failed: ${response.status}`);
    }

    const authHeader = response.headers.get('authorization') || response.headers.get('Authorization');
    
    if (authHeader && authHeader.startsWith('Bearer ')) {
      const token = authHeader.substring(7);
      return { token };
    }

    const allHeaders = {};
    response.headers.forEach((value, key) => {
      allHeaders[key] = value;
    });
    
    throw new Error('Token not found in response headers. Available headers: ' + JSON.stringify(allHeaders));
  }

  async logout(token) {
    return httpService.post(API_CONFIG.ENDPOINTS.LOGOUT, null, token);
  }

  async getProfile(token, login) {
    const url = login 
      ? `${API_CONFIG.ENDPOINTS.PROFILE}?login=${encodeURIComponent(login)}`
      : API_CONFIG.ENDPOINTS.PROFILE;
    
    return httpService.get(url, token);
  }
}

export const authService = new AuthService();