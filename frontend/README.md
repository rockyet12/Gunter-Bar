# Bar Gunter - Frontend

Este es el frontend de la aplicación Bar Gunter, desarrollada con React y TypeScript.

## Estructura del Proyecto

```
frontend/
├── src/                      # Código fuente principal
│   ├── features/            # Funcionalidades principales organizadas por dominio
│   │   └── auth/           # Característica de autenticación
│   │       ├── components/  # Componentes de UI para autenticación
│   │       │   ├── LoginForm.tsx
│   │       │   └── RegisterForm.tsx
│   │       ├── contexts/    # Contextos de React para estado global
│   │       │   └── AuthContext.tsx
│   │       ├── hooks/       # Hooks personalizados
│   │       │   └── useAuth.ts
│   │       └── services/    # Servicios y lógica de negocio
│   │           └── authService.ts
│   │
│   ├── core/               # Configuraciones centrales y utilidades
│   │   ├── theme.ts        # Configuración del tema de Material-UI
│   │   └── components/     # Componentes compartidos core
│   │
│   ├── shared/            # Recursos compartidos entre características
│   │   ├── components/    # Componentes reutilizables
│   │   ├── hooks/        # Hooks compartidos
│   │   └── services/     # Servicios compartidos
│   │
│   ├── types/            # Definiciones de tipos globales
│   │   └── index.ts
│   │
│   ├── App.tsx           # Componente raíz de la aplicación
│   └── Routes.tsx        # Configuración de rutas principal
```

## Características Principales

### Sistema de Autenticación

El sistema de autenticación está implementado usando React Context y hooks personalizados:

- **LoginForm**: Permite a los usuarios iniciar sesión
  - Validación de campos
  - Manejo de errores
  - Redirección automática

- **RegisterForm**: Permite registrar nuevos usuarios
  - Validación de formulario
  - Creación de cuenta
  - Inicio de sesión automático

- **AuthContext**: Maneja el estado global de autenticación
  - Token JWT
  - Información del usuario
  - Estado de autenticación

### Rutas y Navegación

Utilizamos React Router v6 para el manejo de rutas:

- Rutas públicas (/login, /register)
- Rutas protegidas (requieren autenticación)
- Redirección automática basada en autenticación

## Tecnologías Utilizadas

- **React 18**: Framework principal
- **TypeScript**: Añade tipado estático
- **Material-UI**: Componentes y sistema de diseño
- **React Router**: Navegación y rutas
- **Formik** (próximamente): Manejo de formularios
- **Yup** (próximamente): Validación de datos

## Scripts Disponibles

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

## Convenciones de Código

### Estructura de Carpetas

- **features/**: Cada característica tiene su propia carpeta con:
  - `components/`: Componentes específicos
  - `contexts/`: Estado global relacionado
  - `hooks/`: Lógica reutilizable
  - `services/`: Comunicación con APIs

- **shared/**: Componentes y utilidades compartidas

### Componentes

- Un componente por archivo
- Nombres en PascalCase
- Exportaciones nombradas
- Props tipadas con TypeScript

### Estilos

- Material-UI para componentes base
- Tema personalizado en `core/theme.ts`
- Estilos en línea con `sx` prop

## Variables de Entorno

El proyecto utiliza las siguientes variables de entorno:

```env
REACT_APP_API_URL=http://localhost:3000/api
```

## Próximas Características

1. Dashboard de administración
2. Gestión de productos
3. Sistema de pedidos
4. Integración de pagos
5. Gestión de inventario

## Guía de Contribución

1. Crear rama desde `main`
2. Seguir convenciones de código
3. Asegurar que pase el linting
4. Crear Pull Request
