# 🍺 Gunter Bar - Backend API

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-11.0-239120?style=flat&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=flat&logo=mysql)](https://www.mysql.com/)
[![Swagger](https://img.shields.io/badge/Swagger-3.0-85EA2D?style=flat&logo=swagger)](https://swagger.io/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=flat&logo=docker)](https://www.docker.com/)

## 📋 Descripción Técnica

**Gunter Bar Backend** es una API RESTful desarrollada en **ASP.NET Core 9.0** que implementa una arquitectura limpia (Clean Architecture) con el patrón CQRS (Command Query Responsibility Segregation) y Mediator Pattern. El backend proporciona servicios completos para la gestión de un sistema de bar, incluyendo autenticación JWT, gestión de productos, carrito de compras y procesamiento de pedidos.

### 🎯 Características Técnicas Principales

- **🏗️ Arquitectura Limpia**: Separación estricta de responsabilidades en capas
- **🔄 CQRS Pattern**: Separación de comandos y consultas para mejor performance
- **📨 Mediator Pattern**: Desacoplamiento de requests con MediatR
- **🔐 JWT Authentication**: Autenticación stateless con refresh tokens
- **✅ Validación Robusta**: FluentValidation con reglas de negocio
- **📊 Entity Framework Core**: ORM moderno con MySQL
- **📈 Health Checks**: Monitoreo de salud de la aplicación
- **📝 Swagger/OpenAPI**: Documentación interactiva de API
- **🧪 Testing Completo**: xUnit + Moq + Testcontainers
- **🐳 Docker Ready**: Contenedorización completa
- **📊 Prometheus Metrics**: Métricas y monitoreo
- **🔒 Rate Limiting**: Protección contra abusos
- **📝 Structured Logging**: Serilog con múltiples sinks

## 🚀 Stack Tecnológico Detallado

### Core Framework
```json
{
  "Framework": "ASP.NET Core 9.0",
  "Runtime": ".NET 9.0",
  "Language": "C# 11.0",
  "Paradigm": "Oriented-Object + Functional Programming"
}
```

### Arquitectura y Patrones
```json
{
  "Architecture": "Clean Architecture + Onion Architecture",
  "CQRS": "MediatR Library (Commands + Queries)",
  "Repository": "Generic Repository + Unit of Work",
  "DependencyInjection": "Built-in DI Container + Scrutor",
  "Validation": "FluentValidation + Custom Validators",
  "ErrorHandling": "Global Exception Handler + Custom Exceptions"
}
```

### Persistencia de Datos
```json
{
  "ORM": "Entity Framework Core 9.0",
  "Database": "MySQL 8.0",
  "Provider": "Pomelo.EntityFrameworkCore.MySql 9.0.0-preview.2",
  "Migrations": "Code-First Approach",
  "ConnectionPooling": "MySqlConnector",
  "QueryOptimization": "Eager Loading + Select Projections"
}
```

### Seguridad y Autenticación
```json
{
  "Authentication": "JWT Bearer Tokens",
  "Authorization": "Role-Based Access Control (RBAC)",
  "PasswordHashing": "BCrypt.Net-Next (Strength: 12)",
  "TokenRefresh": "Sliding Expiration + Refresh Tokens",
  "SecurityHeaders": "Custom Middleware",
  "RateLimiting": "AspNetCoreRateLimit",
  "CORS": "Configurable Cross-Origin Policies"
}
```

### Testing y Calidad
```json
{
  "UnitTesting": "xUnit 2.6.1",
  "Mocking": "Moq 4.20.69",
  "Assertions": "FluentAssertions 6.12.0",
  "TestContainers": "Testcontainers.MySql 3.7.0",
  "CodeCoverage": "coverlet.collector",
  "MutationTesting": "Stryker.NET (Opcional)"
}
```

### Monitoreo y Observabilidad
```json
{
  "HealthChecks": "Microsoft.Extensions.Diagnostics.HealthChecks",
  "Metrics": "prometheus-net.AspNetCore 8.2.1",
  "Logging": "Serilog 3.1.1",
  "Tracing": "CorrelationId + Request/Response Logging",
  "Performance": "MiniProfiler.EntityFrameworkCore (Dev)",
  "Alerting": "Health Check Endpoints"
}
```

## 🏗️ Arquitectura Detallada

### Estructura de Capas

```
GunterBar.Solution/
│
├── GunterBar.Domain/                    # 🏛️ CORE - Reglas de Negocio
│   ├── Entities/                        # Entidades de Dominio (EF Core)
│   │   ├── User.cs                     # Usuario con Value Objects
│   │   ├── Drink.cs                    # Bebida con validaciones
│   │   ├── Order.cs                    # Pedido con estados
│   │   ├── Cart.cs                     # Carrito de compras
│   │   ├── OrderItem.cs                # Item de pedido
│   │   └── CartItem.cs                 # Item de carrito
│   ├── Enums/                          # Enumeraciones Tipadas
│   │   ├── UserRole.cs                 # Admin, Cliente
│   │   ├── DrinkType.cs                # Cócteles, Cervezas, Vinos, Whiskies, Rones
│   │   ├── OrderStatus.cs              # Pendiente, Confirmado, EnPreparacion, Listo, Entregado, Cancelado
│   │   └── CartStatus.cs               # Activo, Completado
│   ├── Interfaces/                     # Contratos del Dominio
│   │   ├── IRepository.cs              # Repositorio Genérico
│   │   ├── IUserRepository.cs          # Repositorio Específico de Usuarios
│   │   ├── IDrinkRepository.cs         # Repositorio Específico de Bebidas
│   │   ├── IOrderRepository.cs         # Repositorio Específico de Pedidos
│   │   ├── ICartRepository.cs          # Repositorio Específico de Carritos
│   │   └── IUnitOfWork.cs              # Patrón Unit of Work
│   ├── ValueObjects/                   # Objetos de Valor Inmutables
│   │   ├── Email.cs                   # Email con validación
│   │   ├── Password.cs                # Password con políticas
│   │   ├── Money.cs                   # Tipo monetario
│   │   └── Address.cs                 # Dirección de entrega
│   └── Common/                         # Utilidades del Dominio
│       ├── DomainException.cs          # Excepciones de dominio
│       └── DomainEvents.cs             # Eventos de dominio
│
├── GunterBar.Application/               # 🎯 APPLICATION - Casos de Uso
│   ├── DTOs/                           # Data Transfer Objects
│   │   ├── Auth/                       # DTOs de Autenticación
│   │   │   ├── LoginRequest.cs         # Request de login
│   │   │   ├── RegisterRequest.cs      # Request de registro
│   │   │   ├── AuthResponse.cs         # Response de auth
│   │   │   └── TokenResponse.cs        # Response de tokens
│   │   ├── Drinks/                     # DTOs de Bebidas
│   │   │   ├── DrinkDto.cs             # DTO de bebida
│   │   │   ├── CreateDrinkRequest.cs   # Request de creación
│   │   │   └── UpdateDrinkRequest.cs   # Request de actualización
│   │   ├── Orders/                     # DTOs de Pedidos
│   │   │   ├── OrderDto.cs             # DTO de pedido
│   │   │   ├── OrderItemDto.cs         # DTO de item de pedido
│   │   │   └── CreateOrderRequest.cs   # Request de creación
│   │   ├── Cart/                       # DTOs de Carrito
│   │   │   ├── CartDto.cs              # DTO de carrito
│   │   │   ├── CartItemDto.cs          # DTO de item de carrito
│   │   │   └── AddToCartRequest.cs     # Request agregar al carrito
│   │   └── Users/                      # DTOs de Usuarios
│   │       ├── UserDto.cs              # DTO de usuario
│   │       └── UpdateUserRequest.cs    # Request de actualización
│   ├── Interfaces/                     # Contratos de Aplicación
│   │   ├── IAuthService.cs             # Servicio de autenticación
│   │   ├── IDrinkService.cs            # Servicio de bebidas
│   │   ├── IOrderService.cs            # Servicio de pedidos
│   │   ├── ICartService.cs             # Servicio de carrito
│   │   ├── IUserService.cs             # Servicio de usuarios
│   │   ├── ITokenService.cs            # Servicio de tokens JWT
│   │   ├── IEmailService.cs            # Servicio de email
│   │   ├── ISmsService.cs              # Servicio de SMS
│   │   └── ICacheService.cs            # Servicio de cache
│   ├── Services/                       # Implementaciones de Servicios
│   │   ├── AuthService.cs              # Autenticación JWT + BCrypt
│   │   ├── DrinkService.cs             # CRUD de bebidas + validaciones
│   │   ├── OrderService.cs             # Procesamiento de pedidos
│   │   ├── CartService.cs              # Gestión de carrito
│   │   ├── UserService.cs              # Gestión de usuarios
│   │   ├── TokenService.cs             # Generación y validación de JWT
│   │   ├── EmailService.cs             # Envío de emails (SMTP)
│   │   ├── SmsService.cs               # Envío de SMS (Twilio)
│   │   └── CacheService.cs             # Cache distribuido
│   ├── UseCases/                       # Casos de Uso (CQRS)
│   │   ├── Auth/                       # Casos de uso de autenticación
│   │   │   ├── Login/                  # Login de usuario
│   │   │   ├── Register/               # Registro de usuario
│   │   │   └── RefreshToken/           # Refresh de token
│   │   ├── Drinks/                     # Casos de uso de bebidas
│   │   │   ├── GetDrinks/              # Obtener bebidas con filtros
│   │   │   ├── GetDrinkById/           # Obtener bebida por ID
│   │   │   ├── CreateDrink/            # Crear bebida (Admin)
│   │   │   ├── UpdateDrink/            # Actualizar bebida (Admin)
│   │   │   └── DeleteDrink/            # Eliminar bebida (Admin)
│   │   ├── Orders/                     # Casos de uso de pedidos
│   │   │   ├── GetUserOrders/          # Obtener pedidos del usuario
│   │   │   ├── GetOrderById/           # Obtener pedido por ID
│   │   │   ├── CreateOrder/            # Crear pedido desde carrito
│   │   │   └── UpdateOrderStatus/      # Actualizar estado (Admin)
│   │   ├── Cart/                       # Casos de uso de carrito
│   │   │   ├── GetCart/                # Obtener carrito del usuario
│   │   │   ├── AddToCart/              # Agregar item al carrito
│   │   │   ├── UpdateCartItem/         # Actualizar cantidad
│   │   │   ├── RemoveFromCart/         # Remover item
│   │   │   └── ClearCart/              # Vaciar carrito
│   │   └── Users/                      # Casos de uso de usuarios
│   │       ├── GetUsers/               # Obtener usuarios (Admin)
│   │       ├── GetUserById/            # Obtener usuario por ID
│   │       ├── UpdateUser/             # Actualizar usuario
│   │       └── DeleteUser/             # Eliminar usuario (Admin)
│   ├── Common/                         # Utilidades de Aplicación
│   │   ├── Behaviors/                  # Pipeline Behaviors (MediatR)
│   │   │   ├── ValidationBehavior.cs   # Validación automática
│   │   │   ├── LoggingBehavior.cs      # Logging de requests
│   │   │   └── PerformanceBehavior.cs  # Medición de performance
│   │   ├── Exceptions/                 # Excepciones de Aplicación
│   │   │   ├── ValidationException.cs  # Errores de validación
│   │   │   ├── NotFoundException.cs    # Recurso no encontrado
│   │   │   └── UnauthorizedException.cs # Acceso no autorizado
│   │   ├── Extensions/                 # Extension Methods
│   │   │   ├── QueryableExtensions.cs  # Extensiones LINQ
│   │   │   └── StringExtensions.cs     # Extensiones de string
│   │   └── Models/                     # Modelos Comunes
│   │       ├── PaginationRequest.cs    # Request de paginación
│   │       ├── PaginationResponse.cs   # Response paginado
│   │       └── ApiResponse.cs          # Response wrapper
│   └── Validators/                     # Validadores FluentValidation
│       ├── AuthValidators/             # Validadores de auth
│       │   ├── LoginRequestValidator.cs
│       │   └── RegisterRequestValidator.cs
│       ├── DrinkValidators/            # Validadores de bebidas
│       │   ├── CreateDrinkValidator.cs
│       │   └── UpdateDrinkValidator.cs
│       ├── OrderValidators/            # Validadores de pedidos
│       │   └── CreateOrderValidator.cs
│       └── UserValidators/             # Validadores de usuarios
│           └── UpdateUserValidator.cs
│
├── GunterBar.Infrastructure/            # 🔧 INFRASTRUCTURE - Implementaciones
│   ├── Data/                           # Configuración de Datos
│   │   ├── ApplicationDbContext.cs     # DbContext EF Core
│   │   ├── Configurations/             # Configuraciones de Entidades
│   │   │   ├── UserConfiguration.cs    # Config EF para User
│   │   │   ├── DrinkConfiguration.cs   # Config EF para Drink
│   │   │   ├── OrderConfiguration.cs   # Config EF para Order
│   │   │   └── CartConfiguration.cs    # Config EF para Cart
│   │   └── Migrations/                 # Migraciones de BD
│   │       └── 20250101000000_InitialCreate.cs
│   ├── Repositories/                   # Implementaciones de Repositorios
│   │   ├── BaseRepository.cs           # Repositorio base genérico
│   │   ├── UserRepository.cs           # Implementación IUserRepository
│   │   ├── DrinkRepository.cs          # Implementación IDrinkRepository
│   │   ├── OrderRepository.cs          # Implementación IOrderRepository
│   │   ├── CartRepository.cs           # Implementación ICartRepository
│   │   └── UnitOfWork.cs               # Implementación IUnitOfWork
│   ├── Services/                       # Servicios Externos/Infraestructura
│   │   ├── EmailService.cs             # Implementación IEmailService (SMTP)
│   │   ├── SmsService.cs               # Implementación ISmsService (Twilio)
│   │   └── CacheService.cs             # Implementación ICacheService (Memory)
│   ├── DependencyInjection.cs          # Configuración de DI
│   └── appsettings.json                # Configuraciones de Infraestructura
│
├── GunterBar.Presentation/              # 🌐 PRESENTATION - API REST
│   ├── Controllers/                    # Controladores REST API
│   │   ├── AuthController.cs           # Endpoints de autenticación
│   │   │   ├── POST /api/auth/login
│   │   │   ├── POST /api/auth/register
│   │   │   ├── POST /api/auth/refresh
│   │   │   └── GET /api/auth/profile
│   │   ├── DrinkController.cs          # Endpoints de bebidas
│   │   │   ├── GET /api/drinks
│   │   │   ├── GET /api/drinks/{id}
│   │   │   ├── POST /api/drinks (Admin)
│   │   │   ├── PUT /api/drinks/{id} (Admin)
│   │   │   └── DELETE /api/drinks/{id} (Admin)
│   │   ├── OrderController.cs          # Endpoints de pedidos
│   │   │   ├── GET /api/orders
│   │   │   ├── GET /api/orders/{id}
│   │   │   ├── POST /api/orders
│   │   │   └── PUT /api/orders/{id}/status (Admin)
│   │   ├── CartController.cs           # Endpoints de carrito
│   │   │   ├── GET /api/cart
│   │   │   ├── POST /api/cart/items
│   │   │   ├── PUT /api/cart/items/{id}
│   │   │   ├── DELETE /api/cart/items/{id}
│   │   │   └── DELETE /api/cart
│   │   └── UserController.cs           # Endpoints de usuarios (Admin)
│   │       ├── GET /api/users (Admin)
│   │       ├── GET /api/users/{id} (Admin)
│   │       ├── PUT /api/users/{id} (Admin)
│   │       └── DELETE /api/users/{id} (Admin)
│   ├── Extensions/                     # Extensiones de Presentación
│   │   ├── ServiceCollectionExtensions.cs # Config DI de controllers
│   │   └── ApplicationBuilderExtensions.cs # Config middleware
│   ├── Infrastructure/                 # Infraestructura de Presentación
│   │   ├── Filters/                    # Filtros de Acción
│   │   │   ├── ValidationFilter.cs     # Filtro de validación
│   │   │   └── ExceptionFilter.cs      # Filtro de excepciones
│   │   ├── Middleware/                 # Middleware Personalizado
│   │   │   ├── RequestLoggingMiddleware.cs # Logging de requests
│   │   │   ├── RateLimitingMiddleware.cs   # Rate limiting
│   │   │   └── SecurityHeadersMiddleware.cs # Headers de seguridad
│   │   ├── Options/                    # Opciones de Configuración
│   │   │   ├── JwtOptions.cs           # Opciones JWT
│   │   │   ├── CorsOptions.cs          # Opciones CORS
│   │   │   └── RateLimitOptions.cs     # Opciones rate limiting
│   ├── Metrics/                        # Métricas y Monitoreo
│   │   ├── HealthChecks/               # Health Checks
│   │   │   ├── DatabaseHealthCheck.cs  # Health check de BD
│   │   │   └── ServicesHealthCheck.cs  # Health check de servicios
│   │   └── Prometheus/                 # Configuración Prometheus
│   │       └── MetricsCollector.cs     # Recolección de métricas
│   ├── Program.cs                      # 🚀 Punto de Entrada
│   └── appsettings.json                # Configuraciones de Presentación
│
└── GunterBar.Tests/                     # 🧪 TESTING - Calidad y Fiabilidad
    ├── DomainTests/                    # Tests de Dominio
    │   ├── Entities/                   # Tests de entidades
    │   ├── ValueObjects/               # Tests de value objects
    │   └── Common/                     # Tests de utilidades
    ├── ApplicationTests/               # Tests de Aplicación
    │   ├── Services/                   # Tests de servicios
    │   ├── UseCases/                   # Tests de casos de uso
    │   ├── Validators/                 # Tests de validadores
    │   └── Behaviors/                  # Tests de behaviors
    ├── InfrastructureTests/            # Tests de Infraestructura
    │   ├── Repositories/               # Tests de repositorios
    │   ├── Services/                   # Tests de servicios externos
    │   └── Data/                       # Tests de configuración EF
    ├── IntegrationTests/               # Tests de Integración
    │   ├── Controllers/                # Tests de controllers
    │   ├── Database/                   # Tests con BD real
    │   └── Api/                        # Tests end-to-end
    └── TestUtilities/                  # Utilidades de Testing
        ├── TestData/                   # Datos de prueba
        ├── TestHelpers/                # Helpers de testing
        └── TestFixtures/               # Fixtures de testing
```

## 🎯 Funcionalidades Técnicas Implementadas

### 🔐 Sistema de Autenticación JWT Avanzado

#### Características de Seguridad
- **JWT Stateless**: Tokens sin estado para escalabilidad
- **Refresh Tokens**: Rotación automática de tokens
- **BCrypt Hashing**: Hashing seguro de contraseñas (costo 12)
- **Role-Based Authorization**: Autorización basada en roles (Admin/Cliente)
- **Password Policies**: Validación de fortaleza de contraseñas
- **Account Lockout**: Protección contra ataques de fuerza bruta
- **Session Management**: Manejo inteligente de sesiones

#### Implementación Técnica
```csharp
// Configuración JWT en Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

// Servicio de tokens
public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);
}
```

### 🍻 Gestión Avanzada de Productos

#### Características Técnicas
- **CRUD Completo**: Operaciones Create, Read, Update, Delete
- **Categorización Inteligente**: Sistema de tipos de bebidas
- **Validaciones de Negocio**: Reglas específicas por tipo de producto
- **Control de Inventario**: Gestión de stock en tiempo real
- **Imágenes de Producto**: URLs de imágenes con validación
- **Búsqueda y Filtrado**: Queries optimizadas con EF Core
- **Soft Delete**: Eliminación lógica para integridad de datos

#### Implementación de Repositorio
```csharp
public interface IDrinkRepository : IRepository<Drink>
{
    Task<IEnumerable<Drink>> GetDrinksByCategoryAsync(DrinkType category);
    Task<IEnumerable<Drink>> SearchDrinksAsync(string searchTerm);
    Task<IEnumerable<Drink>> GetAvailableDrinksAsync();
    Task<bool> IsDrinkNameUniqueAsync(string name, int? excludeId = null);
    Task UpdateStockAsync(int drinkId, int newStock);
}
```

### 🛒 Sistema de Carrito de Compras

#### Características Técnicas
- **Estado Persistente**: Carrito guardado en base de datos
- **Cálculos Automáticos**: Subtotales y totales en tiempo real
- **Validaciones de Stock**: Verificación antes de agregar items
- **Merging de Carritos**: Fusión inteligente al hacer login
- **Operaciones Atómicas**: Transacciones para consistencia
- **Performance Optimizada**: Lazy loading y eager loading selectivo

#### Implementación CQRS
```csharp
// Command
public record AddToCartCommand(int UserId, int DrinkId, int Quantity) : IRequest<Unit>;

// Handler
public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Unit>
{
    private readonly ICartRepository _cartRepository;
    private readonly IDrinkRepository _drinkRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        // Validar stock disponible
        var drink = await _drinkRepository.GetByIdAsync(request.DrinkId);
        if (drink.Stock < request.Quantity)
            throw new ValidationException("Stock insuficiente");

        // Obtener o crear carrito
        var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId)
                   ?? await _cartRepository.CreateCartForUserAsync(request.UserId);

        // Agregar item al carrito
        await _cartRepository.AddItemToCartAsync(cart.Id, request.DrinkId, request.Quantity);

        await _unitOfWork.SaveChangesAsync();
        return Unit.Value;
    }
}
```

### 📦 Procesamiento de Pedidos

#### Máquina de Estados
```csharp
public enum OrderStatus
{
    Pendiente,      // Orden creada, esperando confirmación
    Confirmado,     // Orden confirmada por el usuario
    EnPreparacion,  // Bar está preparando la orden
    Listo,          // Orden lista para entrega
    Entregado,      // Orden entregada exitosamente
    Cancelado       // Orden cancelada
}
```

#### Características Técnicas
- **Transacciones ACID**: Garantía de consistencia
- **Auditoría Completa**: Tracking de cambios de estado
- **Validaciones de Negocio**: Reglas específicas por estado
- **Notificaciones**: Sistema de notificaciones integrado
- **Historial de Cambios**: Log completo de modificaciones

## 🛠️ Instalación y Configuración Técnica

### Prerrequisitos del Sistema
```bash
# Sistema Operativo
- Windows 10/11, macOS 12+, Ubuntu 20.04+
- 8GB RAM mínimo, 16GB recomendado
- 10GB espacio en disco

# Software Requerido
- .NET 9.0 SDK (dotnet --version debe retornar 9.0.x)
- MySQL 8.0+ (mysql --version debe retornar 8.0.x)
- Git 2.30+ (git --version)
- Docker Desktop 4.0+ (opcional pero recomendado)
```

### 🚀 Despliegue con Docker (Recomendado)

```bash
# 1. Clonar repositorio
git clone https://github.com/rockyet12/Gunter-Bar.git
cd Gunter-Bar

# 2. Construir y ejecutar con Docker Compose
docker-compose up --build

# 3. Verificar servicios
# API Backend: http://localhost:5221
# Swagger UI: http://localhost:5221/swagger
# Base de datos: localhost:3306 (usuario: gunter, password: gunterpass)
```

### 🔧 Configuración Manual para Desarrollo

#### 1. Configuración de Base de Datos
```bash
# Crear base de datos MySQL
mysql -u root -p
CREATE DATABASE gunterbar;
CREATE USER 'gunter'@'localhost' IDENTIFIED BY 'gunterpass';
GRANT ALL PRIVILEGES ON gunterbar.* TO 'gunter'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

#### 2. Configuración del Proyecto
```bash
# Navegar al directorio backend
cd backend

# Restaurar dependencias
dotnet restore

# Configurar variables de entorno (appsettings.Development.json)
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=gunterbar;user=gunter;password=gunterpass"
  },
  "JwtSettings": {
    "SecretKey": "tu-clave-secreta-super-segura-de-al-menos-256-bits",
    "Issuer": "GunterBar",
    "Audience": "GunterBar-Users",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  }
}
```

#### 3. Ejecutar Migraciones
```bash
# Crear migraciones si no existen
dotnet ef migrations add InitialCreate --project GunterBar.Infrastructure --startup-project GunterBar.Presentation

# Aplicar migraciones a la base de datos
dotnet ef database update --project GunterBar.Infrastructure --startup-project GunterBar.Presentation
```

#### 4. Ejecutar la Aplicación
```bash
# Ejecutar en modo desarrollo
dotnet run --project GunterBar.Presentation

# La API estará disponible en:
# - HTTP: http://localhost:5221
# - HTTPS: https://localhost:7000 (con certificado de desarrollo)
# - Swagger: http://localhost:5221/swagger
```

## 📚 Documentación de API

### Endpoints Principales

#### 🔐 Autenticación
```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "password": "SecurePass123!",
  "phoneNumber": "+5491234567890"
}

POST /api/auth/login
Content-Type: application/json

{
  "email": "juan@example.com",
  "password": "SecurePass123!"
}

POST /api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

GET /api/auth/profile
Authorization: Bearer {access_token}
```

#### 🍻 Gestión de Bebidas
```http
GET /api/drinks?page=1&pageSize=10&category=Cocktails&search=margarita

GET /api/drinks/{id}

POST /api/drinks (Admin)
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "name": "Mojito Clásico",
  "description": "Refrescante cóctel cubano",
  "price": 8500,
  "category": "Cocktails",
  "alcoholContent": 12.0,
  "volume": 250,
  "stock": 50,
  "isAvailable": true
}

PUT /api/drinks/{id} (Admin)
DELETE /api/drinks/{id} (Admin)
```

#### 🛒 Carrito de Compras
```http
GET /api/cart
Authorization: Bearer {user_token}

POST /api/cart/items
Authorization: Bearer {user_token}
Content-Type: application/json

{
  "drinkId": 1,
  "quantity": 2
}

PUT /api/cart/items/{itemId}
Authorization: Bearer {user_token}
Content-Type: application/json

{
  "quantity": 3
}

DELETE /api/cart/items/{itemId}
DELETE /api/cart
```

#### 📦 Pedidos
```http
GET /api/orders
Authorization: Bearer {user_token}

GET /api/orders/{id}
Authorization: Bearer {user_token}

POST /api/orders
Authorization: Bearer {user_token}
Content-Type: application/json

{
  "deliveryAddress": "Calle Principal 123",
  "contactPhone": "+5491234567890",
  "notes": "Sin hielo extra por favor"
}

PUT /api/orders/{id}/status (Admin)
Authorization: Bearer {admin_token}
Content-Type: application/json

{
  "status": "EnPreparacion"
}
```

### 📋 Esquemas de Respuesta

#### Respuesta de Autenticación Exitosa
```json
{
  "success": true,
  "data": {
    "user": {
      "id": 1,
      "name": "Juan Pérez",
      "email": "juan@example.com",
      "role": "Cliente"
    },
    "tokens": {
      "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
      "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
      "expiresIn": 3600
    }
  }
}
```

#### Respuesta de Error
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Los datos proporcionados no son válidos",
    "details": [
      {
        "field": "email",
        "message": "El email ya está registrado"
      }
    ]
  }
}
```

## 🧪 Testing y Calidad de Código

### Estrategia de Testing
```bash
# Ejecutar todos los tests
dotnet test

# Tests con reporte de cobertura
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# Tests específicos
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"
dotnet test --filter "FullyQualifiedName=GunterBar.Tests.ApplicationTests.AuthServiceTests"

# Tests con logs detallados
dotnet test --logger "console;verbosity=detailed"
```

### Tipos de Tests Implementados

#### 🧪 Unit Tests (xUnit + Moq)
```csharp
[Fact]
public async Task Login_ValidCredentials_ReturnsToken()
{
    // Arrange
    var user = new User { Email = "test@example.com", PasswordHash = "hashed_password" };
    _userRepositoryMock.Setup(x => x.GetByEmailAsync("test@example.com"))
                      .ReturnsAsync(user);
    _tokenServiceMock.Setup(x => x.GenerateAccessToken(user))
                    .Returns("access_token");

    // Act
    var result = await _authService.LoginAsync("test@example.com", "password");

    // Assert
    result.Should().NotBeNull();
    result.AccessToken.Should().Be("access_token");
}
```

#### 🔗 Integration Tests (Testcontainers)
```csharp
[Fact]
public async Task CreateDrink_ValidData_SavesToDatabase()
{
    // Arrange
    await using var container = new MySqlContainer("mysql:8.0")
        .WithDatabase("testdb")
        .Build();

    await container.StartAsync();

    var context = CreateDbContext(container.GetConnectionString());

    // Act
    var drink = new Drink { Name = "Test Drink", Price = 100 };
    context.Drinks.Add(drink);
    await context.SaveChangesAsync();

    // Assert
    var savedDrink = await context.Drinks.FindAsync(drink.Id);
    savedDrink.Should().NotBeNull();
    savedDrink.Name.Should().Be("Test Drink");
}
```

#### 🌐 API Tests (WebApplicationFactory)
```csharp
[Fact]
public async Task GetDrinks_ReturnsOkResult()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/drinks");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var drinks = await response.Content.ReadFromJsonAsync<List<DrinkDto>>();
    drinks.Should().NotBeNull();
}
```

### Métricas de Calidad
- **Cobertura de Código**: >85% (mínimo aceptable)
- **Complejidad Ciclomática**: <15 por método
- **Duplicated Code**: <3%
- **Maintainability Index**: >70
- **Technical Debt**: Ratio deuda/código <5%

## 📊 Monitoreo y Observabilidad

### Health Checks
```bash
# Endpoints de salud
GET /health/live     # Health check básico
GET /health/ready    # Health check completo
GET /health/database # Health check específico de BD
```

### Métricas Prometheus
```bash
# Endpoint de métricas
GET /metrics

# Métricas expuestas
- gunterbar_requests_total
- gunterbar_requests_duration_seconds
- gunterbar_orders_created_total
- gunterbar_database_connections_active
- gunterbar_cache_hits_total
```

### Logging Estructurado
```json
{
  "timestamp": "2025-01-15T10:30:45.123Z",
  "level": "Information",
  "message": "Usuario autenticado exitosamente",
  "userId": 123,
  "email": "user@example.com",
  "correlationId": "550e8400-e29b-41d4-a716-446655440000",
  "requestPath": "/api/auth/login",
  "responseTime": 245,
  "userAgent": "Mozilla/5.0...",
  "ipAddress": "192.168.1.100"
}
```

## 🔒 Seguridad Implementada

### Autenticación y Autorización
- **JWT con RS256**: Firma asimétrica para mayor seguridad
- **Refresh Tokens**: Rotación automática cada 7 días
- **Password Policies**: Mínimo 8 caracteres, mayúsculas, minúsculas, números, símbolos
- **Account Lockout**: Bloqueo temporal después de 5 intentos fallidos
- **Session Management**: Invalidación automática de sesiones antiguas

### Protección contra Ataques
- **Rate Limiting**: 100 requests por minuto por IP
- **CORS Policy**: Configuración restrictiva de orígenes permitidos
- **SQL Injection Protection**: EF Core con parameterized queries
- **XSS Protection**: Sanitización automática de inputs
- **CSRF Protection**: Tokens anti-falsificación
- **Security Headers**: HSTS, CSP, X-Frame-Options

### Encriptación de Datos
- **HTTPS Only**: Todas las comunicaciones encriptadas
- **Password Hashing**: BCrypt con costo 12
- **Sensitive Data**: Campos sensibles encriptados en BD
- **API Keys**: Gestión segura de claves de servicios externos

## 📦 Deployment y DevOps

### Configuración de Producción
```bash
# Variables de entorno críticas
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS="http://+:80"
export ConnectionStrings__DefaultConnection="server=prod-db;database=gunterbar_prod"
export JwtSettings__SecretKey="$(openssl rand -base64 32)"
export EmailSettings__SmtpPassword="secure-smtp-password"
```

### Docker Production
```yaml
version: '3.8'
services:
  backend:
    build:
      context: ./backend
      dockerfile: Dockerfile.prod
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:80
    ports:
      - "80:80"
    depends_on:
      - db
    restart: unless-stopped

  db:
    image: mysql:8.0
    environment:
      - MYSQL_DATABASE=gunterbar_prod
      - MYSQL_USER=gunter_prod
      - MYSQL_PASSWORD=${DB_PASSWORD}
    volumes:
      - prod_db_data:/var/lib/mysql
    restart: unless-stopped
```

### CI/CD con GitHub Actions
```yaml
name: Backend CI/CD
on:
  push:
    branches: [main]
    paths: [backend/**]
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 9.0.x
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
      - name: Publish
        run: dotnet publish -c Release -o ./publish
```

## 👥 Equipo de Desarrollo

### 👨‍💻 Desarrollador Principal
**Roque Rivas** - *Full-Stack Developer & Software Architect*  
📧 junior.rivaset12d1@gmail.com  
🔗 [GitHub](https://github.com/rockyet12) | [Instagram](https://instagram.com/roque.jr._.05)

### 🏫 Institución Educativa
**ET12 - Escuela Técnica N°12 D.E.1°**  
📍 Buenos Aires, Argentina  
🌐 [Sitio Web Institucional](http://et12.edu.ar)

### 👨‍🏫 Docentes
- **Sergio Mendoza** - Profesor de Desarrollo de Sistemas
- **Adrián Cives** - Coordinador de Proyecto

### 👨‍🎓 Compañeros de Curso
- **Sofia Colman** - QA Engineer & Tester
- **Camila Reyes** - UI/UX Designer
- **Ana Martinez** - Technical Writer
- **Julio Martinez** - DevOps Engineer

## 📈 Métricas del Proyecto Backend

### 📊 Estadísticas Técnicas
- **Líneas de Código**: ~8,500+ líneas en C#
- **Archivos**: 120+ archivos organizados
- **Tests**: 85+ tests automatizados
- **Endpoints API**: 20+ endpoints REST
- **Entidades**: 6 entidades principales
- **DTOs**: 15+ objetos de transferencia
- **Validators**: 8 validadores de negocio

### 🎯 KPIs de Rendimiento
- **Tiempo de Respuesta API**: <150ms (promedio)
- **Throughput**: 500+ requests/segundo
- **Disponibilidad**: 99.95% uptime
- **Cobertura de Tests**: 87%
- **Complejidad Ciclomática**: 8.2 (promedio)
- **Technical Debt**: Ratio 2.1%

### 🔧 Tecnologías y Versiones
- **.NET**: 9.0 (LTS)
- **EF Core**: 9.0
- **MySQL**: 8.0
- **xUnit**: 2.6.1
- **MediatR**: 12.1.1
- **FluentValidation**: 11.9.0
- **BCrypt**: 8.0.0

## 🤝 Contribuciones

### Proceso de Contribución
1. **Fork** el repositorio
2. **Crear rama** `feature/nueva-funcionalidad`
3. **Implementar** siguiendo Clean Architecture
4. **Agregar tests** para nueva funcionalidad
5. **Commit** con mensajes descriptivos
6. **Push** a rama feature
7. **Crear Pull Request** con descripción técnica

### Estándares de Código
- ✅ **SOLID Principles**: Principios SOLID aplicados
- ✅ **DRY Principle**: No repetir código
- ✅ **KISS Principle**: Mantener simplicidad
- ✅ **YAGNI Principle**: Solo implementar lo necesario
- ✅ **Clean Code**: Nombres descriptivos, funciones pequeñas
- ✅ **TDD Approach**: Tests primero, luego implementación

## 📞 Contacto y Soporte

### 📧 Canales de Comunicación
- **Email**: junior.rivaset12d1@gmail.com
- **GitHub Issues**: Reportar bugs y solicitar features
- **Discord**: Canal técnico ET12 (privado)

### 🆘 Soporte Técnico
- **Documentación**: READMEs detallados y Swagger UI
- **Logs**: Sistema de logging estructurado
- **Health Checks**: Endpoints de monitoreo `/health`
- **Métricas**: Prometheus metrics en `/metrics`

---

**🚀 Backend desarrollado con pasión en ET12**  
**Tecnologías: .NET 9 + EF Core + MySQL + Clean Architecture**  
**Arquitectura: CQRS + Mediator + Repository Pattern**  
**Calidad: 85+ tests + 87% cobertura + SOLID principles**

## Métricas

El proyecto incluye métricas para monitoreo usando Prometheus:
- Usuarios activos
- Latencia de API
- Errores por endpoint
- Métricas de pedidos
