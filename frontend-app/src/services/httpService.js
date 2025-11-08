import API_CONFIG from '../config/api';

class HttpService {
  constructor() {
    this.baseURL = API_CONFIG.BASE_URL;
    this.timeout = API_CONFIG.TIMEOUT;
  }

  async request(url, options = {}) {
    const headers = {
      'Content-Type': 'application/json',
      ...options.headers,
    };
  
    const config = {
      ...options,
      headers,
    };
  
    try {
      const response = await fetch(`${this.baseURL}${url}`, config);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
  
      const contentType = response.headers.get('content-type');
      if (contentType && contentType.includes('application/json')) {
        return await response.json();
      }
      
      return null;
    } catch (error) {
      console.error('API request failed:', error);
      throw error;
    }
  }

  async get(url, token = null) {
    const headers = token ? { 'Authorization': `Bearer ${token}` } : {};
    return this.request(url, { method: 'GET', headers });
  }

  async post(url, data = null, token = null) {
    const headers = token ? { 'Authorization': `Bearer ${token}` } : {};
    const body = data ? JSON.stringify(data) : undefined;
    
    return this.request(url, { 
      method: 'POST', 
      headers, 
      body 
    });
  }

  async delete(url, data = null, token = null) {
    const headers = token ? { 'Authorization': `Bearer ${token}` } : {};
    const body = data ? JSON.stringify(data) : undefined;
    
    return this.request(url, { 
      method: 'DELETE', 
      headers, 
      body 
    });
  }
}

export const httpService = new HttpService();