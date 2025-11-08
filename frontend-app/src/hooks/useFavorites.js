import { useState, useEffect } from 'react';
import { favoritesService } from '../services/favoritesService';

export const useFavorites = (token) => {
  const [favorites, setFavorites] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [saving, setSaving] = useState(false);

  const loadFavorites = async () => {
    if (!token) return;
    
    try {
      setLoading(true);
      setError(null);
      const data = await favoritesService.getFavorites(token);
      setFavorites(data || []);
    } catch (err) {
      setError(err.message || 'Failed to load favorites');
      console.error('Error loading favorites:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadFavorites();
  }, [token]);

  const addToFavorites = async (currencyIds) => {
    if (!token || !currencyIds.length) return;
    
    try {
      setSaving(true);
      await favoritesService.addFavorites(currencyIds, token);
      await loadFavorites();
    } catch (err) {
      setError(err.message || 'Failed to add favorites');
      throw err;
    } finally {
      setSaving(false);
    }
  };

  const removeFromFavorites = async (currencyIds) => {
    if (!token || !currencyIds.length) return;
    
    try {
      setSaving(true);
      await favoritesService.removeFavorites(currencyIds, token);
      await loadFavorites();
    } catch (err) {
      setError(err.message || 'Failed to remove favorites');
      throw err;
    } finally {
      setSaving(false);
    }
  };

  const toggleFavorite = async (currencyId, isCurrentlyFavorite) => {
    if (isCurrentlyFavorite) {
      await removeFromFavorites([currencyId]);
    } else {
      await addToFavorites([currencyId]);
    }
  };

  const isFavorite = (currencyId) => {
    return favorites.some(fav => fav.id === currencyId);
  };

  return {
    favorites,
    loading,
    error,
    saving,
    addToFavorites,
    removeFromFavorites,
    toggleFavorite,
    isFavorite,
    reloadFavorites: loadFavorites
  };
};