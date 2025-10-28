import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../components/forms/AuthContext';
import { Button } from '../components/buttons';
import { useIntersectionObserver } from '../utils/useIntersectionObserver';
import { useCart } from '../components/common/CartContext';
import Cart from '../components/common/Cart';
import ProductReviewModal from '../components/common/ProductReviewModal';
import './pages.css';

interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  category: string;
  rating: number;
  image?: string;
  images?: string[];
  createdAt?: string;
  tags?: string[];
  alcoholContent?: number;
  volume?: string;
  origin?: string;
  recommendations?: Product[];
  featured?: boolean;
}

const Home: React.FC = () => {
  const { isAuthenticated } = useAuth();
  const { addItem } = useCart();
  const { ref: productsRef, isIntersecting: productsVisible } = useIntersectionObserver();

  // Estado para el modal de reseñas
  const [reviewModalOpen, setReviewModalOpen] = React.useState(false);
  const [selectedProduct, setSelectedProduct] = React.useState<Product | null>(null);

  // Debug: mostrar cuando las animaciones se activan
  React.useEffect(() => {
    if (productsVisible) {
      console.log('Productos visibles - animaciones activadas');
    }
  }, [productsVisible]);

  // Productos destacados
  const featuredProducts: Product[] = [
    {
      id: 1,
      name: "Mojito Clásico",
      description: "Refrescante cóctel con ron, menta fresca, azúcar y soda. Perfecto para días calurosos. Una bebida clásica que combina los sabores tradicionales cubanos con un toque moderno.",
      price: 8500,
      category: "Cócteles",
      rating: 4.8,
      image: "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=300&h=300&fit=crop",
      images: [
        "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1551218808-94e220e084d2?w=600&h=400&fit=crop"
      ],
      createdAt: "2025-01-15",
      tags: ["Refrescante", "Clásico", "Verano"],
      alcoholContent: 12,
      volume: "250ml",
      origin: "Cuba",
      featured: true,
      recommendations: [
        {
          id: 5,
          name: "Margarita Frozen",
          description: "Cóctel refrescante con tequila, triple sec y jugo de limón, servido congelado.",
          price: 9500,
          category: "Cócteles",
          rating: 4.5,
          image: "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=300&h=300&fit=crop"
        },
        {
          id: 6,
          name: "Cerveza Lager Premium",
          description: "Cerveza lager suave y refrescante, perfecta para cualquier ocasión.",
          price: 5500,
          category: "Cervezas",
          rating: 4.4,
          image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop"
        }
      ]
    },
    {
      id: 2,
      name: "Cerveza Artesanal IPA",
      description: "Cerveza artesanal con notas cítricas y amargas. Elaborada con lúpulos premium de las mejores cosechas. Una IPA que destaca por su amargor balanceado y aroma floral intenso.",
      price: 6500,
      category: "Cervezas",
      rating: 4.6,
      image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop",
      images: [
        "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1551218377-a0d05627c4e0?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1586790170083-2f9ceadc732d?w=600&h=400&fit=crop"
      ],
      createdAt: "2025-02-20",
      tags: ["Artesanal", "IPA", "Amarga"],
      alcoholContent: 6.5,
      volume: "330ml",
      origin: "Colombia",
      recommendations: [
        {
          id: 6,
          name: "Cerveza Lager Premium",
          description: "Cerveza lager suave y refrescante, perfecta para cualquier ocasión.",
          price: 5500,
          category: "Cervezas",
          rating: 4.4,
          image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop"
        },
        {
          id: 3,
          name: "Stout Oscura",
          description: "Cerveza stout oscura con notas de chocolate y café, ideal para paladares intensos.",
          price: 6800,
          category: "Cervezas",
          rating: 4.3,
          image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop"
        }
      ]
    },
    {
      id: 3,
      name: "Whisky Escocés 12 años",
      description: "Whisky escocés añejado 12 años con notas de vainilla, roble y miel. Un single malt premium que ha reposado en barricas de roble americano y europeo.",
      price: 45000,
      category: "Whiskies",
      rating: 4.9,
      image: "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=300&h=300&fit=crop",
      images: [
        "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1510812431401-41d2bd2722f3?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1586375300773-8384e3e4916f?w=600&h=400&fit=crop"
      ],
      createdAt: "2025-03-10",
      tags: ["Premium", "Single Malt", "12 años"],
      alcoholContent: 40,
      volume: "750ml",
      origin: "Escocia",
      featured: true,
      recommendations: [
        {
          id: 7,
          name: "Ron Añejo 15 años",
          description: "Ron premium añejado con notas de caramelo, vainilla y roble tostado.",
          price: 52000,
          category: "Rones",
          rating: 4.8,
          image: "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=300&h=300&fit=crop"
        },
        {
          id: 4,
          name: "Vino Tinto Cabernet",
          description: "Vino tinto robusto con taninos suaves y aromas a frutas rojas maduras.",
          price: 18000,
          category: "Vinos",
          rating: 4.7,
          image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop"
        }
      ]
    },
    {
      id: 4,
      name: "Vino Tinto Cabernet",
      description: "Vino tinto robusto con taninos suaves y aromas a frutas rojas maduras. Un Cabernet Sauvignon que combina la intensidad de los taninos con la suavidad del roble.",
      price: 18000,
      category: "Vinos",
      rating: 4.7,
      image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop",
      images: [
        "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1586370434639-0fe43b2d32d6?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1553361371-9b22f78d8b7d?w=600&h=400&fit=crop"
      ],
      createdAt: "2025-04-05",
      tags: ["Tinto", "Cabernet", "Robusto"],
      alcoholContent: 13.5,
      volume: "750ml",
      origin: "Chile",
      recommendations: [
        {
          id: 8,
          name: "Vino Blanco Sauvignon Blanc",
          description: "Vino blanco fresco con aromas cítricos y notas herbales, ideal para mariscos.",
          price: 22000,
          category: "Vinos",
          rating: 4.6,
          image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop"
        },
        {
          id: 3,
          name: "Whisky Escocés 12 años",
          description: "Whisky escocés añejado 12 años con notas de vainilla, roble y miel.",
          price: 45000,
          category: "Whiskies",
          rating: 4.9,
          image: "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=300&h=300&fit=crop"
        }
      ]
    },
    {
      id: 5,
      name: "Margarita Frozen",
      description: "Cóctel refrescante con tequila, triple sec y jugo de limón, servido congelado. Una versión moderna del clásico margarita con un toque helado irresistible.",
      price: 9500,
      category: "Cócteles",
      rating: 4.5,
      image: "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=300&h=300&fit=crop",
      images: [
        "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1551218808-94e220e084d2?w=600&h=400&fit=crop"
      ],
      createdAt: "2025-05-12",
      tags: ["Frozen", "Refrescante", "Cítrico"],
      alcoholContent: 15,
      volume: "300ml",
      origin: "México",
      recommendations: [
        {
          id: 1,
          name: "Mojito Clásico",
          description: "Refrescante cóctel con ron, menta fresca, azúcar y soda.",
          price: 8500,
          category: "Cócteles",
          rating: 4.8,
          image: "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=300&h=300&fit=crop"
        },
        {
          id: 6,
          name: "Cerveza Lager Premium",
          description: "Cerveza lager suave y refrescante, perfecta para cualquier ocasión.",
          price: 5500,
          category: "Cervezas",
          rating: 4.4,
          image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop"
        }
      ]
    },
    {
      id: 6,
      name: "Cerveza Lager Premium",
      description: "Cerveza lager suave y refrescante, perfecta para cualquier ocasión. Una lager premium con un equilibrio perfecto entre maltas y lúpulos.",
      price: 5500,
      category: "Cervezas",
      rating: 4.4,
      image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop",
      images: [
        "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1551218377-a0d05627c4e0?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1586790170083-2f9ceadc732d?w=600&h=400&fit=crop"
      ],
      createdAt: "2025-06-08",
      tags: ["Lager", "Premium", "Suave"],
      alcoholContent: 5,
      volume: "330ml",
      origin: "Colombia",
      recommendations: [
        {
          id: 2,
          name: "Cerveza Artesanal IPA",
          description: "Cerveza artesanal con notas cítricas y amargas.",
          price: 6500,
          category: "Cervezas",
          rating: 4.6,
          image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop"
        },
        {
          id: 3,
          name: "Stout Oscura",
          description: "Cerveza stout oscura con notas de chocolate y café.",
          price: 6800,
          category: "Cervezas",
          rating: 4.3,
          image: "https://images.unsplash.com/photo-1608270586620-248524c67de9?w=300&h=300&fit=crop"
        }
      ]
    },
    {
      id: 7,
      name: "Ron Añejo 15 años",
      description: "Ron premium añejado con notas de caramelo, vainilla y roble tostado. Un ron añejo excepcional que ha reposado 15 años en barricas de roble blanco.",
      price: 52000,
      category: "Rones",
      rating: 4.8,
      image: "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=300&h=300&fit=crop",
      images: [
        "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1510812431401-41d2bd2722f3?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1586375300773-8384e3e4916f?w=600&h=400&fit=crop"
      ],
      createdAt: "2025-07-15",
      tags: ["Añejo", "Premium", "15 años"],
      alcoholContent: 40,
      volume: "750ml",
      origin: "Caribe",
      recommendations: [
        {
          id: 3,
          name: "Whisky Escocés 12 años",
          description: "Whisky escocés añejado 12 años con notas de vainilla, roble y miel.",
          price: 45000,
          category: "Whiskies",
          rating: 4.9,
          image: "https://images.unsplash.com/photo-1528823872057-9c018a7a7553?w=300&h=300&fit=crop"
        },
        {
          id: 4,
          name: "Vino Tinto Cabernet",
          description: "Vino tinto robusto con taninos suaves y aromas a frutas rojas maduras.",
          price: 18000,
          category: "Vinos",
          rating: 4.7,
          image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop"
        }
      ]
    },
    {
      id: 8,
      name: "Vino Blanco Sauvignon Blanc",
      description: "Vino blanco fresco con aromas cítricos y notas herbales, ideal para mariscos. Un Sauvignon Blanc que destaca por su acidez refrescante y aromas tropicales.",
      price: 22000,
      category: "Vinos",
      rating: 4.6,
      image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop",
      images: [
        "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1586370434639-0fe43b2d32d6?w=600&h=400&fit=crop",
        "https://images.unsplash.com/photo-1553361371-9b22f78d8b7d?w=600&h=400&fit=crop"
      ],
      createdAt: "2025-08-20",
      tags: ["Blanco", "Sauvignon Blanc", "Fresco"],
      alcoholContent: 12.5,
      volume: "750ml",
      origin: "Chile",
      recommendations: [
        {
          id: 4,
          name: "Vino Tinto Cabernet",
          description: "Vino tinto robusto con taninos suaves y aromas a frutas rojas maduras.",
          price: 18000,
          category: "Vinos",
          rating: 4.7,
          image: "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=300&h=300&fit=crop"
        },
        {
          id: 1,
          name: "Mojito Clásico",
          description: "Refrescante cóctel con ron, menta fresca, azúcar y soda.",
          price: 8500,
          category: "Cócteles",
          rating: 4.8,
          image: "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=300&h=300&fit=crop"
        }
      ]
    }
  ];

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(price);
  };

  const handleProductClick = (product: Product) => {
    setSelectedProduct(product);
    setReviewModalOpen(true);
  };

  const handleReviewSubmit = (productId: number, rating: number, comment: string) => {
    // Aquí puedes enviar la reseña al backend
    console.log('Reseña enviada:', { productId, rating, comment });
    alert(`¡Gracias por tu reseña! Calificación: ${rating} estrellas${comment ? '\nComentario: ' + comment : ''}`);
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

  return (
    <div className="home-page">
      <section className="hero">
        <div className="hero-header">
          <img src="/logo.jpeg" alt="Bar Gunter Logo" className="hero-logo" />
          <h2>¡Bienvenido a Bar Gunter!</h2>
        </div>
        <p className="hero-description">
          En Bar Gunter, somos apasionados por ofrecerte las mejores experiencias con bebidas premium. 
          Desde cócteles artesanales preparados por expertos bartenders, hasta una selección exclusiva de 
          whiskies añejos, vinos finos y cervezas artesanales. Cada bebida cuenta una historia y está 
          diseñada para crear momentos inolvidables.
        </p>
        <div className="hero-highlights">
          <div className="highlight-item">
            <span className="highlight-icon">🍸</span>
            <span>Cócteles Artesanales</span>
          </div>
          <div className="highlight-item">
            <span className="highlight-icon">🥂</span>
            <span>Bebidas Premium</span>
          </div>
          <div className="highlight-item">
            <span className="highlight-icon">🌟</span>
            <span>Experiencia Única</span>
          </div>
        </div>
        <div className="hero-actions">
          <Link to="/menu">
            <Button variant="primary" size="lg">
              Ver Nuestros Productos
            </Button>
          </Link>
          {!isAuthenticated && (
            <div className="auth-prompt">
              <p>¿Listo para ordenar? ¡Crea una cuenta para comenzar!</p>
              <div className="auth-buttons">
                <Link to="/register">
                  <Button variant="outline" size="md">
                    Registrarse
                  </Button>
                </Link>
                <Link to="/login">
                  <Button variant="secondary" size="md">
                    Iniciar Sesión
                  </Button>
                </Link>
              </div>
            </div>
          )}
        </div>
      </section>

      <section className="features">
        <h3>¿Por qué elegir Gunter Bar?</h3>
        <div className="features-grid">
          <div className="feature-card">
            <h4>🍹 Bebidas Premium</h4>
            <p>Cócteles artesanales y bebidas elaboradas con los mejores ingredientes</p>
          </div>
          <div className="feature-card">
            <h4>🚚 Entrega Rápida</h4>
            <p>Entrega rápida y confiable hasta tu puerta</p>
          </div>
          <div className="feature-card">
            <h4>⭐ Servicio de Calidad</h4>
            <p>Servicio al cliente excepcional y soporte</p>
          </div>
        </div>
      </section>

      <section className="extravagant-products">
        <h3>✨ Bebidas Extravagantes</h3>
        <p className="section-subtitle">Descubre nuestras creaciones más exclusivas y sofisticadas</p>
        <div className="products-grid extravagant-grid">
          {featuredProducts.filter(product => product.featured).map((product, index) => (
            <div
              key={product.id}
              className={`product-card extravagant-card animate-on-scroll ${productsVisible ? 'visible' : ''}`}
              style={{ animationDelay: `${index * 0.15}s` }}
              onClick={() => handleProductClick(product)}
            >
              <div className="product-badge">EXTRAVAGANTE</div>
              <div className="product-image">
                <img src={product.image} alt={product.name} />
                <div className="product-category">{product.category}</div>
              </div>
              <div className="product-info">
                <h4 className="product-name">{product.name}</h4>
                <p className="product-description">{product.description}</p>
                <div className="product-rating">
                  {renderStars(product.rating)}
                  <span className="rating-number">({product.rating})</span>
                </div>
                <div className="product-price extravagant-price">
                  {formatPrice(product.price)}
                </div>
                <div className="product-actions">
                  <Button
                    variant="primary"
                    size="sm"
                    className="buy-button extravagant-button"
                    onClick={() => addItem({
                      id: product.id,
                      name: product.name,
                      price: product.price,
                      image: product.image
                    })}
                  >
                    ¡Pedir Ahora!
                  </Button>
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>

      <section className="featured-products" ref={productsRef}>
        <h3>🌟 Productos Destacados</h3>
        <p className="section-subtitle">Descubre nuestras bebidas más populares y apreciadas</p>
        <div className="products-grid">
          {featuredProducts.map((product, index) => (
            <div
              key={product.id}
              className={`product-card animate-on-scroll ${productsVisible ? 'visible' : ''}`}
              style={{ animationDelay: `${index * 0.1}s` }}
              onClick={() => handleProductClick(product)}
            >
              <div className="product-image">
                <img src={product.image} alt={product.name} />
                <div className="product-category">{product.category}</div>
              </div>
              <div className="product-info">
                <h4 className="product-name">{product.name}</h4>
                <p className="product-description">{product.description}</p>
                <div className="product-rating">
                  {renderStars(product.rating)}
                  <span className="rating-number">({product.rating})</span>
                </div>
                <div className="product-price">
                  {formatPrice(product.price)}
                </div>
                <div className="product-actions">
                  <Button
                    variant="primary"
                    size="sm"
                    className="buy-button"
                    onClick={() => addItem({
                      id: product.id,
                      name: product.name,
                      price: product.price,
                      image: product.image
                    })}
                  >
                    Agregar al Carrito
                  </Button>
                  <Link to="/menu">
                    <Button variant="outline" size="sm">
                      Ver Más
                    </Button>
                  </Link>
                </div>
              </div>
            </div>
          ))}
        </div>
      </section>

      <section className="cta-section">
        <div className="cta-content">
          <h3>¿Listo para explorar más?</h3>
          <p>Descubre toda nuestra colección completa de bebidas premium en nuestros productos</p>
          <Link to="/menu">
            <Button variant="primary" size="lg" className="cta-button">
              Ver Todos los Productos 🍹
            </Button>
          </Link>
        </div>
      </section>

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

export default Home;
