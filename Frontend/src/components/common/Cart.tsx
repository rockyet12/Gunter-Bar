import React, { useState, useEffect, useRef } from 'react';
import { useCart } from './CartContext';
import { Button } from '../buttons';

const Cart: React.FC = () => {
  const { items, removeItem, updateQuantity, total, itemCount, clearCart } = useCart();
  const [isExpanded, setIsExpanded] = useState(false);
  const [position, setPosition] = useState({ x: 0, y: 0 });
  const [isDragging, setIsDragging] = useState(false);
  const [dragOffset, setDragOffset] = useState({ x: 0, y: 0 });
  const iconRef = useRef<HTMLDivElement>(null);

  // Inicializar posición en el centro
  useEffect(() => {
    const savedPosition = localStorage.getItem('cartPosition');
    if (savedPosition) {
      setPosition(JSON.parse(savedPosition));
    } else {
      // Centro de la pantalla
      setPosition({
        x: window.innerWidth / 2 - 30, // 30 es la mitad del ancho del icono
        y: window.innerHeight / 2 - 30  // 30 es la mitad del alto del icono
      });
    }
  }, []);

  const handleMouseDown = (e: React.MouseEvent) => {
    if (iconRef.current) {
      const rect = iconRef.current.getBoundingClientRect();
      setDragOffset({
        x: e.clientX - rect.left,
        y: e.clientY - rect.top
      });
      setIsDragging(true);
    }
  };

  const handleMouseMove = (e: MouseEvent) => {
    if (isDragging) {
      const newX = e.clientX - dragOffset.x;
      const newY = e.clientY - dragOffset.y;

      // Limitar el movimiento dentro de la ventana
      const maxX = window.innerWidth - 60;
      const maxY = window.innerHeight - 60;

      const clampedX = Math.max(0, Math.min(newX, maxX));
      const clampedY = Math.max(0, Math.min(newY, maxY));

      setPosition({ x: clampedX, y: clampedY });
    }
  };

  const handleMouseUp = () => {
    if (isDragging) {
      setIsDragging(false);
      localStorage.setItem('cartPosition', JSON.stringify(position));
    }
  };

  // Agregar event listeners globales para el drag
  useEffect(() => {
    if (isDragging) {
      document.addEventListener('mousemove', handleMouseMove);
      document.addEventListener('mouseup', handleMouseUp);
    }

    return () => {
      document.removeEventListener('mousemove', handleMouseMove);
      document.removeEventListener('mouseup', handleMouseUp);
    };
  }, [isDragging, dragOffset, position]);

  const handleCheckout = () => {
    alert(`Pedido realizado por un total de ${formatPrice(total)}. ¡Gracias por tu compra!`);
    clearCart();
    setIsExpanded(false);
  };

  if (items.length === 0) {
    return null; // Don't show cart if empty
  }

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(price);
  };

  return (
    <>
      {/* Cart Icon */}
      <div
        ref={iconRef}
        className={`cart-icon ${isDragging ? 'dragging' : ''}`}
        style={{
          left: `${position.x}px`,
          top: `${position.y}px`,
          cursor: isDragging ? 'grabbing' : 'grab'
        }}
        onMouseDown={handleMouseDown}
        onClick={() => {
          if (!isDragging) {
            setIsExpanded(!isExpanded);
          }
        }}
      >
        <span className="cart-icon-emoji">🛒</span>
        {itemCount > 0 && <span className="cart-count">{itemCount}</span>}
      </div>

      {/* Expanded Cart */}
      {isExpanded && (
        <div className="cart-expanded">
          <div className="cart-overlay" onClick={() => setIsExpanded(false)}></div>
          <div className="cart-content">
            <div className="cart-header">
              <h3>Carrito ({itemCount})</h3>
              <button className="close-btn" onClick={() => setIsExpanded(false)}>✕</button>
            </div>
            <div className="cart-items">
              {items.map(item => (
                <div key={item.id} className="cart-item">
              {item.image && <img src={item.image} alt={item.name} className="cart-item-image" />}
              <div className="cart-item-info">
                    <h4>{item.name}</h4>
                    <p>{formatPrice(item.price)}</p>
                    <div className="cart-item-controls">
                      <button
                        onClick={() => updateQuantity(item.id, item.quantity - 1)}
                        className="quantity-btn"
                      >
                        -
                      </button>
                      <span>{item.quantity}</span>
                      <button
                        onClick={() => updateQuantity(item.id, item.quantity + 1)}
                        className="quantity-btn"
                      >
                        +
                      </button>
                      <button
                        onClick={() => removeItem(item.id)}
                        className="remove-btn"
                      >
                        🗑️
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
            <div className="cart-footer">
              <div className="cart-total">
                <strong>Total: {formatPrice(total)}</strong>
              </div>
              <Button variant="primary" size="lg" className="checkout-btn" onClick={handleCheckout}>
                Comprar Ahora
              </Button>
            </div>
          </div>
        </div>
      )}
    </>
  );
};

export default Cart;
