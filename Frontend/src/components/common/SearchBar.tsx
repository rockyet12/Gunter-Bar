import React, { useState } from 'react';
import './SearchBar.css';

interface SearchBarProps {
  onSearch: (query: string, category: string, priceRange: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ onSearch }) => {
  const [query, setQuery] = useState('');
  const [category, setCategory] = useState('');
  const [priceRange, setPriceRange] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSearch(query, category, priceRange);
  };

  const handleClear = () => {
    setQuery('');
    setCategory('');
    setPriceRange('');
  };

  const categories = [
    'Todos',
    'Cócteles',
    'Cervezas',
    'Vinos',
    'Whiskies',
    'Rones'
  ];

  const priceRanges = [
    'Todos',
    'Hasta $10.000',
    '$10.000 - $25.000',
    '$25.000 - $50.000',
    'Más de $50.000'
  ];

  return (
    <form className="search-bar" onSubmit={handleSubmit}>
      <div className="search-input-group">
        <input
          type="text"
          placeholder="Buscar productos..."
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          className="search-input"
        />
        <button type="submit" className="search-button">
          🔍
        </button>
        <button type="button" className="clear-button" onClick={handleClear}>
          ✕
        </button>
      </div>

      <div className="filters-group">
        <select
          value={category}
          onChange={(e) => setCategory(e.target.value)}
          className="filter-select"
        >
          {categories.map(cat => (
            <option key={cat} value={cat === 'Todos' ? '' : cat}>{cat}</option>
          ))}
        </select>

        <select
          value={priceRange}
          onChange={(e) => setPriceRange(e.target.value)}
          className="filter-select"
        >
          {priceRanges.map(range => (
            <option key={range} value={range === 'Todos' ? '' : range}>{range}</option>
          ))}
        </select>
      </div>
    </form>
  );
};

export default SearchBar;
