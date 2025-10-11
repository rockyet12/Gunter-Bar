# 🏗️ Gunter-Bar Architecture v2.0

## Overview
This document describes the updated architecture of Gunter-Bar after the major .NET 9 upgrade and feature enhancements implemented in October 2025.

## 🎯 Architecture Principles

### Clean Architecture
The system follows Clean Architecture principles with clear separation of concerns:

1. **Domain Layer** (Core Business Logic)
2. **Application Layer** (Use Cases & Services)  
3. **Infrastructure Layer** (Data Access & External Services)
4. **Presentation Layer** (API Controllers & Frontend)

### Key Benefits
- **Testability**: Each layer can be tested independently
- **Maintainability**: Clear boundaries and responsibilities
- **Flexibility**: Easy to swap implementations
- **Scalability**: Loosely coupled components

---

## 🔧 Technology Stack v2.0

### Backend (.NET 9)
```
┌─────────────────────────────────────────┐
│              .NET 9.0                   │
├─────────────────────────────────────────┤
│ ASP.NET Core Web API                    │
│ Entity Framework Core 9.0              │
│ JWT Authentication Bearer               │
│ Swagger/OpenAPI Documentation          │
│ MySQL via Pomelo Provider               │
└─────────────────────────────────────────┘
```

### Frontend (React + TypeScript)
```
┌─────────────────────────────────────────┐
│           React 18 + TypeScript         │
├─────────────────────────────────────────┤
│ Vite (Build Tool)                       │
│ React Router (Navigation)               │
│ Axios (HTTP Client)                     │
│ JWT Token Management                    │
│ Responsive UI Components                │
└─────────────────────────────────────────┘
```

### Database (MySQL/MariaDB)
```
┌─────────────────────────────────────────┐
│         MySQL/MariaDB 10.11+            │
├─────────────────────────────────────────┤
│ Code-First Migrations                   │
│ Optimized Indexes                       │
│ Foreign Key Constraints                 │
│ Transaction Support                     │
└─────────────────────────────────────────┘
```

---

## 📊 System Architecture Diagram

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   React Client  │    │    Admin Panel  │    │  Mobile Client  │
│   (TypeScript)  │    │   (React PWA)   │    │   (Future)      │
└─────────┬───────┘    └─────────┬───────┘    └─────────┬───────┘
          │                      │                      │
          └──────────────────────┼──────────────────────┘
                                 │
                    ┌────────────▼────────────┐
                    │     Load Balancer       │
                    │      (Nginx)           │
                    └────────────┬────────────┘
                                 │
                    ┌────────────▼────────────┐
                    │   ASP.NET Core API      │
                    │      (.NET 9)          │
                    │                        │
                    │  ┌─────────────────┐   │
                    │  │  Controllers    │   │
                    │  │  - User         │   │
                    │  │  - Product      │   │
                    │  │  - Order        │   │
                    │  │  - Ticket       │   │
                    │  │  - Drink        │   │
                    │  └─────────────────┘   │
                    │                        │
                    │  ┌─────────────────┐   │
                    │  │   Services      │   │
                    │  │  - Business     │   │
                    │  │  - Auth         │   │
                    │  │  - Analytics    │   │
                    │  └─────────────────┘   │
                    │                        │
                    │  ┌─────────────────┐   │
                    │  │  Repositories   │   │
                    │  │  - Data Access  │   │
                    │  │  - Caching      │   │
                    │  └─────────────────┘   │
                    └────────────┬────────────┘
                                 │
                    ┌────────────▼────────────┐
                    │     MySQL Database      │
                    │                        │
                    │  ┌─────────────────┐   │
                    │  │     Tables      │   │
                    │  │  - Users        │   │
                    │  │  - Products     │   │
                    │  │  - Orders       │   │
                    │  │  - Tickets      │   │
                    │  │  - Categories   │   │
                    │  └─────────────────┘   │
                    └─────────────────────────┘
```

---

## 🏛️ Backend Architecture (Clean Architecture)

### Domain Layer (`BarGunter.Domain`)
```
Domain/
├── Entities/
│   ├── User.cs              # User entity with roles
│   ├── Product.cs           # Product catalog
│   ├── Category.cs          # Product categories
│   ├── Order.cs             # Customer orders
│   ├── OrderItem.cs         # Order line items
│   ├── Ticket.cs            # Table tickets
│   ├── Drink.cs             # Beverage catalog
│   ├── DrinkType.cs         # Drink categories
│   └── Cart.cs              # Shopping cart
└── Enums/
    ├── UserRole.cs          # Admin, Employee, Customer
    ├── OrderStatus.cs       # Pending, Processing, etc.
    └── TicketStatus.cs      # Active, Closed, etc.
```

### Application Layer (`BarGunter.Application`)
```
Application/
├── Services/
│   ├── IUserService.cs          # User management
│   ├── IProductService.cs       # Product operations
│   ├── IOrderService.cs         # Order processing
│   ├── ITicketService.cs        # Ticket management
│   ├── IDrinkService.cs         # Beverage operations
│   ├── ICategoryService.cs      # Category management
│   └── IAuthService.cs          # Authentication
├── DTOs/
│   ├── UserDto.cs               # User data transfer
│   ├── ProductDto.cs            # Product data transfer
│   ├── OrderDto.cs              # Order data transfer
│   └── ...                     # Other DTOs
├── Common/
│   ├── Interfaces/
│   ├── Behaviors/               # Cross-cutting concerns
│   └── Exceptions/              # Custom exceptions
└── Contracts/
    ├── IRepositories/           # Repository interfaces
    └── IServices/               # Service interfaces
```

### Infrastructure Layer (`BarGunter.Infrastructure`)
```
Infrastructure/
├── Data/
│   └── ApplicationDbContext.cs  # EF Core context
├── Repositories/
│   ├── UserRepository.cs        # User data access
│   ├── ProductRepository.cs     # Product data access
│   ├── OrderRepository.cs       # Order data access
│   └── ...                     # Other repositories
├── Migrations/                  # EF Core migrations
├── Persistences/               # Data persistence logic
└── Settings/                   # Configuration classes
```

### Presentation Layer (`BarGunter.API`)
```
API/
├── Controllers/
│   ├── UserController.cs        # User endpoints
│   ├── ProductController.cs     # Product endpoints
│   ├── OrderController.cs       # Order endpoints
│   ├── TicketController.cs      # Ticket endpoints
│   ├── DrinkController.cs       # Drink endpoints
│   └── ...                     # Other controllers
├── Configuration/
│   ├── DependencyInjection.cs  # IoC container setup
│   ├── JwtSetup.cs             # JWT configuration
│   └── SwaggerSetup.cs         # API documentation
└── Program.cs                  # Application startup
```

---

## 🌐 Frontend Architecture (React + TypeScript)

### Component Architecture
```
src/
├── components/                  # Reusable UI components
│   ├── common/                 # Shared components
│   │   ├── Button/
│   │   ├── Modal/
│   │   ├── Form/
│   │   └── Layout/
│   ├── auth/                   # Authentication components
│   │   ├── LoginForm/
│   │   ├── RegisterForm/
│   │   └── ProtectedRoute/
│   ├── products/               # Product components
│   │   ├── ProductCard/
│   │   ├── ProductList/
│   │   └── ProductFilter/
│   └── orders/                 # Order components
│       ├── OrderCard/
│       ├── OrderHistory/
│       └── OrderStatus/
├── pages/                      # Page-level components
│   ├── Home/
│   ├── Login/
│   ├── Products/
│   ├── Orders/
│   ├── Dashboard/
│   └── Profile/
├── services/                   # API communication
│   ├── authService.js
│   ├── productService.js
│   ├── orderService.js
│   └── userService.js
├── hooks/                      # Custom React hooks
│   ├── useAuth.js
│   ├── useApi.js
│   └── useLocalStorage.js
├── store/                      # State management
│   ├── authStore.js
│   ├── cartStore.js
│   └── appStore.js
└── utils/                      # Utility functions
    ├── constants.js
    ├── helpers.js
    └── validators.js
```

---

## 🔐 Security Architecture

### Authentication Flow
```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Client    │    │     API     │    │  Database   │
└──────┬──────┘    └──────┬──────┘    └──────┬──────┘
       │                  │                  │
       │ 1. Login Request │                  │
       ├─────────────────▶│                  │
       │                  │ 2. Validate User │
       │                  ├─────────────────▶│
       │                  │ 3. User Data     │
       │                  │◄─────────────────┤
       │ 4. JWT Token     │                  │
       │◄─────────────────┤                  │
       │                  │                  │
       │ 5. API Request   │                  │
       │ + Bearer Token   │                  │
       ├─────────────────▶│                  │
       │                  │ 6. Validate JWT  │
       │                  │                  │
       │ 7. Response      │                  │
       │◄─────────────────┤                  │
```

### Role-Based Authorization
```
┌─────────────────────────────────────────────────┐
│                    Roles                        │
├─────────────────────────────────────────────────┤
│  Admin    │  Employee   │    Customer           │
├───────────┼─────────────┼───────────────────────┤
│ All       │ Products    │ Own Profile           │
│ Users     │ Orders      │ Own Orders            │
│ Products  │ Tickets     │ Product Catalog       │
│ Orders    │ Categories  │ Shopping Cart         │
│ Tickets   │ Drinks      │ Order History         │
│ Analytics │ Users (RO)  │                       │
│ System    │             │                       │
└───────────┴─────────────┴───────────────────────┘
```

---

## 📊 Data Architecture

### Entity Relationships
```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│    User     │     │   Order     │     │  Product    │
│             │────▶│             │◄────│             │
│ - Id        │ 1:n │ - Id        │ n:m │ - Id        │
│ - Name      │     │ - UserId    │     │ - Name      │
│ - Email     │     │ - Status    │     │ - Price     │
│ - Role      │     │ - Total     │     │ - Stock     │
└─────────────┘     └──────┬──────┘     └──────┬──────┘
                           │                   │
                           │ 1:n               │ n:1
                           ▼                   ▼
                    ┌─────────────┐     ┌─────────────┐
                    │  OrderItem  │     │  Category   │
                    │             │     │             │
                    │ - OrderId   │     │ - Id        │
                    │ - ProductId │     │ - Name      │
                    │ - Quantity  │     │ - Desc      │
                    │ - Price     │     │             │
                    └─────────────┘     └─────────────┘
```

### Database Optimization
- **Indexes**: Strategic indexing on frequently queried columns
- **Foreign Keys**: Referential integrity enforcement
- **Transactions**: ACID compliance for critical operations
- **Connection Pooling**: Efficient database connection management

---

## 🚀 Performance Architecture

### Caching Strategy
```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Client    │    │  API Server │    │  Database   │
│             │    │             │    │             │
│ ┌─────────┐ │    │ ┌─────────┐ │    │             │
│ │Browser  │ │    │ │Memory   │ │    │             │
│ │Cache    │ │    │ │Cache    │ │    │             │
│ └─────────┘ │    │ └─────────┘ │    │             │
│             │    │             │    │             │
│ ┌─────────┐ │    │ ┌─────────┐ │    │             │
│ │Local    │ │    │ │Redis    │ │    │             │
│ │Storage  │ │    │ │(Future) │ │    │             │
│ └─────────┘ │    │ └─────────┘ │    │             │
└─────────────┘    └─────────────┘    └─────────────┘
```

### API Performance
- **Async/Await**: Non-blocking operations
- **Pagination**: Large dataset handling
- **Filtering**: Server-side data filtering
- **Compression**: Response compression for faster transfers

---

## 🔄 Deployment Architecture

### Development Environment
```
┌─────────────────────────────────────────────────┐
│              Developer Machine                  │
├─────────────────────────────────────────────────┤
│  Frontend (React)     Backend (.NET 9)         │
│  Port: 5173          Port: 5000                │
│                                                 │
│              Local MySQL                        │
│              Port: 3306                         │
└─────────────────────────────────────────────────┘
```

### Production Environment (Future)
```
┌─────────────────┐    ┌─────────────────┐
│   Load Balancer │    │   Docker Host   │
│     (Nginx)     │    │                 │
└─────────┬───────┘    └─────────┬───────┘
          │                      │
          └──────────────────────┘
                     │
        ┌────────────▼────────────┐
        │      Container 1        │
        │   Frontend + Backend    │
        └─────────────────────────┘
                     │
        ┌────────────▼────────────┐
        │     MySQL Database      │
        │      (Persistent)       │
        └─────────────────────────┘
```

---

## 🧪 Testing Architecture

### Testing Pyramid
```
                    ┌─────────────┐
                    │     E2E     │ <- Few, High Value
                    │   Tests     │
                    └─────────────┘
                ┌─────────────────────┐
                │   Integration       │ <- Some, Key Paths
                │     Tests           │
                └─────────────────────┘
            ┌─────────────────────────────┐
            │        Unit Tests           │ <- Many, Fast
            │   (Controllers, Services)   │
            └─────────────────────────────┘
```

### Test Coverage Goals
- **Unit Tests**: 90%+ coverage
- **Integration Tests**: Key API endpoints
- **E2E Tests**: Critical user journeys
- **Performance Tests**: Load and stress testing

---

## 📈 Monitoring & Observability

### Logging Strategy
```
┌─────────────────────────────────────────────────┐
│                 Logging Levels                  │
├─────────────────────────────────────────────────┤
│ ERROR   │ System errors, exceptions            │
│ WARN    │ Performance issues, deprecations     │
│ INFO    │ User actions, business events        │
│ DEBUG   │ Development debugging (dev only)     │
└─────────────────────────────────────────────────┘
```

### Health Checks
- **Database Connectivity**: MySQL connection status
- **API Responsiveness**: Endpoint response times
- **Memory Usage**: Application memory consumption
- **Disk Space**: Available storage monitoring

---

## 🔮 Future Architecture Considerations

### Scalability Improvements
- **Microservices**: Break monolith into focused services
- **Message Queues**: Async processing with RabbitMQ/Azure Service Bus
- **Caching Layer**: Redis for distributed caching
- **CDN**: Static asset delivery optimization

### Technology Upgrades
- **GraphQL**: More efficient data fetching
- **SignalR**: Real-time notifications
- **Docker**: Containerization for easier deployment
- **Kubernetes**: Container orchestration

### Security Enhancements
- **OAuth 2.0**: Third-party authentication
- **HTTPS**: End-to-end encryption
- **API Rate Limiting**: DDoS protection
- **Audit Logging**: Comprehensive security tracking

---

*This architecture document reflects the current state after the October 2025 .NET 9 upgrade and feature enhancements.*
