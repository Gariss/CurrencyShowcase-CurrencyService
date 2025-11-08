import React from 'react';

const CurrencyItem = ({ currency, isFavorite, onToggleFavorite }) => {
  const handleToggle = () => {
    onToggleFavorite(currency.id, isFavorite);
  };

  return (
    <div className="currency-item">
      <div className="currency-col favorite-col">
        <input
          type="checkbox"
          checked={isFavorite}
          onChange={handleToggle}
          className="favorite-checkbox"
        />
      </div>
      <div className="currency-col code-col">
        <strong>{currency.charCode}</strong>
      </div>
      <div className="currency-col name-col">
        {currency.name}
      </div>
      <div className="currency-col rate-col">
        {typeof currency.rate === 'number' ? currency.rate.toFixed(4) : 'N/A'}
      </div>
    </div>
  );
};

export default CurrencyItem;