import React, { useState } from 'react';
import { Button } from '../buttons';

interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  category: string;
  rating: number;
  image?: string;
  images?: string[]; // Galería de fotos
  createdAt?: string; // Fecha de creación
  tags?: string[]; // Etiquetas
  alcoholContent?: number; // Contenido de alcohol
  volume?: string; // Volumen (ej: 330ml)
  origin?: string; // Origen
  recommendations?: Product[]; // Productos recomendados
}

interface Review {
  id: number;
  userName: string;
  rating: number;
  comment: string;
  date: string;
  images?: string[];
}

interface ProductReviewModalProps {
  product: Product | null;
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (productId: number, rating: number, comment: string, images: File[]) => void;
}

const ProductReviewModal: React.FC<ProductReviewModalProps> = ({
  product,
  isOpen,
  onClose,
  onSubmit
}) => {
  const [rating, setRating] = useState(0);
  const [hoverRating, setHoverRating] = useState(0);
  const [comment, setComment] = useState('');
  const [selectedImages, setSelectedImages] = useState<File[]>([]);
  const [imagePreviews, setImagePreviews] = useState<string[]>([]);

  // Reseñas de ejemplo (en un futuro vendrían del backend)
  const [existingReviews] = useState<Review[]>([
    {
      id: 1,
      userName: "María González",
      rating: 5,
      comment: "¡Excelente producto! La calidad es increíble y el sabor es perfecto. Definitivamente lo recomiendo.",
      date: "2025-10-25",
      images: ["https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=100&h=100&fit=crop"]
    },
    {
      id: 2,
      userName: "Carlos Rodríguez",
      rating: 4,
      comment: "Muy bueno, aunque esperaba un poco más de intensidad en el sabor. Aun así, vale la pena.",
      date: "2025-10-20"
    },
    {
      id: 3,
      userName: "Vendedor Premium - Juan Pérez",
      rating: 5,
      comment: "Como vendedor certificado, puedo confirmar que este producto mantiene la más alta calidad en cada botella. Nuestros clientes lo adoran.",
      date: "2025-10-22",
      images: [
        "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=100&h=100&fit=crop",
        "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=100&h=100&fit=crop"
      ]
    }
  ]);

  if (!isOpen || !product) return null;

  const handleStarClick = (starValue: number) => {
    setRating(starValue);
  };

  const handleStarHover = (starValue: number) => {
    setHoverRating(starValue);
  };

  const handleStarLeave = () => {
    setHoverRating(0);
  };

  const handleSubmit = () => {
    if (rating > 0) {
      onSubmit(product.id, rating, comment, selectedImages);
      setRating(0);
      setComment('');
      setSelectedImages([]);
      setImagePreviews([]);
      onClose();
    }
  };

  const handleImageSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files;
    if (files) {
      const newFiles = Array.from(files);
      const newPreviews: string[] = [];

      newFiles.forEach(file => {
        const reader = new FileReader();
        reader.onload = (e) => {
          if (e.target?.result) {
            newPreviews.push(e.target.result as string);
            if (newPreviews.length === newFiles.length) {
              setImagePreviews(prev => [...prev, ...newPreviews]);
            }
          }
        };
        reader.readAsDataURL(file);
      });

      setSelectedImages(prev => [...prev, ...newFiles]);
    }
  };

  const removeImage = (index: number) => {
    setSelectedImages(prev => prev.filter((_, i) => i !== index));
    setImagePreviews(prev => prev.filter((_, i) => i !== index));
  };

  const renderInteractiveStars = () => {
    return (
      <div className="rating-stars">
        {[1, 2, 3, 4, 5].map((star) => (
          <span
            key={star}
            className={`star ${star <= (hoverRating || rating) ? 'filled' : 'empty'}`}
            onClick={() => handleStarClick(star)}
            onMouseEnter={() => handleStarHover(star)}
            onMouseLeave={handleStarLeave}
          >
            ★
          </span>
        ))}
      </div>
    );
  };

  const renderStaticStars = (rating: number) => {
    return (
      <div className="rating-stars">
        {[1, 2, 3, 4, 5].map((star) => (
          <span
            key={star}
            className={`star ${star <= rating ? 'filled' : 'empty'}`}
          >
            ★
          </span>
        ))}
      </div>
    );
  };

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(price);
  };

  return (
    <div className="review-modal-overlay" onClick={onClose}>
      <div className="review-modal-content large-modal" onClick={(e) => e.stopPropagation()}>
        <div className="review-modal-header">
          <h3>Detalles del Producto</h3>
          <button className="close-btn" onClick={onClose}>✕</button>
        </div>

        <div className="product-details-section">
          {/* Galería de fotos */}
          <div className="product-gallery">
            <div className="main-image">
              <img
                src={product.images?.[0] || product.image || '/placeholder.jpg'}
                alt={product.name}
                className="product-main-image"
              />
            </div>
            {product.images && product.images.length > 1 && (
              <div className="image-thumbnails">
                {product.images.map((image, index) => (
                  <img
                    key={index}
                    src={image}
                    alt={`${product.name} ${index + 1}`}
                    className="thumbnail-image"
                  />
                ))}
              </div>
            )}
          </div>

          {/* Información detallada del producto */}
          <div className="product-info-section">
            <div className="product-header">
              <h2>{product.name}</h2>
              <div className="product-meta">
                <span className="product-category">{product.category}</span>
                {product.createdAt && (
                  <span className="product-date">
                    Agregado: {new Date(product.createdAt).toLocaleDateString('es-ES')}
                  </span>
                )}
              </div>
            </div>

            <div className="product-rating-large">
              {renderStaticStars(product.rating)}
              <span className="rating-number">({product.rating})</span>
            </div>

            <div className="product-price-large">
              {formatPrice(product.price)}
            </div>

            <div className="product-description-full">
              <h4>Descripción</h4>
              <p>{product.description}</p>
            </div>

            <div className="product-details-grid">
              {product.volume && (
                <div className="detail-item">
                  <span className="detail-label">Volumen:</span>
                  <span className="detail-value">{product.volume}</span>
                </div>
              )}
              {product.alcoholContent && (
                <div className="detail-item">
                  <span className="detail-label">Contenido de Alcohol:</span>
                  <span className="detail-value">{product.alcoholContent}%</span>
                </div>
              )}
              {product.origin && (
                <div className="detail-item">
                  <span className="detail-label">Origen:</span>
                  <span className="detail-value">{product.origin}</span>
                </div>
              )}
            </div>

            {product.tags && product.tags.length > 0 && (
              <div className="product-tags">
                <h4>Etiquetas</h4>
                <div className="tags-container">
                  {product.tags.map((tag, index) => (
                    <span key={index} className="tag">{tag}</span>
                  ))}
                </div>
              </div>
            )}
          </div>
        </div>

        {/* Recomendaciones */}
        {product.recommendations && product.recommendations.length > 0 && (
          <div className="recommendations-section">
            <h4>Te recomendamos</h4>
            <div className="recommendations-grid">
              {product.recommendations.map(rec => (
                <div key={rec.id} className="recommendation-card">
                  <img src={rec.image} alt={rec.name} className="recommendation-image" />
                  <div className="recommendation-info">
                    <h5>{rec.name}</h5>
                    <p>{formatPrice(rec.price)}</p>
                    <div className="recommendation-rating">
                      {renderStaticStars(rec.rating)}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* Reseñas existentes */}
        <div className="existing-reviews-section">
          <h4>Reseñas de otros clientes ({existingReviews.length})</h4>
          {existingReviews.length > 0 ? (
            <div className="reviews-list">
              {existingReviews.map(review => (
                <div key={review.id} className="review-item">
                  <div className="review-header">
                    <span className="review-user">{review.userName}</span>
                    <div className="review-rating">
                      {renderStaticStars(review.rating)}
                    </div>
                    <span className="review-date">{new Date(review.date).toLocaleDateString('es-ES')}</span>
                  </div>
                  <p className="review-comment">{review.comment}</p>
                  {review.images && review.images.length > 0 && (
                    <div className="review-images">
                      {review.images.map((image, index) => (
                        <img key={index} src={image} alt={`Review ${index + 1}`} className="review-image" />
                      ))}
                    </div>
                  )}
                </div>
              ))}
            </div>
          ) : (
            <p className="no-reviews">Aún no hay reseñas para este producto. ¡Sé el primero en opinar!</p>
          )}
        </div>

        {/* Formulario para dejar reseña */}
        <div className="leave-review-section">
          <h4>Deja tu reseña</h4>

          <div className="review-rating-section">
            <h5>¿Qué te pareció este producto?</h5>
            <div className="rating-container">
              {renderInteractiveStars()}
              <span className="rating-text">
                {rating === 0 ? 'Selecciona tu calificación' :
                 rating === 1 ? 'Muy malo' :
                 rating === 2 ? 'Malo' :
                 rating === 3 ? 'Regular' :
                 rating === 4 ? 'Bueno' : 'Excelente'}
              </span>
            </div>
          </div>

          <div className="review-comment-section">
            <h5>Comentario (opcional)</h5>
            <textarea
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === 'Enter' && !e.shiftKey && rating > 0) {
                  e.preventDefault();
                  handleSubmit();
                }
              }}
              placeholder="Comparte tu experiencia con este producto..."
              rows={4}
              maxLength={500}
            />
            <div className="comment-counter">
              {comment.length}/500 caracteres
            </div>
            <div className="image-upload-inline">
              <input
                type="file"
                accept="image/*"
                multiple
                onChange={handleImageSelect}
                id="image-upload"
                className="image-input"
              />
              <label htmlFor="image-upload" className="image-upload-label-compact">
                <span>�</span>
              </label>
              {imagePreviews.length > 0 && (
                <div className="image-previews">
                  {imagePreviews.map((preview, index) => (
                    <div key={index} className="image-preview">
                      <img src={preview} alt={`Preview ${index + 1}`} />
                      <button
                        type="button"
                        onClick={() => removeImage(index)}
                        className="remove-image-btn"
                      >
                        ✕
                      </button>
                    </div>
                  ))}
                </div>
              )}
            </div>
          </div>

          <div className="review-submit-section">
            <Button
              variant="primary"
              size="md"
              onClick={handleSubmit}
              disabled={rating === 0}
            >
              ➤
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductReviewModal;
