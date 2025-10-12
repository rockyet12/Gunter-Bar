# Gunter-Bar

Este proyecto contiene el **backend** y el **frontend** de **Gunter-Bar**, una aplicación diseñada para gestionar una tienda online de bebidas con funcionalidades avanzadas de mixología, gestión de pedidos, combos y eventos.

---

## Trabajo Práctico – Escuela Técnica N° 12 D.E. 1° "Libertador Gral. José de San Martín" (ET12)

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

## Visión
"Gunter Bar", un comercio electrónico que trasciende la simple venta de bebidas para convertirse en un sitio web de experiencias y aprendizaje. Será la referencia para entusiastas que buscan licores e insumos de coctelería de alta calidad, con asesoramiento experto para dominar el arte del cóctel en casa.  
El sitio destacará por un diseño visualmente atractivo e intuitivo, con contenido de alto valor, brindando una experiencia memorable y enriquecedora.

## Propuesta
Catálogo curado de bebidas (destilados, vinos, cervezas artesanales y mixers exclusivos) + "Academia de Cócteles" interactiva. Cada compra inicia una aventura de sabor con guía profesional.

### E‑commerce
- Catálogo impecable: fotos de alta calidad, descripciones con notas de cata y recomendaciones de maridaje/coctelería.
- Kits temáticos de coctelería: p. ej. "Kit Old Fashioned Clásico" con ingredientes + enlace al tutorial.
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
Posicionar "El Arte del Cóctel" como plataforma líder de e‑commerce de bebidas premium y escuela de mixología digital, equilibrando facturación por productos y construcción de comunidad de aprendizaje.

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
- Servidor web con capacidad para alta concurrencia
- Base de datos robusta para gestión de inventario y usuarios
- Sistema de pagos integrado y seguro
- CDN para entrega rápida de contenido multimedia
- Sistemas de backup y recuperación

### Software y servicios
- Plataforma de e‑commerce escalable
- CMS para gestión de contenido de la academia
- Sistema de gestión de inventario en tiempo real
- Herramientas de analítica y métricas
- Integración con redes sociales y marketing digital

### DevOps
- Integración continua y deployment automatizado
- Monitoreo de performance y disponibilidad
- Gestión de logs centralizados
- Automatización de testing
- Escalado automático según demanda

### Arquitectura técnica y entornos
- Desarrollo: ambiente local con hot reload
- Testing: ambiente de pruebas automatizadas
- Staging: réplica de producción para validación final
- Producción: ambiente optimizado y monitoreado
- Entornos containerizados para consistencia

### Seguridad
- App:
  - Autenticación JWT; autorización por roles (Cliente, Empleado, Jefe de Ventas)
  - Validación de inputs, límites de tamaño, saneamiento y registro de auditoría
- Infra:
  - TLS, headers seguros (HSTS, CSP, no‑sniff), firewall y protección DDoS
  - Parcheo y actualizaciones regulares
- Operación:
  - Gestión de contraseñas y 2FA en cuentas críticas
  - Auditoría de accesos y operaciones sensibles

### Backups y continuidad
- Base de datos:
  - Backup diario cifrado; retención 7–30 días; prueba de restauración mensual
- Media y configuración:
  - Código versionado (Git); artefactos reproducibles
- Recuperación:
  - Plan de recuperación ante desastres
  - Pruebas regulares de restauración

### Logística operativa
- Modos:
  - Retiro en local, envío local por zona, delivery a domicilio
- Estados:
  - Pedido: Recibido → Pagado → En preparación → En camino → Entregado | Cancelado
- Tracking:
  - Sistema de seguimiento en tiempo real
  - Notificaciones automáticas por email/SMS

### Roles, permisos y módulos

#### Roles
- Cliente (Customer): navegar, comprar, ver pedidos y seguimiento, acceso a academia.
- Empleado (Employee): gestionar productos, ver pedidos, preparar despachos, solo lectura en precios.
- Jefe de Ventas (SalesManager): vista y control global, modificar precios, gestión de usuarios, reportes avanzados.

#### Módulos principales
- Productos: catálogo, stock, combos, promociones.
- Pedidos/Checkout: carrito, pago, selección de envío/dirección.
- Logística: cotización, etiquetas, tracking en tiempo real.
- Academia: recetas de cócteles, videotutoriales, guías de maridaje.
- Eventos: promociones especiales, cupones de bienvenida.
- Administración: gestión de roles, reportes de ventas, analytics.

---

## 📁 Estructura del Proyecto

```
Gunter-Bar/
├── backend/                     # Backend .NET 9
│   ├── GunterBar.Domain/        # Entidades y reglas de negocio
│   ├── GunterBar.Application/   # Servicios y lógica de aplicación
│   ├── GunterBar.Infrastructure/# Acceso a datos y servicios externos
│   ├── GunterBar.API/          # API REST y controllers
│   └── GunterBar.sln           # Solución del backend
└── frontend/                   # Frontend React + TypeScript
    └── (estructura del frontend)
```

---

*La documentación técnica detallada se agregará progresivamente durante el desarrollo del proyecto.*
