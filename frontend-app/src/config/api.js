const API_CONFIG = {
  // Backend API settings
  BASE_URL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:5092',
  TIMEOUT: parseInt(import.meta.env.VITE_API_TIMEOUT) || 10000,
  
  // Frontend settings
  FRONTEND_URL: import.meta.env.VITE_FRONTEND_URL || 'http://localhost:5173',
  
  ENDPOINTS: {
    REGISTER: '/api/users',
    LOGIN: '/api/users/login',
    LOGOUT: '/api/users/logout',
    PROFILE: '/api/users',
    CURRENCIES: '/api/currencies',
    CURRENCY_BY_CODE: '/api/currencies/{charCode}',
    REFRESH_CURRENCIES: '/api/currencies/refresh',
    FAVORITES: '/api/favorites'
  }
};

export default API_CONFIG;