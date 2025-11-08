import React from 'react';
import { useAuthContext } from '../context/AuthContext';
import { useCurrencies } from '../hooks/useCurrencies';
import { useFavorites } from '../hooks/useFavorites';
import CurrencyList from '../components/CurrencyList';
import './CurrencyPage.css';

const CurrencyPage = () => {
  const { token, user } = useAuthContext();
  const { currencies, loading: currenciesLoading, error: currenciesError, refreshCurrencies } = useCurrencies(token);
  const { favorites, loading: favoritesLoading, error: favoritesError, toggleFavorite } = useFavorites(token);

  const handleRefresh = async () => {
    await refreshCurrencies();
  };

  const loading = currenciesLoading || favoritesLoading;
  const error = currenciesError || favoritesError;

  return (
    <div className="currency-page">
      <div className="page-header">
        <div className="user-welcome">
          <h2>Welcome back, {user?.name || user?.login}!</h2>
          {user?.email && <p>{user.email}</p>}
          {user?.registrationDate && (
            <p>Member since: {new Date(user.registrationDate).toLocaleDateString()}</p>
          )}
        </div>
        
        <button 
          onClick={handleRefresh} 
          className="btn refresh-btn"
          disabled={loading}
        >
          Refresh Rates
        </button>
      </div>

      {error && (
        <div className="error-message">
          {error}
        </div>
      )}

      <div className="content-section">
        <h3>Currency List</h3>
        <p className="section-description">
          Select your favorite currencies by checking the boxes below.
          Your selections will be saved automatically.
        </p>
        
        <CurrencyList
          currencies={currencies}
          favorites={favorites}
          onToggleFavorite={toggleFavorite}
          loading={loading}
        />
      </div>

      <div className="favorites-summary">
        <h4>Your Favorites ({favorites.length})</h4>
        {favorites.length > 0 ? (
          <div className="favorites-tags">
            {favorites.map(fav => (
              <span key={fav.id} className="favorite-tag">
                {fav.charCode} - {fav.name}
              </span>
            ))}
          </div>
        ) : (
          <p>No favorite currencies selected yet.</p>
        )}
      </div>
    </div>
  );
};

export default CurrencyPage;