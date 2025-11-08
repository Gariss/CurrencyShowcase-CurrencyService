import { httpService } from './httpService';
import API_CONFIG from '../config/api';

class FavoritesService {
  async getFavorites(token) {
    return httpService.get(API_CONFIG.ENDPOINTS.FAVORITES, token);
  }

  async addFavorites(currencyIds, token) {
    return httpService.post(API_CONFIG.ENDPOINTS.FAVORITES, currencyIds, token);
  }

  async removeFavorites(currencyIds, token) {
    return httpService.delete(API_CONFIG.ENDPOINTS.FAVORITES, currencyIds, token);
  }
}

export const favoritesService = new FavoritesService();