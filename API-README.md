# 🔌 Gunter Bar API - RESTful Documentation

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI_3.0-85EA2D?style=flat&logo=swagger)](https://swagger.io/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=flat&logo=json-web-tokens)](https://jwt.io/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=flat&logo=mysql)](https://www.mysql.com/)

## 📋 Descripción Técnica

**Gunter Bar API** es una interfaz RESTful completa construida con ASP.NET Core 9.0 que implementa Clean Architecture y CQRS para gestionar operaciones de un sistema de bar. Proporciona endpoints seguros para autenticación, gestión de usuarios, productos, pedidos y carrito de compras.

### 🎯 Características Técnicas de la API

- **🏛️ Clean Architecture**: Separación clara de responsabilidades
- **⚡ CQRS Pattern**: Commands y Queries separados
- **🔐 JWT Authentication**: Autenticación stateless con refresh tokens
- **📊 Swagger/OpenAPI**: Documentación interactiva automática
- **🛡️ Validation**: Validación robusta con FluentValidation
- **📈 Metrics**: Monitoreo con Prometheus
- **🔄 Caching**: Redis para optimización de rendimiento
- **📧 Notifications**: Email y SMS integrados

## 🚀 Stack Tecnológico API

### Framework y Arquitectura
```json
{
  "Framework": "ASP.NET Core 9.0",
  "Architecture": "Clean Architecture + CQRS",
  "ORM": "Entity Framework Core 9.0",
  "Database": "MySQL 8.0 with Pomelo Provider",
  "Authentication": "JWT Bearer Tokens",
  "Validation": "FluentValidation",
  "Documentation": "Swashbuckle.AspNetCore (Swagger)",
  "Logging": "Serilog",
  "Metrics": "Prometheus + Grafana",
  "Caching": "Redis (opcional)",
  "Email": "MailKit + SMTP",
  "SMS": "Twilio API"
}
```

### Dependencias Principales
```xml
<!-- GunterBar.Presentation.csproj -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.2" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.1" />
<PackageReference Include="prometheus-net.AspNetCore" Version="8.2.1" />
```

## 🏗️ Arquitectura de la API

### Clean Architecture Layers

```
┌─────────────────────────────────────┐
│         Presentation Layer          │
│  ┌─────────────────────────────────┐ │
│  │     Controllers (REST API)     │ │
│  │                                 │ │
│  │  • AuthController              │ │
│  │  • UserController              │ │
│  │  • DrinkController             │ │
│  │  • OrderController             │ │
│  │  • CartController              │ │
│  └─────────────────────────────────┘ │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│        Application Layer            │
│  ┌─────────────────────────────────┐ │
│  │   Use Cases (CQRS Commands)     │ │
│  │                                 │ │
│  │  • Commands: CreateUser,       │ │
│  │    UpdateDrink, PlaceOrder     │ │
│  │  • Queries: GetUser,           │ │
│  │    GetDrinks, GetOrders        │ │
│  │  • Validators: UserValidator,  │ │
│  │    DrinkValidator, OrderValidator│ │
│  └─────────────────────────────────┘ │
│  ┌─────────────────────────────────┐ │
│  │     DTOs (Data Transfer)        │ │
│  │                                 │ │
│  │  • Request/Response objects    │ │
│  │  • Validation attributes       │ │
│  └─────────────────────────────────┘ │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│        Domain Layer                 │
│  ┌─────────────────────────────────┐ │
│  │     Business Entities           │ │
│  │                                 │ │
│  │  • User, Drink, Order, Cart     │ │
│  │  • Value Objects: Money, Email │ │
│  │  • Domain Events               │ │
│  └─────────────────────────────────┘ │
│  ┌─────────────────────────────────┐ │
│  │   Domain Services & Rules       │ │
│  │                                 │ │
│  │  • Business logic validation   │ │
│  │  • Domain invariants           │ │
│  └─────────────────────────────────┘ │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│      Infrastructure Layer           │
│  ┌─────────────────────────────────┐ │
│  │     External Concerns           │ │
│  │                                 │ │
│  │  • Entity Framework Context    │ │
│  │  • Repository implementations  │ │
│  │  • External services: Email,   │ │
│  │    SMS, Cache, File storage    │ │
│  └─────────────────────────────────┘ │
└─────────────────────────────────────┘
```

### CQRS Implementation

#### Command Pattern
```csharp
// Application/UseCases/Users/CreateUserCommand.cs
public class CreateUserCommand : IRequest<UserDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserRole Role { get; set; }
}

// Application/UseCases/Users/CreateUserCommandHandler.cs
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Business logic implementation
        var user = new User(request.Email, request.FirstName, request.LastName, request.Role);
        user.SetPassword(_passwordHasher.HashPassword(request.Password));

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new UserDto(user);
    }
}
```

#### Query Pattern
```csharp
// Application/UseCases/Drinks/GetDrinksQuery.cs
public class GetDrinksQuery : IRequest<IEnumerable<DrinkDto>>
{
    public string? Category { get; set; }
    public bool? AvailableOnly { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SearchTerm { get; set; }
}

// Application/UseCases/Drinks/GetDrinksQueryHandler.cs
public class GetDrinksQueryHandler : IRequestHandler<GetDrinksQuery, IEnumerable<DrinkDto>>
{
    private readonly IDrinkRepository _drinkRepository;

    public async Task<IEnumerable<DrinkDto>> Handle(GetDrinksQuery request, CancellationToken cancellationToken)
    {
        var query = _drinkRepository.GetAll();

        if (request.AvailableOnly == true)
            query = query.Where(d => d.IsAvailable);

        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(d => d.Category == request.Category);

        if (request.MinPrice.HasValue)
            query = query.Where(d => d.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(d => d.Price <= request.MaxPrice.Value);

        if (!string.IsNullOrEmpty(request.SearchTerm))
            query = query.Where(d => d.Name.Contains(request.SearchTerm) ||
                                    d.Description.Contains(request.SearchTerm));

        var drinks = await query.ToListAsync(cancellationToken);
        return drinks.Select(d => new DrinkDto(d));
    }
}
```

## 🔐 Autenticación y Autorización

### JWT Token Structure
```json
{
  "alg": "HS256",
  "typ": "JWT"
}
{
  "sub": "user-id",
  "email": "user@example.com",
  "role": "Customer|Admin|Bartender",
  "iat": 1640995200,
  "exp": 1641081600,
  "iss": "GunterBar.API",
  "aud": "GunterBar.Client"
}
```

### Authentication Flow

#### 1. Login Request
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "customer@gunterbar.com",
  "password": "SecurePass123!"
}
```

#### 2. Token Response
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh-token-here",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "user": {
    "id": "user-guid",
    "email": "customer@gunterbar.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Customer"
  }
}
```

#### 3. Using Tokens
```http
GET /api/drinks
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Refresh Token Flow
```http
POST /api/auth/refresh
Content-Type: application/json
Authorization: Bearer <expired-access-token>

{
  "refreshToken": "refresh-token-here"
}
```

## 📚 API Endpoints Documentation

### 🔑 Authentication Endpoints

#### POST /api/auth/register
Registra un nuevo usuario en el sistema.

**Request Body:**
```json
{
  "email": "newuser@gunterbar.com",
  "password": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Customer"
}
```

**Response (201 Created):**
```json
{
  "id": "user-guid",
  "email": "newuser@gunterbar.com",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Customer",
  "createdAt": "2024-01-01T10:00:00Z"
}
```

**Validation Rules:**
- Email: Formato válido, único en el sistema
- Password: Mínimo 8 caracteres, al menos 1 mayúscula, 1 minúscula, 1 número
- FirstName/LastName: 2-50 caracteres, solo letras y espacios

#### POST /api/auth/login
Autentica un usuario y devuelve tokens JWT.

**Request Body:**
```json
{
  "email": "customer@gunterbar.com",
  "password": "SecurePass123!"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "refresh-token-uuid",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "user": {
    "id": "user-guid",
    "email": "customer@gunterbar.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "Customer"
  }
}
```

#### POST /api/auth/refresh
Renueva el access token usando un refresh token válido.

### 👥 User Management Endpoints

#### GET /api/users/profile
Obtiene el perfil del usuario autenticado.

**Authorization:** Bearer Token (Customer, Admin, Bartender)

**Response (200 OK):**
```json
{
  "id": "user-guid",
  "email": "customer@gunterbar.com",
  "firstName": "John",
  "lastName": "Doe",
  "role": "Customer",
  "createdAt": "2024-01-01T10:00:00Z",
  "lastLoginAt": "2024-01-15T14:30:00Z"
}
```

#### PUT /api/users/profile
Actualiza el perfil del usuario autenticado.

**Request Body:**
```json
{
  "firstName": "Johnny",
  "lastName": "Doe",
  "email": "johnny@gunterbar.com"
}
```

#### GET /api/users
Obtiene lista de usuarios (solo Admin).

**Authorization:** Bearer Token (Admin)

**Query Parameters:**
- `page=1` - Página (default: 1)
- `pageSize=10` - Elementos por página (default: 10)
- `role=Customer` - Filtrar por rol
- `search=john` - Buscar por nombre o email

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "user-guid",
      "email": "customer@gunterbar.com",
      "firstName": "John",
      "lastName": "Doe",
      "role": "Customer",
      "createdAt": "2024-01-01T10:00:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

### 🍺 Drink Management Endpoints

#### GET /api/drinks
Obtiene lista de bebidas con filtros opcionales.

**Query Parameters:**
- `category=Beer` - Filtrar por categoría
- `availableOnly=true` - Solo bebidas disponibles
- `minPrice=5.00` - Precio mínimo
- `maxPrice=20.00` - Precio máximo
- `search=IPA` - Buscar por nombre o descripción

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "drink-guid",
      "name": "Gunter IPA",
      "description": "India Pale Ale premium",
      "price": 8.50,
      "category": "Beer",
      "isAvailable": true,
      "imageUrl": "https://api.gunterbar.com/images/gunter-ipa.jpg",
      "createdAt": "2024-01-01T10:00:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

#### GET /api/drinks/{id}
Obtiene detalles de una bebida específica.

#### POST /api/drinks
Crea una nueva bebida (Admin/Bartender).

**Authorization:** Bearer Token (Admin, Bartender)

**Request Body:**
```json
{
  "name": "Nueva Cerveza",
  "description": "Descripción detallada",
  "price": 7.50,
  "category": "Beer",
  "isAvailable": true,
  "imageUrl": "https://api.gunterbar.com/images/nueva-cerveza.jpg"
}
```

#### PUT /api/drinks/{id}
Actualiza una bebida existente (Admin/Bartender).

#### DELETE /api/drinks/{id}
Elimina una bebida (Admin only).

### 🛒 Cart Management Endpoints

#### GET /api/cart
Obtiene el carrito de compras del usuario.

**Response (200 OK):**
```json
{
  "id": "cart-guid",
  "userId": "user-guid",
  "items": [
    {
      "drinkId": "drink-guid",
      "drinkName": "Gunter IPA",
      "quantity": 2,
      "unitPrice": 8.50,
      "totalPrice": 17.00
    }
  ],
  "subtotal": 17.00,
  "tax": 2.55,
  "total": 19.55,
  "createdAt": "2024-01-15T14:00:00Z",
  "updatedAt": "2024-01-15T14:30:00Z"
}
```

#### POST /api/cart/items
Agrega un item al carrito.

**Request Body:**
```json
{
  "drinkId": "drink-guid",
  "quantity": 2
}
```

#### PUT /api/cart/items/{drinkId}
Actualiza la cantidad de un item en el carrito.

**Request Body:**
```json
{
  "quantity": 3
}
```

#### DELETE /api/cart/items/{drinkId}
Elimina un item del carrito.

#### DELETE /api/cart
Vacía completamente el carrito.

### 📋 Order Management Endpoints

#### GET /api/orders
Obtiene los pedidos del usuario.

**Query Parameters:**
- `status=Pending` - Filtrar por estado
- `page=1` - Página
- `pageSize=10` - Elementos por página

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "order-guid",
      "userId": "user-guid",
      "status": "Pending",
      "items": [
        {
          "drinkId": "drink-guid",
          "drinkName": "Gunter IPA",
          "quantity": 2,
          "unitPrice": 8.50,
          "totalPrice": 17.00
        }
      ],
      "subtotal": 17.00,
      "tax": 2.55,
      "total": 19.55,
      "createdAt": "2024-01-15T14:00:00Z",
      "updatedAt": "2024-01-15T14:30:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

#### GET /api/orders/{id}
Obtiene detalles de un pedido específico.

#### POST /api/orders
Crea un nuevo pedido desde el carrito actual.

**Request Body:**
```json
{
  "specialInstructions": "Sin hielo extra por favor"
}
```

**Response (201 Created):**
```json
{
  "id": "order-guid",
  "status": "Pending",
  "estimatedDeliveryTime": "2024-01-15T15:00:00Z",
  "message": "Pedido creado exitosamente"
}
```

#### PUT /api/orders/{id}/status
Actualiza el estado de un pedido (Admin/Bartender).

**Authorization:** Bearer Token (Admin, Bartender)

**Request Body:**
```json
{
  "status": "InProgress"
}
```

**Estados válidos:** Pending, InProgress, Ready, Completed, Cancelled

## 📊 Response Codes and Error Handling

### HTTP Status Codes

| Code | Description | Usage |
|------|-------------|-------|
| 200 | OK | Operación exitosa |
| 201 | Created | Recurso creado |
| 204 | No Content | Operación exitosa sin contenido |
| 400 | Bad Request | Datos inválidos |
| 401 | Unauthorized | Token faltante/inválido |
| 403 | Forbidden | Permisos insuficientes |
| 404 | Not Found | Recurso no encontrado |
| 409 | Conflict | Conflicto (ej: email duplicado) |
| 422 | Unprocessable Entity | Validación fallida |
| 500 | Internal Server Error | Error del servidor |

### Error Response Format
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "The request contains invalid data",
  "instance": "/api/auth/register",
  "errors": {
    "Email": ["The Email field is required."],
    "Password": ["Password must be at least 8 characters long."]
  }
}
```

## 🔒 Security Features

### Input Validation
```csharp
// Application/DTOs/User/CreateUserRequest.cs
public class CreateUserRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, and one number")]
    public string Password { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name can only contain letters and spaces")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name can only contain letters and spaces")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid role")]
    public UserRole Role { get; set; }
}
```

### Rate Limiting
```csharp
// Presentation/Middleware/RateLimitingMiddleware.cs
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Dictionary<string, (int Count, DateTime ResetTime)> _requests = new();

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var key = $"{ip}:{DateTime.UtcNow.ToString("yyyyMMddHHmm")}";

        if (_requests.TryGetValue(key, out var entry))
        {
            if (entry.Count >= 100) // 100 requests per minute
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsJsonAsync(new { message = "Too many requests" });
                return;
            }
            _requests[key] = (entry.Count + 1, entry.ResetTime);
        }
        else
        {
            _requests[key] = (1, DateTime.UtcNow.AddMinutes(1));
        }

        await _next(context);
    }
}
```

### CORS Configuration
```csharp
// Presentation/Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://gunterbar.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

## 📈 Monitoring and Metrics

### Prometheus Metrics
```csharp
// Presentation/Program.cs
app.UseMetricServer();
app.UseHttpMetrics();

// Custom metrics
var requestCounter = Metrics.CreateCounter("api_requests_total", "Total API requests",
    new CounterConfiguration { LabelNames = new[] { "method", "endpoint", "status" } });

app.Use(async (context, next) =>
{
    await next();
    requestCounter.WithLabels(context.Request.Method, context.Request.Path, context.Response.StatusCode.ToString()).Inc();
});
```

### Health Checks
```csharp
// Presentation/Program.cs
builder.Services.AddHealthChecks()
    .AddDbContextCheck<GunterBarDbContext>("Database")
    .AddRedis("redis:6379", "Cache")
    .AddUrlGroup(new Uri("https://api.twilio.com"), "SMS Service");

app.MapHealthChecks("/health");
```

## 🧪 Testing the API

### Using Swagger UI
1. Inicia la aplicación
2. Navega a `https://localhost:7221/swagger`
3. Autorízate con un token JWT
4. Prueba los endpoints interactivamente

### Using cURL Examples

#### Register User
```bash
curl -X POST "https://localhost:7221/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@gunterbar.com",
    "password": "TestPass123!",
    "firstName": "Test",
    "lastName": "User",
    "role": "Customer"
  }'
```

#### Login and Get Token
```bash
TOKEN=$(curl -X POST "https://localhost:7221/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@gunterbar.com",
    "password": "TestPass123!"
  }' | jq -r '.accessToken')
```

#### Get Drinks
```bash
curl -X GET "https://localhost:7221/api/drinks" \
  -H "Authorization: Bearer $TOKEN"
```

### Using Postman Collection
Importa el archivo `GunterBar.postman_collection.json` incluido en el repositorio.

## 🚀 Deployment and Scaling

### Environment Variables
```bash
# Database
ConnectionStrings__DefaultConnection="server=mysql;port=3306;database=gunterbar;user=gunter;password=gunterpass"

# JWT
JWT__Secret="your-256-bit-secret-here"
JWT__Issuer="GunterBar.API"
JWT__Audience="GunterBar.Client"
JWT__ExpiryMinutes=60

# Email (opcional)
Email__SmtpHost="smtp.gmail.com"
Email__SmtpPort=587
Email__Username="your-email@gmail.com"
Email__Password="your-app-password"

# SMS (opcional)
SMS__AccountSid="your-twilio-sid"
SMS__AuthToken="your-twilio-token"
SMS__FromNumber="+1234567890"

# Redis (opcional)
Redis__ConnectionString="redis:6379"
```

### Docker Deployment
```yaml
# docker-compose.yml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "5221:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:80
    depends_on:
      - mysql
```

## 📞 Support and Documentation

### API Documentation
- **Swagger UI**: `https://localhost:7221/swagger`
- **OpenAPI Spec**: `https://localhost:7221/swagger/v1/swagger.json`
- **Health Check**: `https://localhost:7221/health`

### Additional Resources
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [JWT Authentication](https://tools.ietf.org/html/rfc7519)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## 👥 Development Team

### 👨‍💻 Lead Developer
**Roque Rivas** - *Full-Stack Developer & API Architect*  
📧 junior.rivaset12d1@gmail.com  
🔗 [GitHub](https://github.com/rockyet12)

### 🏫 Educational Institution
**ET12 - Escuela Técnica N°12 D.E.1°**  
📍 Buenos Aires, Argentina

### 👨‍🏫 Technical Mentors
- **Sergio Mendoza** - Systems Development Professor
- **Adrián Cives** - Project Coordinator

---

**🔌 API RESTful desarrollada con ASP.NET Core 9.0**  
**Arquitectura: Clean Architecture + CQRS + JWT Authentication**  
**Documentación: Swagger/OpenAPI 3.0 + Comprehensive Examples**  
**Seguridad: Input Validation + Rate Limiting + CORS**
