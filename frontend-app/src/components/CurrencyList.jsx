import React from 'react';
import CurrencyItem from './CurrencyItem';
import './CurrencyList.css';

const CurrencyList = ({ 
  currencies, 
  favorites, 
  onToggleFavorite, 
  loading 
}) => {
  if (loading) {
    return <div className="loading">Loading currencies...</div>;
  }

  if (!currencies.length) {
    return <div className="no-data">No currencies available</div>;
  }

  return (
    <div className="currency-list">
      <div className="currency-header">
        <div className="currency-col favorite-col">Favorite</div>
        <div className="currency-col code-col">Code</div>
        <div className="currency-col name-col">Name</div>
        <div className="currency-col rate-col">Rate</div>
      </div>
      
      {currencies.map(currency => (
        <CurrencyItem
          key={currency.id || currency.charCode}
          currency={currency}
          isFavorite={favorites.some(fav => fav.id === currency.id)}
          onToggleFavorite={onToggleFavorite}
        />
      ))}
    </div>
  );
};

export default CurrencyList;