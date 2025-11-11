import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../components/forms/AuthContext';
import { Button } from '../components/buttons';
import SearchBar from '../components/common/SearchBar';
import '../components/common/SearchBar.css';
import './MainLayout.css';

interface MainLayoutProps {
  children: React.ReactNode;
}

const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const closeMenu = () => {
    setIsMenuOpen(false);
  };

  const handleSearch = (query: string, category: string, priceRange: string) => {
    // Navigate to menu with search params
    navigate(`/menu?search=${encodeURIComponent(query)}&category=${encodeURIComponent(category)}&price=${encodeURIComponent(priceRange)}`);
  };

  return (
    <div className="main-layout">
      <header className="header">
        <div className="header-left">
          {/* Hamburger Menu Button */}
          <button className="hamburger-menu" onClick={toggleMenu} aria-label="Toggle menu">
            <span className={`hamburger-line ${isMenuOpen ? 'open' : ''}`}></span>
            <span className={`hamburger-line ${isMenuOpen ? 'open' : ''}`}></span>
            <span className={`hamburger-line ${isMenuOpen ? 'open' : ''}`}></span>
          </button>
        </div>

        <div className="header-content">
          <div className="logo">
            <img src="/logo.jpeg" alt="Bar Gunter Logo" className="logo-icon" />
            <h1>Bar Gunter</h1>
          </div>
          <SearchBar onSearch={handleSearch} />
        </div>

        <div className="header-right">
          {/* User Profile on the right */}
          {isAuthenticated && user ? (
            <button onClick={() => navigate('/profile')} className="user-profile-link">
              <div className="user-profile">
                <img
                  src={user.profileImageUrl || "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=40&h=40&fit=crop&crop=face"}
                  alt="Foto de perfil"
                  className="profile-image"
                />
              </div>
            </button>
          ) : (
            <div className="text-white">No autenticado</div>
          )}
        </div>

        {/* Mobile Menu Overlay */}
        <div className={`menu-overlay ${isMenuOpen ? 'show' : ''}`} onClick={closeMenu}></div>

        {/* Navigation Menu */}
        <nav className={`nav-menu ${isMenuOpen ? 'open' : ''}`}>
          <ul>
            <li><Link to="/" onClick={closeMenu}>Inicio</Link></li>
            {isAuthenticated && <li><Link to="/menu" onClick={closeMenu}>Productos</Link></li>}
            {isAuthenticated && user?.role === 'Seller' && <li><Link to="/seller" onClick={closeMenu}>Panel de Vendedor</Link></li>}
            {isAuthenticated && <li><Link to="/profile" onClick={closeMenu}>Mi Perfil</Link></li>}
            {isAuthenticated ? (
              <li>
                <Button variant="outline" size="sm" onClick={() => { logout(); closeMenu(); }}>
                  Cerrar Sesión
                </Button>
              </li>
            ) : (
              <>
                <li><Link to="/login" onClick={closeMenu}>Iniciar Sesión</Link></li>
                <li><Link to="/register" onClick={closeMenu}>Registrarse</Link></li>
              </>
            )}
          </ul>
        </nav>
      </header>
      <main className="body">
        {children}
      </main>
      <footer className="footer">
        <div className="footer-content">
          <div className="footer-section">
            <h3>Bar Gunter</h3>
            <p>Tu lugar favorito para disfrutar de los mejores cócteles y bebidas premium en un ambiente único.</p>
          </div>
          <div className="footer-section">
            <h4>Enlaces Rápidos</h4>
            <ul>
              <li><Link to="/">Inicio</Link></li>
              <li><Link to="/menu">Productos</Link></li>
              {isAuthenticated && user?.role === 'Seller' && <li><Link to="/seller">Panel de Vendedor</Link></li>}
              <li><Link to="/profile">Mi Perfil</Link></li>
              <li><Link to="/contact">Contacto</Link></li>
            </ul>
          </div>
          <div className="footer-section">
            <h4>Contacto</h4>
            <p>📍 Calle Principal 123, Ciudad</p>
            <p>📞 +57 300 123 4567</p>
            <p>✉️ junior.rivaset12d1@gmail.com</p>
            <p>🕒 Lun-Dom: 5PM - 2AM</p>
          </div>
          <div className="footer-section">
            <h4>Síguenos</h4>
            <div className="social-links">
              <a href="https://github.com/rockyet12" target="_blank" rel="noopener noreferrer" aria-label="GitHub">� GitHub</a>
              <a href="https://instagram.com/roque.jr._.05" target="_blank" rel="noopener noreferrer" aria-label="Instagram">📷 Instagram</a>
              <a href="#" aria-label="Twitter">🐦 Twitter</a>
              <a href="#" aria-label="TikTok">🎵 TikTok</a>
            </div>
          </div>
          <div className="footer-section">
            <h4>Legal</h4>
            <ul>
              <li><Link to="/privacy">Política de Privacidad</Link></li>
              <li><Link to="/terms">Términos de Servicio</Link></li>
              <li><Link to="/cookies">Política de Cookies</Link></li>
            </ul>
          </div>
        </div>
        <div className="footer-bottom">
          <p>&copy; 2025 Gunter Bar. Todos los derechos reservados. Diseñado con ❤️ para amantes de las bebidas.</p>
        </div>
      </footer>
    </div>
  );
};

export default MainLayout;
