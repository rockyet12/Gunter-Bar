# Gunter Bar - Seller Frontend

Frontend administrativo para vendedores del sistema Gunter Bar. Esta aplicación permite a los vendedores gestionar sus productos, pedidos y obtener estadísticas de su negocio.

## Características

- **Dashboard completo** con estadísticas en tiempo real
- **Gestión de productos** (agregar, editar, eliminar)
- **Gestión de pedidos** y estados
- **Reportes y analytics**
- **Interfaz optimizada** para administración

## Tecnologías

- **React 18** con TypeScript
- **Vite** para desarrollo rápido
- **Tailwind CSS** para estilos
- **React Router** para navegación
- **Axios** para API calls
- **React Hook Form** con Yup para formularios

## Instalación

```bash
# Instalar dependencias
npm install

# Iniciar servidor de desarrollo
npm run dev
```

La aplicación estará disponible en `http://localhost:5174`

## Configuración

Crear un archivo `.env` en la raíz del proyecto:

```env
VITE_API_URL=http://localhost:5000/api
```

## Estructura del proyecto

```
seller-frontend/
├── src/
│   ├── components/
│   │   ├── forms/          # Componentes de formularios
│   │   └── common/         # Componentes reutilizables
│   ├── pages/              # Páginas principales
│   ├── utils/              # Utilidades (API, helpers)
│   ├── models/             # Interfaces TypeScript
│   └── layout/             # Layouts de la aplicación
├── public/                 # Archivos estáticos
└── dist/                   # Build de producción
```

## API Endpoints

La aplicación se conecta al backend compartido que incluye:

- `/api/auth/*` - Autenticación
- `/api/drinks/*` - Gestión de productos
- `/api/orders/*` - Gestión de pedidos
- `/api/reviews/*` - Reseñas

## Desarrollo

```bash
# Modo desarrollo
npm run dev

# Build para producción
npm run build

# Preview del build
npm run preview

# Linting
npm run lint
```

## Despliegue

La aplicación puede desplegarse en cualquier servicio que soporte aplicaciones React:

- **Vercel**
- **Netlify** 
- **Railway**
- **Docker**

## Roles y permisos

Esta aplicación está diseñada exclusivamente para usuarios con rol `Seller`. Los usuarios con rol `User` serán redirigidos automáticamente.
