# 📝 Changelog

All notable changes to the Gunter-Bar project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2025-10-11

### 🚀 Major Changes

#### .NET 9 Upgrade
- **BREAKING**: Upgraded entire solution from .NET 8 to .NET 9
- Updated all project files to target `net9.0`
- Updated NuGet packages to .NET 9 compatible versions:
  - `Microsoft.AspNetCore.Authentication.JwtBearer` 8.0.0 → 9.0.0
  - `Microsoft.EntityFrameworkCore` 8.0.4 → 9.0.0
  - `Pomelo.EntityFrameworkCore.MySql` 8.0.0 → 9.0.0
- Updated GitHub Actions CI/CD pipeline for .NET 9 support
- Resolved CS5001 compilation errors caused by version mismatch

### ✨ New Features

#### 🔐 Enhanced User Management (UserController)
- **Advanced User Search**: Search users by name, email, or role with flexible filtering
- **User Statistics Dashboard**: Comprehensive analytics including total users, active/inactive counts, role distribution
- **Profile Management**: Complete user profile update functionality with validation
- **Secure Password Change**: Enhanced password change with current password verification
- **User Status Control**: Admin capability to toggle user active/inactive status
- **Enhanced Logging**: Detailed operation logging for security and audit purposes

#### 📦 Advanced Product Features (ProductController)
- **Promotional Products**: Endpoint to fetch current promotions and featured items
- **Product Cloning**: Quick product duplication for catalog expansion
- **Mass Price Updates**: Efficient bulk price updates by product category
- **Enhanced Product Analytics**: Detailed product statistics and reporting
- **Improved Error Handling**: Better error messages and validation

#### 🧾 Complete Ticket Management Overhaul (TicketController)
- **Advanced Filtering**: Filter tickets by date range, table number, and status
- **Real-time Active Tickets**: Get today's active tickets with live status updates
- **Smart Ticket Closure**: Automated ticket closure with business rule validation
- **Comprehensive Analytics**: Detailed ticket statistics and performance metrics
- **Table Management**: Enhanced table turnover and occupancy tracking

#### 🍺 Enhanced Beverage Management (DrinkTypeController)
- **Popular Types Tracking**: Endpoint to get trending drink categories
- **Advanced Search**: Enhanced search capabilities with type-specific filtering
- **Usage Analytics**: Track and report most popular drink types
- **Category Optimization**: Improved categorization and management features

#### 🍸 Improved Drink Operations (DrinkController)
- **Type-based Filtering**: Advanced filtering by drink types and categories
- **Drink Statistics**: Comprehensive beverage consumption analytics
- **Smart Recommendations**: Suggest drinks based on user preferences and trends
- **Inventory Integration**: Better stock management and low-stock alerts

#### 📋 Advanced Order Processing (OrderController)
- **Status-based Filtering**: Filter orders by processing status (pending, completed, cancelled)
- **User Order History**: Complete order tracking and history per user
- **Order Analytics**: Comprehensive order statistics and business insights
- **Process Optimization**: Streamlined order fulfillment workflow

### 🔧 Technical Improvements

#### Code Quality
- **Clean Architecture**: Maintained separation of concerns across all layers
- **Consistent Coding Standards**: Applied uniform coding conventions throughout
- **Enhanced Documentation**: Comprehensive inline documentation and XML comments
- **Error Handling**: Robust error management with proper HTTP status codes

#### Security Enhancements
- **Role-based Authorization**: Consistent authorization across all new endpoints
- **Input Validation**: Enhanced request validation and sanitization
- **Secure Logging**: Careful logging that doesn't expose sensitive information
- **JWT Security**: Improved token handling and validation

#### Performance Optimizations
- **Efficient Database Queries**: Optimized Entity Framework queries
- **Async Operations**: Consistent use of async/await patterns
- **Memory Management**: Improved resource utilization
- **Response Caching**: Strategic caching for frequently accessed data

#### Developer Experience
- **Swagger Documentation**: Enhanced API documentation with examples
- **Better Error Messages**: More descriptive error responses
- **Debugging Support**: Improved logging for development
- **Testing Support**: Better testability and mock-friendly design

### 🔨 Bug Fixes
- Fixed CS5001 compilation errors in Program.cs
- Resolved duplicate method issues in DrinkController and OrderController
- Fixed authentication token validation edge cases
- Corrected entity relationship mappings in EF Core
- Resolved CORS configuration issues

### 🗑️ Removed
- Removed duplicate UsuarioController file
- Cleaned up unused dependencies
- Removed deprecated .NET 8 specific code patterns

### 📚 Documentation Updates
- Updated README.md with .NET 9 requirements and new features
- Created comprehensive Backend README with detailed API documentation
- Updated project structure documentation
- Added troubleshooting guide for common issues
- Enhanced inline code documentation

### 🚧 Infrastructure Changes
- Updated Docker configurations for .NET 9 compatibility
- Modified GitHub Actions workflow for new .NET version
- Updated development environment setup instructions
- Enhanced build and deployment scripts

## [1.0.0] - 2025-04-19

### 🎉 Initial Release
- Initial project setup with .NET 8
- Basic CRUD operations for all entities
- JWT authentication implementation
- MySQL database integration with Entity Framework Core
- Clean Architecture foundation
- Basic API endpoints for User, Product, Drink, Order, and Ticket management
- Swagger/OpenAPI documentation
- Docker support for development
- Frontend React application with TypeScript

---

## Version History Summary

- **v2.0.0** (2025-10-11): Major .NET 9 upgrade with comprehensive feature enhancements
- **v1.0.0** (2025-04-19): Initial release with core functionality

## Migration Notes

### Upgrading from v1.x to v2.x
1. Update your development environment to .NET 9.0 SDK
2. All API endpoints remain backward compatible
3. New endpoints provide additional functionality without breaking existing integrations
4. Database schema remains unchanged - no migrations required
5. JWT tokens and authentication flow unchanged

### Recommended Actions
- Review new API endpoints in Swagger documentation
- Update any hardcoded .NET version references in deployment scripts
- Take advantage of new analytics and search capabilities
- Review enhanced error handling in client applications

## Support and Contact

For questions about this changelog or the project:
- **School**: Escuela Técnica N° 12 D.E. 1° "Libertador Gral. José de San Martín"
- **Subject**: Desarrollo de Sistemas
- **Teachers**: Sergio Mendoza, Adrián Cives
- **Students**: Sofia Colman, Camila Reyes, Ana Martinez, Roque Rivas, Julio Martinez
