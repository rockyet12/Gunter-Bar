# Gunter Bar - Backend

API REST para el sistema de gestión del bar Gunter.

## Tecnologías Principales

- ASP.NET Core 9.0
- Entity Framework Core
- MariaDB
- Swagger para documentación de API
- Prometheus para métricas

## Estructura del Proyecto

### Capas de la Aplicación

- **GunterBar.Domain**: 
  - Entidades del dominio
  - Interfaces de repositorios
  - Lógica de negocio core

- **GunterBar.Application**: 
  - DTOs
  - Interfaces de servicios
  - Casos de uso
  - Validadores

- **GunterBar.Infrastructure**: 
  - Implementaciones de repositorios
  - Configuración de base de datos
  - Servicios externos
  - Métricas y telemetría

- **GunterBar.Presentation**: 
  - Controllers
  - Middleware
  - Configuración de la API
  - Documentación Swagger

- **GunterBar.Tests**: 
  - Pruebas unitarias
  - Pruebas de integración
  - Tests de métricas

## Configuración y Ejecución

1. **Prerrequisitos**:
   - .NET 9.0 SDK
   - MariaDB

2. **Base de datos**:
   ```bash
   cd backend
   dotnet ef database update
   ```

3. **Variables de entorno**:
   - Configura el archivo `appsettings.json` con tus credenciales de base de datos

4. **Ejecución**:
   ```bash
   cd GunterBar.Presentation
   dotnet run
   ```

La API estará disponible en:
- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`
- Métricas: `http://localhost:5000/metrics`

## Endpoints Principales

### Autenticación
- POST `/api/auth/login`
- POST `/api/auth/register`

### Usuarios
- GET `/api/users`
- GET `/api/users/{id}`
- PUT `/api/users/{id}`
- DELETE `/api/users/{id}`

### Bebidas
- GET `/api/drinks`
- POST `/api/drinks`
- PUT `/api/drinks/{id}`
- DELETE `/api/drinks/{id}`

### Pedidos
- GET `/api/orders`
- POST `/api/orders`
- GET `/api/orders/{id}`
- PUT `/api/orders/{id}`

## Testing

Para ejecutar las pruebas:
```bash
dotnet test
```

## Métricas

El proyecto incluye métricas para monitoreo usando Prometheus:
- Usuarios activos
- Latencia de API
- Errores por endpoint
- Métricas de pedidos
