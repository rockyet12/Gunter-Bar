# Gunter-Bar

Este proyecto contiene el **backend** y el **frontend** de **Gunter-Bar**, una aplicación diseñada para gestionar un bar con autenticación JWT, CRUD de productos/categorías/usuarios y gestión integral de pedidos, carritos, tickets y tragos.

---

## Trabajo Práctico – Escuela Técnica N° 12 D.E. 1° “Libertador Gral. José de San Martín” (ET12)

- Sitio web: http://et12.edu.ar
- Asignatura: Desarrollo de Sistemas
- Nombre del Trabajo Práctico: GUNTER BAR
- Docentes: Sergio Mendoza y Adrián Cives
- Ciclo Lectivo: 2025
- Año y División: 6°17°
- Alumnas y alumnos:
  - Sofia Colman
  - Camila Reyes
  - Ana Martinez
  - Roque Rivas
  - Julio Martinez

### Índice de contenidos del TP
- Visión del proyecto
- Propuesta de valor (E‑commerce + Academia de Cócteles)
- Diseño y UX/UI
- Objetivos
- Beneficios para la comunidad
- Definición de Infraestructura (recursos materiales y técnicos)
- Software y servicios
- DevOps
- Arquitectura técnica y entornos
- Seguridad
- Backups y continuidad
- Logística operativa
- Roles, permisos y módulos
- Technology Stack (EN)
- Coding Conventions (EN)
- Latest Improvements (EN)
- Project Structure (EN)
- API (EN)
- Prerequisites (EN)
- Quick Start (EN)
- Using the API with Swagger (EN)
- Database (EN)
- Main Entities (EN)
- Available Scripts (EN)
- Technical Notes (EN)
- Troubleshooting (EN)
- Start the project (EN)
- Additional Notes (EN)

---

## 🚀 Latest Improvements (EN)

### **Major .NET 9 Upgrade (October 2025)**
- **Complete migration** from .NET 8 to .NET 9 across all projects
- **Updated NuGet packages**: EntityFrameworkCore, JWT Bearer, Pomelo MySQL driver
- **CI/CD compatibility**: GitHub Actions updated for .NET 9 support
- **Performance improvements** and latest framework features

### **Enhanced API Controllers**
All controllers have been significantly improved with new functionality:

#### 🔐 **UserController** - Enhanced User Management
- **Advanced search**: Search users by name, email, or role
- **User statistics**: Get comprehensive user analytics
- **Profile management**: Update user profiles with validation
- **Password security**: Secure password change functionality
- **Admin controls**: Toggle user status (active/inactive)
- **Detailed logging**: All operations tracked for security

#### 📦 **ProductController** - Advanced Product Features
- **Promotional products**: Get current promotions and featured items
- **Product cloning**: Duplicate products for quick catalog expansion
- **Mass price updates**: Update prices by category efficiently
- **Enhanced filtering**: Advanced product search and categorization
- **Stock management**: Better inventory control and alerts

#### 🧾 **TicketController** - Complete Table Management
- **Advanced filtering**: Filter tickets by date range, table, status
- **Real-time tracking**: Today's active tickets with live updates
- **Smart closure**: Automated ticket closure with validation
- **Analytics dashboard**: Comprehensive ticket statistics and reports
- **Table optimization**: Efficient table turnover management

#### 🍺 **DrinkTypeController** - Enhanced Beverage Categories
- **Popular types**: Get trending drink categories
- **Smart search**: Enhanced search with type-specific filtering
- **Usage analytics**: Track most popular drink types
- **Category management**: Advanced categorization features

#### 🍸 **DrinkController** - Improved Beverage Management
- **Type-based filtering**: Advanced filtering by drink types
- **Drink statistics**: Analytics on beverage consumption
- **Smart recommendations**: Suggest drinks based on preferences
- **Inventory optimization**: Better stock management for drinks

#### 📋 **OrderController** - Advanced Order Processing
- **Status filtering**: Filter orders by processing status
- **User order history**: Complete order tracking per user
- **Order analytics**: Comprehensive order statistics
- **Processing optimization**: Streamlined order fulfillment

### **Technical Improvements**
- **Clean Architecture**: Maintained separation of concerns
- **Error handling**: Comprehensive error management and logging
- **Security enhancements**: Role-based authorization across all endpoints
- **Performance optimization**: Efficient database queries and caching
- **Code quality**: Consistent coding standards and documentation

---

## Technology Stack (EN)
- Backend: **.NET 9** (ASP.NET Core Web API), JWT, Swagger/OpenAPI, EF Core 9.0 (Pomelo for MariaDB)
- Frontend: React + TypeScript (Vite), React Router, Axios, JWT client
- Database: MariaDB 10.11+
- Infra: Nginx (reverse proxy), Docker/Compose (optional for dev/prod)
- CI/CD: GitHub Actions with .NET 9 support

---

## Visión
“Gunter Bar”, un comercio electrónico que trasciende la simple venta de bebidas para convertirse en un sitio web de experiencias y aprendizaje. Será la referencia para entusiastas que buscan licores e insumos de coctelería de alta calidad, con asesoramiento experto para dominar el arte del cóctel en casa.  
El sitio destacará por un diseño visualmente atractivo e intuitivo, con contenido de alto valor, brindando una experiencia memorable y enriquecedora.

## Propuesta
Catálogo curado de bebidas (destilados, vinos, cervezas artesanales y mixers exclusivos) + “Academia de Cócteles” interactiva. Cada compra inicia una aventura de sabor con guía profesional.

### E‑commerce
- Catálogo impecable: fotos de alta calidad, descripciones con notas de cata y recomendaciones de maridaje/coctelería.
- Kits temáticos de coctelería: p. ej. “Kit Old Fashioned Clásico” con ingredientes + enlace al tutorial.
- Compra fluida: navegación, filtros y checkout rápidos y seguros.
- Logística de entregas: retiro en local, envíos de zona y delivery con estados visibles (recibido, preparación, en camino, entregado).

### Academia de Cócteles
- Videotutoriales exclusivos: técnicas básicas a avanzadas, dictadas por barmans.
- Recetas interactivas: filtros por licor, dificultad y perfil de sabor.
- Contenido narrativo: historia y cultura detrás de licores y cócteles.

### Diseño y UX/UI
- Estética elegante: interfaz oscura de bar con imágenes vibrantes.
- Comunidad y reviews: valoraciones de productos y recetas para impulsar confianza.

## Objetivo
Posicionar “El Arte del Cóctel” como plataforma líder de e‑commerce de bebidas premium y escuela de mixología digital, equilibrando facturación por productos y construcción de comunidad de aprendizaje.

- Catálogo con excelente relación calidad/precio ajustado al público.
- Web que amplíe alcance y adquisición.
- Promociones temáticas, combos y sorteos para lealtad.
- Marketing y contenido para redes afines a público joven.
- Ambiente acogedor que motive el regreso y la recomendación.

## Beneficios para la comunidad
- Productos de calidad a precios accesibles.
- Experiencia divertida y segura: temática cultural y controlada.
- Participación y fidelización: promos, combos, sorteos y contenido digital.
- Acceso digital: web y redes para interacción y compras online.

---

## Definición de Infraestructura

### Recursos materiales y técnicos
- Estación principal (caja/gestión):
  - Acer Aspire 3 (equipo actual):
    - CPU: AMD Ryzen 5
    - RAM: 24 GB
    - Almacenamiento: 512 GB SSD M.2 (SO y apps) + 1 TB HDD (datos/medios)
    - Conectividad: Wi‑Fi (recomendado Ethernet para caja)
- POS con impresora y lector:
  - Impresora térmica 80 mm (USB/Ethernet)
  - Lector de códigos/QR USB (opcional) y cajón monedero (opcional)
- Red y conectividad:
  - Conexión a Internet (ISP)
  - Router/módem del ISP + Access Point Wi‑Fi AC/AX recomendado
  - Switch gigabit si suman dispositivos cableados (p. ej., cámaras)
- Logística/operación:
  - Pantalla de barra/KDS (monitor 24–27” o tablet) para preparación de pedidos
  - Celular opcional con WhatsApp Business
  - Impresora de etiquetas 4x6 (opcional) si escalan envíos
- Seguridad física:
  - Cámaras IP (2–4) 1080p (ideal PoE con NVR)
  - UPS 650–1000 VA para router/AP y estación principal
- Respaldo:
  - Sistema de backup manual o en la nube (DB y media)

### Software y servicios
- Plataforma:
  - Backend: .NET 8 + JWT + Swagger; EF Core con Pomelo MariaDB
  - Base de datos: MariaDB 10.11+
  - Frontend: React + TypeScript (Vite), React Router, Axios, jwt-decode
- Operación y terceros:
  - POS: Poster POS, KiWi o Vendus
  - Gestión: Xubio, Tango Gestión o Google Sheets
  - Diseño: Canva
  - Atención al cliente: WhatsApp Business, Instagram
  - Facturación electrónica (AR): AFIP o Facturante (API)
  - Almacenamiento: Google Drive (o S3/R2 si crece)
  - Seguridad endpoint: Avast o Bitdefender en la netbook
- DevOps:
  - Docker y Docker Compose
  - Nginx como reverse proxy y hosting de frontend estático
  - HTTPS con Let’s Encrypt
  - Monitoreo (UptimeKuma) y logging (Serilog + sink)
  - CI/CD con GitHub Actions (opcional)

### Arquitectura técnica y entornos
- Producción recomendada:
  - Nginx (reverse proxy) → API .NET (contenedor) → MariaDB (contenedor o DB gestionada)
  - Frontend React servido como estático en Nginx
  - CORS restringido a dominios propios; secretos por variables de entorno
- Entornos:
  - Desarrollo: DB local, Swagger abierto
  - Staging (opcional): pruebas pre‑prod
  - Producción: HTTPS obligatorio, rate limiting, Swagger solo lectura/protegido

### Seguridad
- App:
  - Autenticación JWT; autorización por roles (Cliente/Buyer, Operador/Vendedor, Jefe de Ventas)
  - Validación de inputs, límites de tamaño, saneamiento y registro de auditoría
- Infra:
  - TLS, headers seguros (HSTS, CSP, no‑sniff), firewall y fail2ban
  - Parcheo y actualizaciones regulares
- Operación:
  - Antivirus en estación principal
  - Gestión de contraseñas y 2FA en cuentas críticas

### Backups y continuidad
- MariaDB:
  - Dump diario cifrado; retención 7–30 días; prueba de restauración mensual
- Media y configuración:
  - Código versionado (Git); artefactos reproducibles
- Recuperación:
  - Restaurar Compose (Nginx/API/DB) + último dump; validación de salud y pruebas funcionales

### Logística operativa
- Modos:
  - Retiro en local, envío local por zona, delivery a domicilio
- Estados:
  - Pedido: Recibido → Pagado → En preparación → En camino → Entregado | Cancelado
- Impresión:
  - Ticket de preparación y/o etiqueta con QR para verificación/seguimiento

### Roles, permisos y módulos

#### Roles
- Cliente (Buyer): navegar, comprar, ver pedidos y seguimiento.
- Vendedor/Operador (Seller): gestionar productos propios, ver pedidos con sus líneas, preparar y despachar envíos.
- Jefe de Ventas (SalesManager): vista y control global, políticas, reportes, intervención operativa.

#### Módulos principales
- Productos: catálogo, stock, kits temáticos.
- Pedidos/Checkout: carrito, pago, selección de envío/dirección.
- Envíos: cotización/local, etiquetas, tracking básico.
- Academia: recetas, videotutoriales, storytelling de marcas y cócteles.
- Comunidad: reviews y valoraciones.
- Administración: gestión de roles y políticas, reportes.

---

## Coding Conventions (EN)
- All code identifiers MUST be in English: classes, interfaces, methods, properties, DTOs, route names, and database entity names.
- Use PascalCase for classes and public members, camelCase for local variables and parameters, SNAKE_CASE for environment variables.
- Avoid Spanish terms in code. UI copy and business narrative can remain in Spanish.

---

## ✨ Latest Improvements (EN)

### Backend fixes
- Build: fixed project references and dependencies
- Database: configured MariaDB/MySQL connection via Entity Framework Core
- Entities: updated for EF compatibility (parameterless constructors, nullable navigation properties, [Key] attributes)
- JWT Authentication: Bearer token with roles (Administrator/Client)
- Swagger: enabled with JWT authorization support (Authorize button)
- Controllers: full CRUD for main entities
- DTOs: input validation for register/login using DataAnnotations
- Cleanup: removed duplicates and organized backups

## 🗄️ Project Structure (EN)
```
Backend-Bar/
├── BarGunter.API/           # Web API with controllers and Swagger
├── BarGunter.Application/   # Services and DTOs
├── BarGunter.Domain/        # Entities and enums
├── BarGunter.Infrastructure/# Repositories and DbContext
└── scripts/                 # Build/deploy scripts
```

## 📡 Available API Endpoints (EN)

### Public (no authentication)
- POST `/api/Users/register` - Register new users
- POST `/api/Users/login` - Login (returns JWT token)

### Protected (require JWT)
- GET/POST/PUT/DELETE `/api/Carts` - Cart management
- GET/POST/PUT/DELETE `/api/Categories` - Category management
- GET/POST/PUT/DELETE `/api/Orders` - Order management
- GET/POST/PUT/DELETE `/api/Products` - Product management
- GET/POST/PUT/DELETE `/api/Tickets` - Ticket management
- GET/POST/PUT/DELETE `/api/Types` - Type management
- GET/POST/PUT/DELETE `/api/Cocktails` - Cocktail management

### Admin only
- GET/POST/PUT/DELETE `/api/Users` - User management
- PUT `/api/Users/{id}/role` - Change user role

> Note: If your current API uses Spanish route names (e.g., `/api/Usuario`, `/api/Producto`), adapt the paths accordingly until the refactor is complete.

---

## ✅ Prerequisites (EN)

Before starting, install:

1. **.NET SDK (v9.0 or higher)** — https://dotnet.microsoft.com/download
   - **Important**: This project requires .NET 9 for full compatibility
2. **Node.js and npm** (latest LTS) — https://nodejs.org/
3. **MariaDB/MySQL (v10.11+ recommended)**
   - Default local dev credentials: `root` / `rootroot` (see `appsettings.Development.json`)

---

## 🚀 Quick Start (EN)

### 1) Clone the repository
```bash
git clone https://github.com/rockyet12/Gunter-Bar.git
cd Gunter-Bar
```

### 2) Backend (API)
```bash
cd Backend-Bar

# Restore dependencies
dotnet restore

# Build solution
dotnet build Backend-Bar.sln

# Apply database migrations
dotnet ef database update --project BarGunter.API

# Run API (listens on http://localhost:5172)
dotnet run --project BarGunter.API
```

Swagger UI: http://localhost:5172/swagger

### 3) Frontend (React)
```bash
cd Frontend-Bar
npm install
npm run dev
```

---

## 🔑 Using the API with Swagger (EN)

### 1) Register a user
```json
POST /api/Users/register
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "password": "Secret123",
  "nationalId": 12345678
}
```

### 2) Login and get a JWT
```json
POST /api/Users/login
{
  "email": "john.doe@example.com",
  "password": "Secret123"
}
```

Successful response:
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 3) Authorize in Swagger
1. Copy the `token` value from the login response
2. In Swagger UI, click "Authorize"
3. Enter: `Bearer <your-token-here>`
4. Close the dialog and call protected endpoints

---

## 🗄️ Database (EN)

### Development configuration (appsettings.Development.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;PORT=3306;Database=BarGunterDb;Uid=root;Pwd=rootroot;"
  }
}
```

### Main Entities (EN)
- **User**: user accounts with roles (Admin/Client)
- **Product**: bar products with categories
- **Category**: product categorization
- **Cocktail**: recipes and preparations
- **Order**: customer orders
- **Cart**: shopping carts
- **Ticket**: receipts/invoices
- **Type**: additional classification

---

## 🛠️ Available Scripts (EN)

### Backend
```bash
# Full rebuild script
./Backend-Bar/scripts/rebuild.sh

# Manual build
dotnet build Backend-Bar.sln

# Run with a specific URL
dotnet run --project BarGunter.API --urls "http://localhost:5172"
```

### Frontend
```bash
npm run dev
npm run build
npm run preview
```

---

## 📋 Technical Notes (EN)

### Architecture
- Clean Architecture: Domain, Application, Infrastructure, API
- Entity Framework Core for data access
- JWT Authentication (Bearer) with roles
- Swagger/OpenAPI for interactive docs
- Repository Pattern
- Dependency Injection configured in Program.cs

### Important configuration
- CORS configured for local development
- JWT Secret: `TucodigodeseguridadWAZAAAAAAA!!` (change for production)
- API Port: 5172 (configurable in launchSettings.json)
- Database: MariaDB/MySQL with automatic migrations

### Backups and cleanup
- Backup files in `Backend-Bar/backups/`
- Duplicates removed during project cleanup
- Git history preserved with descriptive commits

---

## 🚨 Troubleshooting (EN)

### Port already in use
```bash
lsof -i :5172
kill <PID>
```

### Build problems
```bash
dotnet clean
dotnet restore
dotnet build
```

### Database
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## ▶️ Start the project (EN)

### Start backend
```bash
cd ../Backend-Bar
dotnet run
```

### Start frontend
```bash
cd ../Frontend-Bar
npm run dev
```

---

## 🔧 Additional Notes (EN)
- Ensure the database specified in the connection string exists.
- Tools like MySQL Workbench or DBeaver can help manage the database.
- If the backend cannot connect, verify the connection string and that MySQL/MariaDB is running.
- Check tool versions are compatible with the project requirements.

---

¡Gracias por usar **Gunter-Bar**! 🍺