import { useState, useEffect } from 'react';
import { currencyService } from '../services/currencyService';

export const useCurrencies = (token) => {
  const [currencies, setCurrencies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const loadCurrencies = async () => {
    if (!token) return;
    
    try {
      setLoading(true);
      setError(null);
      const data = await currencyService.getAllCurrencies(token);
      setCurrencies(data || []);
    } catch (err) {
      setError(err.message || 'Failed to load currencies');
      console.error('Error loading currencies:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCurrencies();
  }, [token]);

  const refreshCurrencies = async () => {
    try {
      setLoading(true);
      await currencyService.refreshCurrencies(token);
      await loadCurrencies();
    } catch (err) {
      setError(err.message || 'Failed to refresh currencies');
    } finally {
      setLoading(false);
    }
  };

  return {
    currencies,
    loading,
    error,
    refreshCurrencies,
    reloadCurrencies: loadCurrencies
  };
};