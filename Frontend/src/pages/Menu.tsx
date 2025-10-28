import React, { useState, useEffect } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { useAuth } from '../components/forms/AuthContext';
import { useCart } from '../components/common/CartContext';
import { Button } from '../components/buttons';
import Cart from '../components/common/Cart';
import ProductReviewModal from '../components/common/ProductReviewModal';
import './pages.css';

interface Drink {
  id: number;
  name: string;
  description: string;
  price: number;
  category: string;
  image?: string;
  rating: number;
  featured?: boolean;
}

const Menu: React.FC = () => {
  const { isAuthenticated } = useAuth();
  const { addItem } = useCart();
  const [searchParams] = useSearchParams();
  const [selectedCategory, setSelectedCategory] = useState<string>('Todos');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [priceFilter, setPriceFilter] = useState<string>('');

  // Estado para el modal de reseñas
  const [reviewModalOpen, setReviewModalOpen] = React.useState(false);
  const [selectedProduct, setSelectedProduct] = React.useState<Drink | null>(null);

  useEffect(() => {
    const search = searchParams.get('search') || '';
    const category = searchParams.get('category') || 'Todos';
    const price = searchParams.get('price') || '';

    setSearchQuery(search);
    setSelectedCategory(category);
    setPriceFilter(price);
  }, [searchParams]);

  const drinks: Drink[] = [
    // Cervezas
    {
      id: 1,
      name: "Cerveza Clásica",
      description: "Cerveza lager refrescante con un final crujiente",
      price: 5500,
      category: "Cerveza",
      image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop&crop=center",
      rating: 4.2
    },
    {
      id: 2,
      name: "IPA Artesanal",
      description: "Cerveza artesanal con lúpulo y notas cítricas intensas",
      price: 7500,
      category: "Cerveza",
      image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop&crop=center",
      rating: 4.7,
      featured: true
    },
    {
      id: 3,
      name: "Stout Oscura",
      description: "Cerveza negra cremosa con notas de chocolate y café",
      price: 6800,
      category: "Cerveza",
      image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop&crop=center",
      rating: 4.5
    },
    {
      id: 4,
      name: "Weissbier",
      description: "Cerveza de trigo alemana, ligera y refrescante",
      price: 6200,
      category: "Cerveza",
      image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop&crop=center",
      rating: 4.3
    },

    // Vinos
    {
      id: 5,
      name: "Vino Tinto Cabernet",
      description: "Vino tinto robusto con taninos suaves y aromas a frutas rojas",
      price: 18000,
      category: "Vino",
      image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop&crop=center",
      rating: 4.6,
      featured: true
    },
    {
      id: 6,
      name: "Vino Blanco Sauvignon Blanc",
      description: "Vino blanco fresco con aromas cítricos y notas herbales",
      price: 22000,
      category: "Vino",
      image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop&crop=center",
      rating: 4.4
    },
    {
      id: 7,
      name: "Vino Rosado",
      description: "Vino rosado seco, perfecto para días soleados",
      price: 16000,
      category: "Vino",
      image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop&crop=center",
      rating: 4.2
    },
    {
      id: 8,
      name: "Vino Espumoso Brut",
      description: "Champagne estilo con burbujas finas y fresco",
      price: 35000,
      category: "Vino",
      image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop&crop=center",
      rating: 4.8,
      featured: true
    },

    // Cócteles
    {
      id: 9,
      name: "Mojito Clásico",
      description: "Refrescante cóctel con ron, menta fresca, azúcar y soda",
      price: 8500,
      category: "Cóctel",
      image: "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=300&h=300&fit=crop&crop=center",
      rating: 4.8,
      featured: true
    },
    {
      id: 10,
      name: "Margarita Frozen",
      description: "Cóctel refrescante con tequila, triple sec y jugo de limón",
      price: 9500,
      category: "Cóctel",
      image: "https://images.unsplash.com/photo-1544145945-f90425340c7e?w=300&h=300&fit=crop&crop=center",
      rating: 4.5
    },
    {
      id: 11,
      name: "Whiskey Sour",
      description: "Cóctel suave de whiskey con limón y azúcar",
      price: 9200,
      category: "Cóctel",
      image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop&crop=center",
      rating: 4.6
    },
    {
      id: 12,
      name: "Piña Colada",
      description: "Cóctel tropical con ron, piña y coco cremoso",
      price: 8800,
      category: "Cóctel",
      image: "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=300&h=300&fit=crop&crop=center",
      rating: 4.4
    },
    {
      id: 13,
      name: "Cosmopolitan",
      description: "Cóctel elegante con vodka, triple sec, jugo de arándano y lima",
      price: 9800,
      category: "Cóctel",
      image: "https://images.unsplash.com/photo-1544145945-f90425340c7e?w=300&h=300&fit=crop&crop=center",
      rating: 4.3
    },

    // Whiskies
    {
      id: 14,
      name: "Whisky Escocés 12 años",
      description: "Whisky escocés añejado con notas de vainilla, roble y miel",
      price: 45000,
      category: "Whisky",
      image: "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=300&h=300&fit=crop&crop=center",
      rating: 4.9,
      featured: true
    },
    {
      id: 15,
      name: "Whisky Irlandés",
      description: "Whisky irlandés suave y triple destilado",
      price: 38000,
      category: "Whisky",
      image: "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=300&h=300&fit=crop&crop=center",
      rating: 4.6
    },
    {
      id: 16,
      name: "Whisky Bourbon",
      description: "Bourbon americano con notas dulces de vainilla y caramelo",
      price: 42000,
      category: "Whisky",
      image: "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=300&h=300&fit=crop&crop=center",
      rating: 4.7
    },

    // Otros
    {
      id: 17,
      name: "Agua Mineral",
      description: "Agua mineral natural con gas",
      price: 2500,
      category: "Sin Alcohol",
      image: "https://images.unsplash.com/photo-1544145945-f90425340c7e?w=300&h=300&fit=crop&crop=center",
      rating: 4.0
    },
    {
      id: 18,
      name: "Jugo Natural de Naranja",
      description: "Jugo de naranja exprimido fresco",
      price: 4000,
      category: "Sin Alcohol",
      image: "https://images.unsplash.com/photo-1600271886742-f049cd451bba?w=300&h=300&fit=crop&crop=center",
      rating: 4.1
    }
  ];

  const addToCart = (drink: Drink) => {
    if (!isAuthenticated) {
      // Redirect to login if not authenticated
      window.location.href = '/login';
      return;
    }
    addItem({
      id: drink.id,
      name: drink.name,
      price: drink.price,
      image: drink.image || ''
    });
  };

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(price);
  };

  const handleProductClick = (product: Drink) => {
    setSelectedProduct(product);
    setReviewModalOpen(true);
  };

  const handleReviewSubmit = (productId: number, rating: number, comment: string, images: File[]) => {
    // Aquí puedes enviar la reseña al backend
    console.log('Reseña enviada:', { productId, rating, comment, images });
    alert(`¡Gracias por tu reseña! Calificación: ${rating} estrellas${comment ? '\nComentario: ' + comment : ''}${images.length > 0 ? '\nFotos: ' + images.length : ''}`);
  };

  const renderStars = (rating: number) => {
    const stars = [];
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 !== 0;

    for (let i = 0; i < fullStars; i++) {
      stars.push(<span key={i} className="star filled">★</span>);
    }

    if (hasHalfStar) {
      stars.push(<span key="half" className="star half">★</span>);
    }

    const emptyStars = 5 - Math.ceil(rating);
    for (let i = 0; i < emptyStars; i++) {
      stars.push(<span key={`empty-${i}`} className="star empty">☆</span>);
    }

    return stars;
  };

  const categories = ['Todos', ...Array.from(new Set(drinks.map(drink => drink.category)))];
  const filteredDrinks = drinks.filter(drink => {
    const matchesCategory = selectedCategory === 'Todos' || drink.category === selectedCategory;
    const matchesSearch = !searchQuery || 
      drink.name.toLowerCase().includes(searchQuery.toLowerCase()) || 
      drink.description.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesPrice = !priceFilter || (() => {
      switch (priceFilter) {
        case 'Hasta $10.000': return drink.price <= 10000;
        case '$10.000 - $25.000': return drink.price > 10000 && drink.price <= 25000;
        case '$25.000 - $50.000': return drink.price > 25000 && drink.price <= 50000;
        case 'Más de $50.000': return drink.price > 50000;
        default: return true;
      }
    })();
    return matchesCategory && matchesSearch && matchesPrice;
  });

  return (
    <div className="menu-page">
      <div className="menu-header">
        <h2>Nuestros Productos</h2>
        <p>Descubre nuestra selección completa de bebidas premium organizada por categorías</p>
      </div>

      {/* Filtros por categoría */}
      <div className="category-filters">
        {categories.map(category => (
          <button
            key={category}
            className={`category-filter ${selectedCategory === category ? 'active' : ''}`}
            onClick={() => setSelectedCategory(category)}
          >
            {category}
          </button>
        ))}
      </div>

      {!isAuthenticated && (
        <div className="auth-banner">
          <p>👋 ¿Quieres hacer un pedido? <Link to="/login">Inicia sesión</Link> o <Link to="/register">crea una cuenta</Link> para comenzar!</p>
        </div>
      )}

      {/* Productos destacados */}
      {selectedCategory === 'Todos' && (
        <section className="featured-section">
          <h3>✨ Productos Destacados</h3>
          <div className="featured-grid">
            {drinks
              .filter(drink => drink.featured)
              .map(drink => (
                <div key={drink.id} className="drink-card featured" onClick={() => handleProductClick(drink)}>
                  {drink.image && (
                    <div className="drink-image">
                      <img src={drink.image} alt={drink.name} />
                      <div className="featured-badge">⭐ Destacado</div>
                    </div>
                  )}
                  <div className="drink-info">
                    <h4>{drink.name}</h4>
                    <p>{drink.description}</p>
                    <div className="drink-rating">
                      {renderStars(drink.rating || 0)}
                      <span className="rating-number">({drink.rating})</span>
                    </div>
                    <span className="price">{formatPrice(drink.price)}</span>
                  </div>
                  <Button
                    variant={isAuthenticated ? "primary" : "outline"}
                    size="sm"
                    onClick={() => addToCart(drink)}
                  >
                    {isAuthenticated ? "Agregar al Carrito" : "Inicia Sesión"}
                  </Button>
                </div>
              ))}
          </div>
        </section>
      )}

      {/* Grid de productos */}
      <div className="drinks-grid">
        {filteredDrinks.map(drink => (
          <div key={drink.id} className="drink-card" onClick={() => handleProductClick(drink)}>
            {drink.image && (
              <div className="drink-image">
                <img src={drink.image} alt={drink.name} />
              </div>
            )}
            <div className="drink-info">
              <h4>{drink.name}</h4>
              <p>{drink.description}</p>
              {drink.rating && (
                <div className="drink-rating">
                  {renderStars(drink.rating)}
                  <span className="rating-number">({drink.rating})</span>
                </div>
              )}
              <span className="price">{formatPrice(drink.price)}</span>
            </div>
            <Button
              variant={isAuthenticated ? "primary" : "outline"}
              size="sm"
              onClick={() => addToCart(drink)}
            >
              {isAuthenticated ? "Agregar al Carrito" : "Inicia Sesión"}
            </Button>
          </div>
        ))}
      </div>

      <Cart />
      <ProductReviewModal
        product={selectedProduct}
        isOpen={reviewModalOpen}
        onClose={() => setReviewModalOpen(false)}
        onSubmit={handleReviewSubmit}
      />
    </div>
  );
};

export default Menu;
