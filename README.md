# Gunter Bar - Sistema de Gestión de Bar

## 📋 Descripción del Proyecto

**Gunter Bar** es un sistema completo de gestión para un bar desarrollado como proyecto educativo para ET12 - Escuela Técnica 12. El sistema permite gestionar usuarios, bebidas, carritos de compra y órdenes, implementando un sistema de autenticación JWT y una arquitectura limpia (Clean Architecture).

## 🚀 Tecnologías Utilizadas

### Backend (.NET 9)
- **Framework**: ASP.NET Core 9.0
- **Arquitectura**: Clean Architecture (Domain, Application, Infrastructure, Presentation)
- **Base de Datos**: SQL Server con Entity Framework Core 9.0
- **Autenticación**: JWT Bearer Authentication
- **Testing**: xUnit con Moq y FluentAssertions
- **Documentación**: Swagger/OpenAPI

### Frontend (React + TypeScript)
- **Framework**: React 18 con TypeScript
- **Bundler**: Create React App
- **Styling**: CSS Modules
- **HTTP Client**: Axios
- **Gestión de Estado**: React Hooks

## 🏗️ Arquitectura del Backend

```
GunterBar.Solution/
│
├── GunterBar.Domain/              (Capa de Dominio)
│   ├── Entities/                  (Entidades de negocio)
│   ├── ValueObjects/              (Objetos de valor)
│   ├── Interfaces/                (Contratos del dominio)
│   └── Enums/                     (Enumeraciones)
│
├── GunterBar.Application/         (Capa de Aplicación)
│   ├── Services/                  (Servicios de aplicación)
│   ├── UseCases/                  (Casos de uso)
│   ├── Interfaces/                (Contratos de aplicación)
│   └── DTOs/                      (Objetos de transferencia)
│
├── GunterBar.Infrastructure/      (Capa de Infraestructura)
│   ├── Data/                      (Contexto de BD y configuraciones)
│   ├── Repositories/              (Implementación de repositorios)
│   └── ExternalServices/          (Servicios externos)
│
├── GunterBar.Presentation/        (Capa de Presentación)
│   ├── Controllers/               (Controladores de API)
│   ├── Models/                    (Modelos de vista)
│   └── Program.cs                 (Punto de entrada)
│
└── GunterBar.Tests/               (Pruebas Unitarias)
    ├── DomainTests/
    ├── ApplicationTests/
    ├── InfrastructureTests/
    └── PresentationTests/
```

## 🎯 Funcionalidades Principales

### Sistema de Autenticación
- [x] Registro de usuarios
- [x] Login con JWT
- [x] Roles de usuario (Admin, Cliente)
- [x] Protección de endpoints

### Gestión de Bebidas
- [x] CRUD de bebidas
- [x] Categorización por tipo
- [x] Gestión de ingredientes
- [x] Control de stock

### Sistema de Carritos
- [x] Agregar/quitar items
- [x] Actualizar cantidades
- [x] Cálculo de totales
- [x] Persistencia por usuario

### Gestión de Órdenes
- [x] Crear órdenes desde carrito
- [x] Estados de órdenes
- [x] Historial de órdenes
- [x] Administración de órdenes

## 🛠️ Instalación y Configuración

### Prerrequisitos
- .NET 9.0 SDK
- SQL Server (LocalDB o instancia completa)
- Node.js 18+ y npm
- Git

### Backend Setup

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/rockyet12/Gunter-Bar.git
   cd Gunter-Bar/backend
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Configurar conexión a base de datos**
   ```bash
   # Editar appsettings.json en GunterBar.Presentation
   # Configurar la cadena de conexión DefaultConnection
   ```

4. **Crear y aplicar migraciones**
   ```bash
   dotnet ef migrations add InitialCreate --project GunterBar.Infrastructure --startup-project GunterBar.Presentation
   dotnet ef database update --project GunterBar.Infrastructure --startup-project GunterBar.Presentation
   ```

5. **Ejecutar la aplicación**
   ```bash
   dotnet run --project GunterBar.Presentation
   ```

### Frontend Setup

1. **Navegar a la carpeta frontend**
   ```bash
   cd ../frontend
   ```

2. **Instalar dependencias**
   ```bash
   npm install
   ```

3. **Ejecutar en modo desarrollo**
   ```bash
   npm start
   ```

## 📚 Documentación API

La documentación de la API está disponible a través de Swagger UI:
- **Desarrollo**: `https://localhost:7000/swagger`
- **Producción**: `[URL_PRODUCCION]/swagger`

### Principales Endpoints

#### Autenticación
- `POST /api/auth/register` - Registro de usuario
- `POST /api/auth/login` - Inicio de sesión
- `GET /api/auth/profile` - Perfil del usuario autenticado

#### Bebidas
- `GET /api/drinks` - Listar bebidas
- `GET /api/drinks/{id}` - Obtener bebida por ID
- `POST /api/drinks` - Crear bebida (Admin)
- `PUT /api/drinks/{id}` - Actualizar bebida (Admin)
- `DELETE /api/drinks/{id}` - Eliminar bebida (Admin)

#### Carrito
- `GET /api/cart` - Obtener carrito del usuario
- `POST /api/cart/items` - Agregar item al carrito
- `PUT /api/cart/items/{id}` - Actualizar cantidad
- `DELETE /api/cart/items/{id}` - Remover item

#### Órdenes
- `GET /api/orders` - Listar órdenes del usuario
- `POST /api/orders` - Crear orden desde carrito
- `GET /api/orders/{id}` - Obtener orden por ID

## 🧪 Testing

### Ejecutar todas las pruebas
```bash
dotnet test
```

### Ejecutar pruebas con cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Tipos de pruebas incluidas
- **Pruebas unitarias**: Lógica de negocio y servicios
- **Pruebas de integración**: Repositorios y base de datos
- **Pruebas de API**: Endpoints y autenticación

## 📦 Deployment

### Configuración para Producción

1. **Configurar variables de entorno**
   ```bash
   export ConnectionStrings__DefaultConnection="[CADENA_PRODUCCION]"
   export JwtSettings__SecretKey="[CLAVE_SECRETA_PRODUCCION]"
   export ASPNETCORE_ENVIRONMENT="Production"
   ```

2. **Publicar aplicación**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

3. **Configurar servidor web** (IIS, Nginx, etc.)

## 👥 Equipo de Desarrollo

**Desarrollador Principal**: Roque Rivas  
**Institución**: ET12 - Escuela Técnica N° 12 D.E. 1°  
**Materia**: Desarrollo de Sistemas  
**Año**: 2025

### Compañeros de Curso
- Sofia Colman
- Camila Reyes
- Ana Martinez
- Julio Martinez

### Docentes
- Sergio Mendoza
- Adrián Cives

## 📄 Licencia

Este proyecto es de uso educativo para ET12. Todos los derechos reservados.

## 🤝 Contribuciones

Este es un proyecto educativo. Las contribuciones están limitadas a los estudiantes de ET12 bajo supervisión del docente.

## 📞 Contacto

Para consultas sobre este proyecto:
- **GitHub**: [@rockyet12](https://github.com/rockyet12)
- **Institución**: ET12 - http://et12.edu.ar

---

**Proyecto desarrollado con 💻 y ☕ en ET12 - Escuela Técnica 12**
