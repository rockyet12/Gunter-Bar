# 🐳 Gunter Bar - Docker Deployment

[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=flat&logo=docker)](https://www.docker.com/)
[![Docker Compose](https://img.shields.io/badge/Docker_Compose-3.8-2496ED?style=flat&logo=docker)](https://docs.docker.com/compose/)
[![Multi-Stage](https://img.shields.io/badge/Multi--Stage-Builds-2496ED?style=flat&logo=docker)](https://docs.docker.com/develop/dev-best-practices/)

## 📋 Descripción Técnica

**Gunter Bar Docker** proporciona una configuración completa de contenedorización para el despliegue simplificado y consistente del sistema de gestión de bar. Utiliza Docker Compose para orquestar múltiples servicios (Backend .NET, Frontend React, Base de datos MySQL) en un entorno aislado y reproducible.

### 🎯 Características Técnicas de Dockerización

- **🏗️ Multi-Stage Builds**: Optimización de imágenes para producción
- **📦 Layer Caching**: Aceleración de builds con cache inteligente
- **🔒 Security**: Imágenes base oficiales y actualizadas
- **⚡ Performance**: Configuración optimizada para desarrollo y producción
- **🔄 Hot Reload**: Desarrollo con hot reloading en contenedores
- **📊 Health Checks**: Monitoreo automático de salud de servicios
- **🔗 Service Discovery**: Comunicación interna entre contenedores
- **💾 Persistent Storage**: Volúmenes para datos persistentes

## 🚀 Stack Tecnológico Docker

### Contenedores Principales
```yaml
services:
  db:           # MySQL 8.0 - Base de datos persistente
  backend:      # ASP.NET Core 9.0 - API REST
  frontend:     # React 18 + Vite - Interfaz web
```

### Tecnologías de Contenedorización
```json
{
  "ContainerRuntime": "Docker Engine 24.0+",
  "Orchestration": "Docker Compose v3.8",
  "BaseImages": {
    "Database": "mysql:8.0 (Official)",
    "Backend": "mcr.microsoft.com/dotnet/aspnet:9.0 (Official)",
    "Frontend": "node:18-alpine (Official)"
  },
  "Networking": "Docker networks (bridge)",
  "Storage": "Named volumes + Bind mounts",
  "Security": "Non-root users + Read-only filesystems"
}
```

## 🏗️ Arquitectura Docker

### Diagrama de Servicios

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend      │    │    Backend      │    │   Database      │
│   (React)       │◄──►│  (ASP.NET Core) │◄──►│   (MySQL)       │
│   Port: 3000    │    │   Port: 5221    │    │   Port: 3306    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         └───────────────────────┼───────────────────────┘
                                 │
                    ┌─────────────────┐
                    │  Docker Network │
                    │  gunter-network │
                    └─────────────────┘
```

### Comunicación Entre Servicios

#### Backend ↔ Database
```yaml
# docker-compose.yml
backend:
  depends_on:
    db:
      condition: service_healthy
  environment:
    - ConnectionStrings__DefaultConnection=server=db;port=3306;database=gunterbar;user=gunter;password=gunterpass
```

#### Frontend ↔ Backend
```javascript
// Frontend/src/services/api.ts
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5221/api';
```

### Volúmenes y Persistencia

#### Named Volumes
```yaml
volumes:
  db_data:          # Persistencia de datos MySQL
  node_modules:     # Cache de dependencias Node.js
```

#### Bind Mounts (Desarrollo)
```yaml
volumes:
  - ./backend:/app  # Código fuente backend
  - ./frontend:/app # Código fuente frontend
  - /app/node_modules # Exclusión de node_modules
```

## 📁 Estructura de Archivos Docker

```
Gunter-Bar/
│
├── docker-compose.yml              # 🐙 Configuración principal
├── docker-compose.override.yml     # 🔧 Overrides para desarrollo
├── docker-compose.prod.yml         # 🚀 Configuración producción
│
├── backend/
│   ├── Dockerfile                  # 🏗️ Build backend
│   ├── Dockerfile.dev              # 🛠️ Desarrollo backend
│   └── appsettings.json            # ⚙️ Configuración .NET
│
├── frontend/
│   ├── Dockerfile                  # 🏗️ Build frontend
│   ├── Dockerfile.dev              # 🛠️ Desarrollo frontend
│   └── nginx.conf                  # 🌐 Configuración Nginx
│
├── docker/
│   ├── .env.example                # 📋 Variables ejemplo
│   ├── init.sql                    # 🗄️ Script inicialización BD
│   └── wait-for-it.sh              # ⏳ Script espera servicios
│
└── .dockerignore                   # 🚫 Exclusiones Docker
```

## 🛠️ Instalación y Configuración

### Prerrequisitos del Sistema
```bash
# Sistema Operativo
- Windows 10/11 Pro/Education, macOS 12+, Ubuntu 20.04+
- 8GB RAM mínimo, 16GB recomendado
- 20GB espacio en disco disponible

# Software Requerido
- Docker Desktop 4.0+ (https://www.docker.com/products/docker-desktop)
- Docker Compose V2 (incluido en Docker Desktop)
- Git 2.30+ (https://git-scm.com/)

# Verificar instalación
docker --version          # Docker Engine 24.0+
docker-compose --version  # Docker Compose v2.0+
```

### 🔧 Configuración Inicial

#### 1. Clonar Repositorio
```bash
git clone https://github.com/rockyet12/Gunter-Bar.git
cd Gunter-Bar
```

#### 2. Configurar Variables de Entorno
```bash
# Copiar archivo de ejemplo
cp docker/.env.example .env

# Configurar variables sensibles
# Editar .env con tus configuraciones
DB_ROOT_PASSWORD=tu_password_root_seguro
DB_PASSWORD=tu_password_db_seguro
JWT_SECRET=tu_clave_jwt_muy_segura_de_256_bits
```

#### 3. Construir y Levantar Servicios
```bash
# Construir imágenes y levantar contenedores
docker-compose up --build

# Ejecutar en segundo plano
docker-compose up --build -d

# Ver logs en tiempo real
docker-compose logs -f
```

### 📊 Verificación de Despliegue

#### Health Checks Automáticos
```bash
# Verificar estado de servicios
docker-compose ps

# Verificar health checks
docker ps --filter "health=healthy"

# Verificar conectividad
curl http://localhost:3000          # Frontend
curl http://localhost:5221/health   # Backend health check
curl http://localhost:5221/swagger  # API Documentation
```

#### Logs y Debugging
```bash
# Ver logs de todos los servicios
docker-compose logs

# Ver logs de un servicio específico
docker-compose logs backend
docker-compose logs frontend
docker-compose logs db

# Seguir logs en tiempo real
docker-compose logs -f backend
```

## 🚀 Modos de Despliegue

### 🛠️ Desarrollo (Hot Reload)

#### Configuración para Desarrollo
```yaml
# docker-compose.override.yml (automáticamente cargado)
version: '3.8'
services:
  backend:
    build:
      context: ./backend
      dockerfile: Dockerfile.dev
    volumes:
      - ./backend:/app
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - DOTNET_USE_POLLING_FILE_WATCHER=true

  frontend:
    build:
      context: ./frontend
      dockerfile: Dockerfile.dev
    volumes:
      - ./frontend:/app
      - /app/node_modules
    environment:
      - NODE_ENV=development
```

#### Flujo de Desarrollo
```bash
# Iniciar desarrollo
docker-compose up

# Los cambios en código se reflejan automáticamente:
# - Backend: Hot reload con dotnet watch
# - Frontend: Hot reload con Vite
# - Database: Datos persistentes entre reinicios
```

### 🚀 Producción (Optimizado)

#### Configuración para Producción
```yaml
# docker-compose.prod.yml
version: '3.8'
services:
  backend:
    build:
      context: ./backend
      dockerfile: Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:80

  frontend:
    build:
      context: ./frontend
      dockerfile: Dockerfile
    environment:
      - NODE_ENV=production
```

#### Despliegue en Producción
```bash
# Usar configuración de producción
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up --build -d

# Verificar despliegue
docker-compose ps
curl http://localhost:80
```

## 📊 Monitoreo y Mantenimiento

### 📈 Métricas de Rendimiento

#### Docker Stats
```bash
# Ver uso de recursos en tiempo real
docker stats

# Stats de contenedor específico
docker stats gunterbar-backend
```

#### Logs Estructurados
```bash
# Logs del backend con formato JSON
docker-compose logs backend | jq .

# Filtrar logs por nivel
docker-compose logs backend | grep "ERROR"

# Logs de los últimos 100 líneas
docker-compose logs --tail=100 backend
```

### 🔄 Operaciones de Mantenimiento

#### Backup de Base de Datos
```bash
# Backup de datos MySQL
docker exec gunterbar-mysql mysqldump -u gunter -pgunterpass gunterbar > backup_$(date +%Y%m%d_%H%M%S).sql

# Restaurar backup
docker exec -i gunterbar-mysql mysql -u gunter -pgunterpass gunterbar < backup.sql
```

#### Actualización de Contenedores
```bash
# Detener servicios
docker-compose down

# Limpiar imágenes no utilizadas
docker image prune -f

# Reconstruir y levantar
docker-compose up --build -d
```

#### Limpieza de Recursos
```bash
# Detener y remover contenedores
docker-compose down

# Remover volúmenes (⚠️ Elimina datos)
docker-compose down -v

# Limpiar imágenes, contenedores y redes no utilizados
docker system prune -a --volumes
```

## 🔧 Troubleshooting

### 🔍 Problemas Comunes y Soluciones

#### Puerto ya en uso
```bash
# Ver qué proceso usa el puerto
lsof -i :3000
netstat -tulpn | grep :3000

# Cambiar puerto en docker-compose.yml
ports:
  - "3001:3000"  # Cambiar host port
```

#### Error de conexión a base de datos
```bash
# Verificar que MySQL esté healthy
docker-compose ps

# Ver logs de MySQL
docker-compose logs db

# Verificar conectividad desde backend
docker exec gunterbar-backend ping db
```

#### Build falla por cache
```bash
# Limpiar cache de Docker
docker builder prune -f

# Reconstruir sin cache
docker-compose build --no-cache
```

#### Memoria insuficiente
```bash
# Verificar uso de memoria
docker stats

# Aumentar límite de memoria en Docker Desktop
# Settings > Resources > Memory > Aumentar a 4GB+
```

### 🐛 Debugging Avanzado

#### Acceder a contenedor en ejecución
```bash
# Shell interactivo en backend
docker exec -it gunterbar-backend bash

# Shell interactivo en base de datos
docker exec -it gunterbar-mysql mysql -u gunter -p gunterbar
```

#### Debug con VS Code
```json
// .vscode/launch.json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Docker .NET Core Attach",
      "type": "coreclr",
      "request": "attach",
      "processId": "${command:pickProcess}",
      "pipeTransport": {
        "pipeProgram": "docker",
        "pipeArgs": ["exec", "-i", "gunterbar-backend"],
        "debuggerPath": "/vsdbg/vsdbg",
        "pipeCwd": "${workspaceFolder}"
      }
    }
  ]
}
```

## 🔒 Seguridad Docker

### ✅ Mejores Prácticas Implementadas

#### Imágenes Base Oficiales
```dockerfile
# Dockerfile.backend
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
```

#### Usuario No-Root
```dockerfile
# Ejecutar como usuario no privilegiado
RUN addgroup -g 1001 -S appgroup && \
    adduser -S appuser -u 1001 -G appgroup

USER appuser
```

#### Secrets Management
```yaml
# docker-compose.yml
secrets:
  db_password:
    file: ./secrets/db_password.txt

services:
  db:
    secrets:
      - db_password
```

#### Network Security
```yaml
# Redes aisladas
networks:
  gunter-network:
    driver: bridge
    internal: true  # No expuesta externamente
```

## 📈 Optimización y Performance

### 🚀 Optimizaciones de Build

#### Multi-Stage Builds
```dockerfile
# Dockerfile.frontend
FROM node:18-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production

FROM nginx:alpine
COPY --from=builder /app/build /usr/share/nginx/html
```

#### Layer Caching
```dockerfile
# Copiar archivos de dependencias primero
COPY package*.json ./
RUN npm install

# Luego copiar código fuente
COPY . .
```

### 📊 Métricas de Optimización

#### Tamaño de Imágenes
```bash
# Ver tamaño de imágenes
docker images gunterbar-*

# Comparar tamaños
docker system df -v
```

#### Rendimiento de Contenedores
```bash
# Benchmark de inicio
time docker-compose up -d

# Ver uso de CPU/Memoria
docker stats --no-stream
```

## 🌐 Despliegue en la Nube

### ☁️ Docker en AWS

#### ECS (Elastic Container Service)
```yaml
# task-definition.json
{
  "family": "gunter-bar",
  "containerDefinitions": [
    {
      "name": "frontend",
      "image": "gunterbar/frontend:latest",
      "memory": 512,
      "essential": true,
      "portMappings": [
        {
          "containerPort": 80,
          "hostPort": 80
        }
      ]
    }
  ]
}
```

#### ECR (Elastic Container Registry)
```bash
# Login a ECR
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin <account>.dkr.ecr.us-east-1.amazonaws.com

# Tag y push
docker tag gunterbar-backend:latest <account>.dkr.ecr.us-east-1.amazonaws.com/gunterbar-backend:latest
docker push <account>.dkr.ecr.us-east-1.amazonaws.com/gunterbar-backend:latest
```

### 🚀 Docker en Vercel/Netlify

#### Vercel (Frontend)
```json
// vercel.json
{
  "version": 2,
  "builds": [
    {
      "src": "package.json",
      "use": "@vercel/static-build",
      "config": {
        "distDir": "build"
      }
    }
  ],
  "routes": [
    {
      "src": "/api/(.*)",
      "dest": "http://gunterbar-backend:5221/api/$1"
    }
  ]
}
```

## 📚 Documentación Adicional

### 📖 Recursos de Aprendizaje
- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Guide](https://docs.docker.com/compose/)
- [Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [Security Best Practices](https://docs.docker.com/engine/security/)

### 🛠️ Comandos Útiles
```bash
# Gestión de contenedores
docker-compose up -d          # Levantar servicios
docker-compose down           # Detener servicios
docker-compose restart        # Reiniciar servicios
docker-compose logs -f        # Ver logs

# Gestión de imágenes
docker-compose build          # Construir imágenes
docker image ls               # Listar imágenes
docker image rm <image>       # Remover imagen

# Debugging
docker exec -it <container> bash    # Acceder a shell
docker inspect <container>          # Ver configuración
docker network ls                   # Ver redes
```

## 👥 Equipo de Desarrollo

### 👨‍💻 Desarrollador Principal
**Roque Rivas** - *Full-Stack Developer & DevOps Engineer*  
📧 junior.rivaset12d1@gmail.com  
🔗 [GitHub](https://github.com/rockyet12) | [Instagram](https://instagram.com/roque.jr._.05)

### 🏫 Institución Educativa
**ET12 - Escuela Técnica N°12 D.E.1°**  
📍 Buenos Aires, Argentina  
🌐 [Sitio Web Institucional](http://et12.edu.ar)

### 👨‍🏫 Docentes
- **Sergio Mendoza** - Profesor de Desarrollo de Sistemas
- **Adrián Cives** - Coordinador de Proyecto

## 📞 Soporte y Contacto

### 🆘 Solución de Problemas
- **Documentación**: Leer logs detallados con `docker-compose logs`
- **Health Checks**: Verificar estado con `docker-compose ps`
- **Debugging**: Acceder a contenedores con `docker exec -it`
- **Comunidad**: [Docker Forums](https://forums.docker.com/)

### 📧 Contacto
- **Email**: junior.rivaset12d1@gmail.com
- **GitHub Issues**: Para reportar problemas específicos
- **Discord**: Canal técnico ET12

---

**🐳 Dockerización completa desarrollada en ET12**  
**Tecnologías: Docker + Docker Compose + Multi-stage builds**  
**Optimización: Imágenes pequeñas + Cache inteligente + Health checks**  
**Seguridad: Usuarios no-root + Redes aisladas + Secrets management**
