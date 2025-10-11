# 🍸 Gunter-Bar Frontend

## Overview
Modern React + TypeScript frontend for the Gunter-Bar management system, built with Vite for optimal development experience and production performance.

## 🚀 Technology Stack
- **React 18** with TypeScript for type-safe development
- **Vite** for blazing fast development and optimized builds
- **React Router** for client-side routing
- **Axios** for API communication
- **JWT Authentication** integration
- **Responsive Design** for mobile and desktop

## 🔗 Backend Integration
This frontend is designed to work seamlessly with the **Gunter-Bar .NET 9 API**:
- **API Version**: v2.0.0 (October 2025)
- **Base URL**: `http://localhost:5000/api`
- **Authentication**: JWT Bearer tokens
- **Enhanced Features**: Supports all new API endpoints including advanced search, analytics, and real-time features

## ✨ Key Features

### 🔐 Authentication & Authorization
- Secure JWT-based authentication
- Role-based access control (Admin, Employee, Customer)
- Persistent login sessions
- Automatic token refresh

### 📦 Product Management
- Product catalog browsing
- Advanced search and filtering
- Promotional products display
- Category-based navigation
- Shopping cart functionality

### 🧾 Order & Ticket Management
- Real-time order tracking
- Table management interface
- Ticket processing workflow
- Order history and analytics

### 👥 User Management
- User profile management
- Advanced user search (Admin/Employee)
- User statistics dashboard
- Role-based feature access

## 🛠️ Development Setup

### Prerequisites
- **Node.js** (v18 or higher)
- **npm** or **yarn**
- **Gunter-Bar Backend API** running on port 5000

### Installation

1. **Navigate to frontend directory**
   ```bash
   cd Frontend-Bar
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Start development server**
   ```bash
   npm run dev
   ```

4. **Access the application**
   - Frontend: `http://localhost:5173`
   - Ensure backend API is running on `http://localhost:5000`

## 📁 Project Structure

```
Frontend-Bar/
├── public/                    # Static assets
│   └── vite.svg              # Application icon
├── src/
│   ├── components/           # 🧩 Reusable components
│   │   ├── common/          # Shared UI components
│   │   ├── auth/            # Authentication components
│   │   ├── products/        # Product-related components
│   │   └── orders/          # Order management components
│   ├── pages/               # 📄 Page components
│   │   ├── Home/           # Landing page
│   │   ├── Login/          # Authentication page
│   │   ├── Products/       # Product catalog
│   │   ├── Orders/         # Order management
│   │   └── Dashboard/      # Admin dashboard
│   ├── hooks/              # 🎣 Custom React hooks
│   │   ├── useAuth.js      # Authentication hook
│   │   ├── useApi.js       # API communication hook
│   │   └── useLocalStorage.js # Local storage hook
│   ├── services/           # 🌐 API service layer
│   │   ├── authService.js  # Authentication API calls
│   │   ├── productService.js # Product API calls
│   │   ├── orderService.js # Order API calls
│   │   └── userService.js  # User management API calls
│   ├── store/              # 🗄️ State management
│   │   ├── authStore.js    # Authentication state
│   │   ├── cartStore.js    # Shopping cart state
│   │   └── appStore.js     # Global application state
│   ├── routes/             # 🛣️ Route configurations
│   │   └── AppRoutes.jsx   # Main routing setup
│   ├── types/              # 📝 TypeScript type definitions
│   │   ├── auth.types.ts   # Authentication types
│   │   ├── product.types.ts # Product types
│   │   └── api.types.ts    # API response types
│   ├── utils/              # 🛠️ Utility functions
│   │   ├── constants.js    # Application constants
│   │   ├── helpers.js      # Helper functions
│   │   └── validators.js   # Form validation
│   ├── App.jsx             # 🏠 Main application component
│   ├── App.css             # Global styles
│   ├── main.jsx            # Application entry point
│   └── index.css           # Base CSS styles
├── eslint.config.js        # ESLint configuration
├── vite.config.js          # Vite configuration
├── package.json            # Project dependencies
└── README.md               # This file
```

## 🔧 Available Scripts

### Development
```bash
npm run dev          # Start development server with HMR
npm run build        # Build for production
npm run preview      # Preview production build locally
npm run lint         # Run ESLint for code quality
npm run lint:fix     # Fix ESLint issues automatically
```

### Testing
```bash
npm run test         # Run unit tests
npm run test:watch   # Run tests in watch mode
npm run test:coverage # Generate test coverage report
```

## 🌐 API Integration

### Authentication Flow
```javascript
// Login example
import { authService } from './services/authService';

const handleLogin = async (credentials) => {
  try {
    const response = await authService.login(credentials);
    // Handle successful login
    localStorage.setItem('token', response.token);
  } catch (error) {
    // Handle login error
    console.error('Login failed:', error.message);
  }
};
```

### API Service Usage
```javascript
// Product service example
import { productService } from './services/productService';

const fetchPromotionalProducts = async () => {
  try {
    const products = await productService.getPromotional();
    setPromotionalProducts(products);
  } catch (error) {
    console.error('Failed to fetch promotional products:', error);
  }
};
```

## 🎨 Styling & Theming

### CSS Architecture
- **Component-scoped styles** for maintainability
- **CSS variables** for consistent theming
- **Responsive design** with mobile-first approach
- **Dark theme** support for bar ambiance

### Theme Configuration
```css
:root {
  --primary-color: #8B4513;     /* Rich brown */
  --secondary-color: #DAA520;   /* Golden accent */
  --background-dark: #1a1a1a;   /* Dark background */
  --text-light: #f5f5f5;       /* Light text */
  --accent-red: #dc3545;        /* Error/warning */
  --accent-green: #28a745;      /* Success */
}
```

## 🔒 Security Features

### Client-Side Security
- **JWT token management** with automatic expiration
- **Role-based route protection**
- **Input validation** and sanitization
- **XSS prevention** measures
- **Secure local storage** usage

### Best Practices
- Environment variables for sensitive configuration
- HTTP-only cookies for token storage (where applicable)
- Regular dependency updates for security patches
- Content Security Policy headers

## 📱 Responsive Design

### Breakpoints
- **Mobile**: < 768px
- **Tablet**: 768px - 1024px
- **Desktop**: > 1024px

### Mobile Features
- Touch-friendly interface
- Swipe gestures for navigation
- Optimized images for mobile devices
- Progressive Web App (PWA) capabilities

## 🚀 Performance Optimizations

### Build Optimizations
- **Code splitting** for reduced bundle size
- **Lazy loading** for route components
- **Image optimization** and compression
- **Tree shaking** for unused code elimination

### Runtime Optimizations
- **React.memo** for component optimization
- **useMemo** and **useCallback** for expensive operations
- **Virtual scrolling** for large lists
- **Debounced search** inputs

## 🧪 Testing Strategy

### Testing Stack
- **Vitest** for unit testing
- **React Testing Library** for component testing
- **MSW** for API mocking
- **Cypress** for end-to-end testing

### Test Coverage
- Components: 90%+ coverage target
- Services: 95%+ coverage target
- Utilities: 100% coverage target

## 📦 Deployment

### Production Build
```bash
npm run build        # Creates optimized production build
```

### Environment Variables
```env
VITE_API_BASE_URL=http://localhost:5000/api
VITE_APP_NAME=Gunter Bar
VITE_APP_VERSION=2.0.0
```

### Docker Support
```dockerfile
# Multi-stage build for production
FROM node:18-alpine as builder
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production

FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
```

## 🤝 Contributing

### Development Workflow
1. Fork the repository
2. Create a feature branch
3. Follow coding conventions
4. Write tests for new features
5. Submit a pull request

### Code Style
- **ESLint** configuration for code quality
- **Prettier** for consistent formatting
- **TypeScript** for type safety
- **Component naming**: PascalCase
- **File naming**: camelCase

## 🐛 Troubleshooting

### Common Issues

#### CORS Errors
```javascript
// Ensure backend CORS configuration allows frontend origin
// Check backend appsettings.json for AllowedOrigins
```

#### API Connection Issues
```javascript
// Verify backend is running on correct port
// Check network connectivity
// Validate JWT token expiration
```

#### Build Failures
```bash
# Clear node_modules and reinstall
rm -rf node_modules package-lock.json
npm install
```

## 📚 Documentation

### Useful Links
- [React Documentation](https://reactjs.org/docs)
- [Vite Guide](https://vitejs.dev/guide/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Backend API Documentation](../Backend-Bar/README.md)

## 🏫 Academic Information

- **School**: Escuela Técnica N° 12 D.E. 1° "Libertador Gral. José de San Martín" (ET12)
- **Subject**: Desarrollo de Sistemas
- **Academic Year**: 2025
- **Class**: 6°17°
- **Students**: Sofia Colman, Camila Reyes, Ana Martinez, Roque Rivas, Julio Martinez
- **Teachers**: Sergio Mendoza, Adrián Cives

## 📝 License

This project is part of a school assignment and is intended for educational purposes.
