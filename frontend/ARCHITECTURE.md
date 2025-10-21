# Arquitectura del Frontend - Bar Gunter

## 1. Diagrama de Arquitectura

```
┌──────────────────────────────────────────────────────────┐
│                      Aplicación                          │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐  │
│  │    Views    │    │   Features  │    │    Core     │  │
│  │  (Páginas)  │ ─► │  (Módulos)  │ ◄─ │(Servicios   │  │
│  │             │    │             │    │ Base)       │  │
│  └─────────────┘    └─────────────┘    └─────────────┘  │
│          ▲                 ▲                  ▲          │
│          │                 │                  │          │
│          ▼                 ▼                  ▼          │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐  │
│  │   Routes    │    │    Store    │    │  Services   │  │
│  │(Navegación) │ ◄► │  (Estado)   │ ◄► │   (APIs)    │  │
│  └─────────────┘    └─────────────┘    └─────────────┘  │
└──────────────────────────────────────────────────────────┘
```

## 2. Capas de la Aplicación

### 2.1 Capa de Presentación
```typescript
// Ejemplo de componente presentacional (src/features/auth/components/LoginForm.tsx)
export const LoginForm: React.FC = () => {
  const { login } = useAuth();
  // ... lógica del componente
  return (
    <form onSubmit={handleSubmit}>
      // ... elementos del formulario
    </form>
  );
};
```

### 2.2 Capa de Lógica de Negocio
```typescript
// Ejemplo de hook de negocio (src/features/auth/hooks/useAuth.ts)
export const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const login = async (credentials: LoginCredentials) => {
    // ... lógica de autenticación
  };
  return { isAuthenticated, login };
};
```

### 2.3 Capa de Servicios
```typescript
// Ejemplo de servicio (src/features/auth/services/authService.ts)
export class AuthService {
  async login(credentials: LoginCredentials): Promise<AuthResponse> {
    // ... llamada a API
  }
}
```

## 3. Organización del Código

### 3.1 Estructura por Características (Feature-First)
```
src/
├── features/          # Módulos de la aplicación
│   ├── auth/         # Módulo de autenticación
│   ├── products/     # Módulo de productos
│   └── orders/       # Módulo de pedidos
```

### 3.2 Shared Resources
```
src/
├── shared/
│   ├── components/   # Componentes reutilizables
│   ├── hooks/       # Hooks compartidos
│   └── utils/       # Utilidades generales
```

## 4. Flujo de Datos

```
┌──────────────┐     ┌───────────┐     ┌──────────┐
│  Componente  │ ──► │   Hook    │ ──► │ Servicio │
│   (Vista)    │     │ (Lógica)  │     │  (API)   │
└──────────────┘     └───────────┘     └──────────┘
       ▲                   │                 │
       │                   ▼                 ▼
       └───────────── Estado Local      API Externa
```

## 5. Patrones de Diseño Implementados

### 5.1 Custom Hooks Pattern
```typescript
// Ejemplo de custom hook
const useAuth = () => {
  // ... lógica
  return { user, login, logout };
};
```

### 5.2 Context Pattern
```typescript
// Ejemplo de contexto
const AuthContext = createContext<AuthContextType>(null);
```

### 5.3 Container/Presentational Pattern
```typescript
// Container
const LoginContainer = () => {
  const { login } = useAuth();
  return <LoginForm onSubmit={login} />;
};

// Presentational
const LoginForm = ({ onSubmit }) => {
  return <form onSubmit={onSubmit}>{/* ... */}</form>;
};
```

## 6. Manejo de Estado

### 6.1 Estado Local
- useState para estado componente-específico
- useReducer para estado complejo

### 6.2 Estado Global
- Context API para estado compartido
- AuthContext para estado de autenticación

## 7. Gestión de Rutas

```typescript
// src/Routes.tsx
const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/login" element={<LoginForm />} />
      <Route path="/" element={<PrivateRoute><Home /></PrivateRoute>} />
    </Routes>
  );
};
```

## 8. Integración con Backend

### 8.1 Servicios API
```typescript
// src/shared/services/apiService.ts
export const apiService = {
  get: async (url: string) => {
    const response = await fetch(url);
    return response.json();
  },
  // ... otros métodos
};
```

### 8.2 Interceptores
```typescript
// Manejo de errores y tokens
axiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

## 9. Seguridad

### 9.1 Protección de Rutas
```typescript
const PrivateRoute = ({ children }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" />;
};
```

### 9.2 Manejo de Tokens
```typescript
const setAuthToken = (token: string) => {
  localStorage.setItem('token', token);
  // ... lógica adicional
};
```

## 10. Optimizaciones

### 10.1 Code Splitting
```typescript
const Dashboard = lazy(() => import('./pages/Dashboard'));
```

### 10.2 Memoización
```typescript
const MemoizedComponent = memo(Component);
const memoizedValue = useMemo(() => computeValue(prop), [prop]);
```

## 11. Convenciones y Estándares

### 11.1 Nombrado
- PascalCase para componentes
- camelCase para funciones y variables
- UPPER_SNAKE_CASE para constantes

### 11.2 Tipos
```typescript
interface ComponentProps {
  // ... definición de props
}

type ComponentState = {
  // ... definición de estado
};
```

## 12. Próximos Pasos

1. Implementación de pruebas unitarias
2. Optimización de rendimiento
3. Internacionalización
4. PWA
5. Integración de análisis y monitoreo
