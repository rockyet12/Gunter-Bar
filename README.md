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

---

## Stack objetivo del proyecto
- Backend: .NET 8 (ASP.NET Core Web API), JWT, Swagger/OpenAPI, EF Core (Pomelo para MariaDB)
- Frontend: React + TypeScript (Vite), React Router, Axios, JWT client
- Base de datos: MariaDB 10.11+
- Infra: Nginx (reverse proxy), Docker/Compose (dev/prod opcional)

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

## ✨ Últimas mejoras implementadas

### 🔧 Fixes aplicados al Backend
- **Compilación**: Solucionados errores de build, referencias de proyectos y dependencias
- **Base de datos**: Configurada conexión a MariaDB/MySQL con Entity Framework Core
- **Entidades**: Ajustadas para compatibilidad con EF (constructores parameterless, nullable navigation properties, [Key] attributes)
- **JWT Authentication**: Configurado Bearer token authentication con roles (Administrador/Cliente)
- **Swagger**: Habilitado con soporte para autorización JWT (botón Authorize)
- **Controllers**: CRUD completo para todas las entidades principales
- **DTOs**: Validaciones de entrada para registro/login con DataAnnotations
- **Estructura limpia**: Eliminados archivos duplicados y backups organizados

### 🗄️ Estructura del proyecto
```
Backend-Bar/
├── BarGunter.API/          # Web API con controllers y Swagger
├── BarGunter.Application/   # Servicios y DTOs
├── BarGunter.Domain/        # Entidades y enums
├── BarGunter.Infrastructure/ # Repositorios y DbContext
└── scripts/                # Scripts de build/deploy
```

### 📡 API Endpoints disponibles

#### 🔓 Públicos (sin autenticación)
- **POST** `/api/Usuario/register` - Registro de nuevos usuarios
- **POST** `/api/Usuario/login` - Login (retorna JWT token)

#### 🔒 Protegidos (requieren JWT)
- **GET/POST/PUT/DELETE** `/api/Carrito` - Gestión de carritos
- **GET/POST/PUT/DELETE** `/api/Categoria` - Gestión de categorías  
- **GET/POST/PUT/DELETE** `/api/Pedido` - Gestión de pedidos
- **GET/POST/PUT/DELETE** `/api/Producto` - Gestión de productos
- **GET/POST/PUT/DELETE** `/api/Ticket` - Gestión de tickets
- **GET/POST/PUT/DELETE** `/api/Tipo` - Gestión de tipos
- **GET/POST/PUT/DELETE** `/api/Trago` - Gestión de tragos

#### 👑 Solo Administradores
- **GET/POST/PUT/DELETE** `/api/Usuario` - Gestión de usuarios
- **PUT** `/api/Usuario/{id}/rol` - Cambiar rol de usuario

---

## ✅ Requisitos previos

Antes de comenzar, asegurate de tener instalados los siguientes programas:

1. **.NET SDK (v8.0 o superior)**  
   Descargalo desde: https://dotnet.microsoft.com/download

2. **Node.js y npm**  
   Se recomienda la última versión LTS. Descargala desde: https://nodejs.org/

3. **MariaDB/MySQL (v8.4+ recomendado)**  
   Usuario: `root`, Password: `rootroot` (configurado en `appsettings.Development.json`)

---

## 🚀 Inicio rápido

### 1. Clonar el repositorio

```bash
git clone https://github.com/rockyet12/Gunter-Bar.git
cd Gunter-Bar
```

### 2. Backend (API)

```bash
cd Backend-Bar

# Restaurar dependencias
dotnet restore

# Compilar el proyecto
dotnet build Backend-Bar.sln

# Aplicar migraciones de base de datos
dotnet ef database update --project BarGunter.API

# Ejecutar el API (escucha en http://localhost:5172)
dotnet run --project BarGunter.API
```

**Swagger UI disponible en:** http://localhost:5172/swagger

### 3. Frontend (React)

```bash
cd Frontend-Bar

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm run dev
```

---

## 🔑 Uso de la API con Swagger

### 1. Registro de usuario
```json
POST /api/Usuario/register
{
  "nombre": "Juan Pérez",
  "email": "juan.perez@example.com", 
  "password": "Secreto123",
  "dni": 12345678
}
```

### 2. Login y obtención de JWT
```json
POST /api/Usuario/login
{
  "email": "juan.perez@example.com",
  "password": "Secreto123"
}
```

**Respuesta exitosa:**
```json
{
  "success": true,
  "message": "Login exitoso",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 3. Autorización en Swagger
1. Copia el valor del campo `token` de la respuesta del login
2. En Swagger UI, haz clic en el botón **"Authorize"**
3. En el campo, escribe: `Bearer [tu-token-aquí]`
4. Cierra el diálogo - ahora puedes usar endpoints protegidos

---

## 🗄️ Base de datos

### Configuración (appsettings.Development.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;PORT=3306;Database=BarGunterDb;Uid=root;Pwd=rootroot;"
  }
}
```

### Entidades principales
- **Usuario**: Gestión de usuarios con roles (Admin/Cliente)
- **Producto**: Productos del bar con categorías
- **Categoria**: Categorización de productos  
- **Trago**: Recetas y preparaciones especiales
- **Pedido**: Órdenes de clientes
- **Carrito**: Carritos de compra
- **Ticket**: Comprobantes y facturación
- **Tipo**: Clasificación adicional

---

## 🛠️ Scripts disponibles

### Backend
```bash
# Script de rebuild completo
./Backend-Bar/scripts/rebuild.sh

# Compilación manual
dotnet build Backend-Bar.sln

# Ejecutar con URL específica  
dotnet run --project BarGunter.API --urls "http://localhost:5172"
```

### Frontend  
```bash
# Desarrollo
npm run dev

# Build para producción
npm run build

# Preview del build
npm run preview
```

---

## 📋 Notas técnicas

### Arquitectura
- **Clean Architecture**: Domain, Application, Infrastructure, API
- **Entity Framework Core**: ORM para acceso a datos
- **JWT Authentication**: Tokens Bearer con roles
- **Swagger/OpenAPI**: Documentación interactiva
- **Repository Pattern**: Abstracción de acceso a datos
- **Dependency Injection**: Configurado en Program.cs

### Configuraciones importantes
- **CORS**: Configurado para desarrollo local
- **JWT Secret**: `TucodigodeseguridadWAZAAAAAAA!!` (cambiar en producción)
- **Puerto API**: 5172 (configurable en launchSettings.json)
- **Base de datos**: MariaDB/MySQL con migraciones automáticas

### Backups y limpieza
- Archivos de backup en `Backend-Bar/backups/`
- Duplicados eliminados durante la limpieza del proyecto
- Git history preservado con commits descriptivos

---

## 🚨 Troubleshooting

### Error de puerto ocupado
```bash
# Encontrar proceso en puerto 5172
lsof -i :5172

# Terminar proceso
kill [PID]
```

### Problemas de compilación
```bash
# Limpiar y rebuild
dotnet clean
dotnet restore  
dotnet build
```

### Base de datos
```bash
# Recrear migraciones
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

```bash
npm install
```

---

## 🚀 Iniciar el proyecto

### Iniciar el backend

```bash
cd ../Backend-Bar
dotnet run
```

### Iniciar el frontend

```bash
cd ../Frontend-Bar
npm run dev
```

Esto iniciará el servidor de desarrollo del frontend.

---

## 🔧 Notas adicionales

- Asegurate de tener creada la base de datos que indiques en la cadena de conexión.
- Podés usar herramientas como **MySQL Workbench** o **DBeaver** para gestionar la base de datos.
- Si el backend no puede conectarse, revisá bien la cadena de conexión y que el servidor de MySQL esté funcionando.
- Si tenés problemas, verificá que las versiones de las herramientas instaladas sean compatibles con los requisitos del proyecto.

---

¡Gracias por usar **Gunter-Bar**! 🍺