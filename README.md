# Gunter-Bar

Este proyecto contiene el **backend** y el **frontend** de **Gunter-Bar**, una aplicación diseñada para gestionar un bar con autenticación JWT, CRUD de productos/categorías/usuarios y gestión de pedidos.

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
    Descargalo desde: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

2. **Node.js y npm**  
    Se recomienda la última versión LTS. Descargala desde: [https://nodejs.org/](https://nodejs.org/)

3. **MariaDB/MySQL (v8.4+ recomendado)**  
    Usuario: `root`, Password: `rootroot` (configurado en `appsettings.Development.json`)

---

## � Inicio rápido

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
