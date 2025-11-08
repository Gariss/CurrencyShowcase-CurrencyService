import API_CONFIG from './api.js';

const APP_CONFIG = {
  // App metadata
  APP_NAME: 'Currency Manager',
  APP_VERSION: '1.0.0',
  
  // URLs
  FRONTEND_URL: API_CONFIG.FRONTEND_URL,
  API_URL: API_CONFIG.BASE_URL,
  
  // Features
  FEATURES: {
    AUTH_ENABLED: true,
    FAVORITES_ENABLED: true,
    CURRENCY_REFRESH_ENABLED: true
  },
  
  // UI Settings
  UI: {
    DEFAULT_THEME: 'light',
    CURRENCY_DECIMAL_PLACES: 4,
    ITEMS_PER_PAGE: 50
  },
  
  // Storage keys
  STORAGE_KEYS: {
    AUTH_TOKEN: 'authToken',
    USER_PREFERENCES: 'userPreferences'
  }
};

export default APP_CONFIG;