import React, { useState } from 'react';
import { Outlet, Link } from 'react-router-dom';
import { useAuth } from '../components/forms/AuthContext';
import { Button } from '../components/buttons';
import './MainLayout.css';

const PublicLayout: React.FC = () => {
  const { isAuthenticated, user, logout } = useAuth();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const closeMenu = () => {
    setIsMenuOpen(false);
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
          <h1>Bar Gunter</h1>
        </div>

        <div className="header-right">
          {/* User Profile on the right */}
          {isAuthenticated && user && (
            <div className="user-profile">
              <img 
                src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=40&h=40&fit=crop&crop=face" 
                alt="Foto de perfil" 
                className="profile-image"
              />
              <span className="user-name">{user.name}</span>
            </div>
          )}
        </div>

        {/* Mobile Menu Overlay */}
        <div className={`menu-overlay ${isMenuOpen ? 'show' : ''}`} onClick={closeMenu}></div>

        {/* Navigation Menu */}
        <nav className={`nav-menu ${isMenuOpen ? 'open' : ''}`}>
          <ul>
            <li><Link to="/" onClick={closeMenu}>Inicio</Link></li>
            <li><Link to="/menu" onClick={closeMenu}>Menú</Link></li>
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
        <Outlet />
      </main>
      <footer className="footer">
        <p>&copy; 2025 Gunter Bar. Todos los derechos reservados.</p>
      </footer>
    </div>
  );
};

export default PublicLayout;
