import { httpService } from './httpService';
import API_CONFIG from '../config/api';

class CurrencyService {
  async getAllCurrencies(token) {
    return httpService.get(API_CONFIG.ENDPOINTS.CURRENCIES, token);
  }

  async getCurrencyByCode(charCode, token) {
    const url = API_CONFIG.ENDPOINTS.CURRENCY_BY_CODE.replace('{charCode}', charCode);
    return httpService.get(url, token);
  }

  async refreshCurrencies(token) {
    return httpService.post(API_CONFIG.ENDPOINTS.REFRESH_CURRENCIES, null, token);
  }
}

export const currencyService = new CurrencyService();