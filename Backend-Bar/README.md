# 🍸 Gunter-Bar Backend API

## Overview
Gunter-Bar Backend is a robust .NET 9 Web API designed for comprehensive bar management, featuring advanced user authentication, product catalog management, order processing, and table/ticket management.

## 🏗️ Architecture
Built following **Clean Architecture** principles:
- **BarGunter.API**: Presentation layer with controllers and middleware
- **BarGunter.Application**: Business logic and services
- **BarGunter.Domain**: Core entities and business rules
- **BarGunter.Infrastructure**: Data access and external services

## 🚀 Key Features

### 🔐 Authentication & Authorization
- **JWT Bearer Token** authentication
- **Role-based authorization**: Admin, Employee, Customer
- **Secure password management** with hashing
- **Token refresh** capabilities

### 📦 Product Management
- Complete **CRUD operations** for products
- **Category-based organization**
- **Promotional product** handling
- **Product cloning** for quick catalog expansion
- **Mass price updates** by category
- **Stock management** and alerts

### 🍺 Beverage Management
- **Drink catalog** with type categorization
- **Popular drinks** tracking
- **Type-based filtering** and search
- **Consumption analytics**

### 🧾 Ticket & Table Management
- **Real-time table status** tracking
- **Advanced ticket filtering** (date, table, status)
- **Automated ticket closure** with validation
- **Table turnover analytics**
- **Today's active tickets** dashboard

### 📋 Order Processing
- **Complete order lifecycle** management
- **Status-based filtering**
- **User order history**
- **Order analytics** and reporting
- **Processing optimization**

### 👥 User Management
- **Advanced user search** (name, email, role)
- **User profile management**
- **Password change** functionality
- **User status control** (active/inactive)
- **Comprehensive user analytics**

## 🛠️ Technology Stack

### Core Framework
- **.NET 9.0** - Latest Microsoft framework
- **ASP.NET Core Web API** - RESTful API development
- **Entity Framework Core 9.0** - ORM for database operations

### Database
- **MySQL/MariaDB** support via **Pomelo.EntityFrameworkCore.MySql 9.0**
- **Code-First migrations** for database schema management
- **Connection pooling** for optimal performance

### Authentication & Security
- **Microsoft.AspNetCore.Authentication.JwtBearer 9.0**
- **BCrypt** for password hashing
- **CORS** configuration for frontend integration
- **Role-based authorization** throughout the API

### Documentation & Testing
- **Swagger/OpenAPI** for API documentation
- **Swashbuckle.AspNetCore** for enhanced UI
- **Comprehensive error handling** and logging

## 📁 Project Structure

```
Backend-Bar/
├── BarGunter.API/                 # 🌐 Web API Layer
│   ├── Controllers/               # 🎮 API Controllers
│   │   ├── UserController.cs      # 👥 User management
│   │   ├── ProductController.cs   # 📦 Product operations
│   │   ├── DrinkController.cs     # 🍸 Beverage management
│   │   ├── TicketController.cs    # 🧾 Table/ticket operations
│   │   ├── OrderController.cs     # 📋 Order processing
│   │   └── ...                   # Other controllers
│   ├── Configuration/             # ⚙️ App configuration
│   │   ├── DependencyInjection.cs
│   │   ├── JwtSetup.cs
│   │   └── SwaggerSetup.cs
│   └── Program.cs                 # 🚀 Application entry point
├── BarGunter.Application/         # 💼 Business Logic Layer
│   ├── Services/                  # 🔧 Business services
│   ├── DTOs/                      # 📄 Data transfer objects
│   ├── Contracts/                 # 📋 Service interfaces
│   └── Common/                    # 🛠️ Shared utilities
├── BarGunter.Domain/              # 🏛️ Core Domain Layer
│   ├── Entities/                  # 🏗️ Domain entities
│   └── Enums/                     # 📊 Domain enumerations
└── BarGunter.Infrastructure/      # 🗃️ Data Access Layer
    ├── Data/                      # 🗄️ Database context
    ├── Repositories/              # 📚 Data repositories
    ├── Migrations/                # 🔄 Database migrations
    └── Settings/                  # ⚙️ Configuration settings
```

## 🔧 API Endpoints

### 🔐 Authentication
- `POST /api/User/login` - User authentication
- `POST /api/User/register` - User registration
- `PUT /api/User/change-password` - Password change

### 👥 User Management
- `GET /api/User` - Get all users (Admin only)
- `GET /api/User/search?term={term}` - Search users
- `GET /api/User/profile` - Get user profile
- `PUT /api/User/profile` - Update profile
- `GET /api/User/stats` - User statistics
- `PUT /api/User/{id}/toggle-status` - Toggle user status

### 📦 Product Management
- `GET /api/Product` - Get all products
- `GET /api/Product/{id}` - Get product by ID
- `POST /api/Product` - Create new product
- `PUT /api/Product/{id}` - Update product
- `DELETE /api/Product/{id}` - Delete product
- `GET /api/Product/promotional` - Get promotional products
- `POST /api/Product/{id}/clone` - Clone product
- `PUT /api/Product/mass-price-update` - Mass price update

### 🍸 Beverage Management
- `GET /api/Drink` - Get all drinks
- `GET /api/Drink/by-type/{typeId}` - Filter by type
- `GET /api/Drink/stats` - Drink statistics
- `GET /api/DrinkType/popular` - Popular drink types

### 🧾 Ticket Management
- `GET /api/Ticket` - Get all tickets
- `GET /api/Ticket/filter` - Advanced filtering
- `GET /api/Ticket/today-active` - Today's active tickets
- `POST /api/Ticket/{id}/close` - Close ticket
- `GET /api/Ticket/stats` - Ticket statistics

### 📋 Order Management
- `GET /api/Order` - Get all orders
- `GET /api/Order/by-status/{status}` - Filter by status
- `GET /api/Order/user/{userId}` - User order history
- `GET /api/Order/stats` - Order statistics

## 🚦 Getting Started

### Prerequisites
- **.NET 9.0 SDK** or higher
- **MySQL/MariaDB** database server
- **Visual Studio 2022** or **VS Code** (recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/rockyet12/Gunter-Bar.git
   cd Gunter-Bar/Backend-Bar
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Update database connection**
   - Modify `appsettings.Development.json`
   - Set your MySQL connection string

4. **Apply database migrations**
   ```bash
   dotnet ef database update --project BarGunter.API
   ```

5. **Build the solution**
   ```bash
   dotnet build Backend-Bar.sln
   ```

6. **Run the API**
   ```bash
   dotnet run --project BarGunter.API
   ```

### 🌐 Access Points
- **API Base URL**: `http://localhost:5000` or `http://localhost:5172`
- **Swagger Documentation**: `http://localhost:5000/swagger`
- **Health Check**: `http://localhost:5000/health`

## 🔒 Security Features

### Authentication
- **JWT Bearer tokens** with configurable expiration
- **Role-based authorization** (Admin, Employee, Customer)
- **Secure password hashing** using BCrypt

### API Security
- **CORS** configuration for allowed origins
- **Request validation** and input sanitization
- **Rate limiting** capabilities
- **Error handling** without sensitive data exposure

### Database Security
- **Connection string** protection via configuration
- **SQL injection** prevention through EF Core
- **Data validation** at multiple layers

## 📊 Monitoring & Logging

### Built-in Features
- **Comprehensive logging** throughout the application
- **Error tracking** with detailed stack traces
- **Performance monitoring** capabilities
- **Health checks** for system status

### Swagger Integration
- **Interactive API documentation**
- **Request/response examples**
- **Authentication testing** interface
- **Schema validation** display

## 🔄 Recent Updates

### .NET 9 Migration (October 2025)
- ✅ **Complete framework upgrade** from .NET 8 to .NET 9
- ✅ **All NuGet packages updated** to latest versions
- ✅ **CI/CD pipeline** updated for .NET 9 compatibility
- ✅ **Performance improvements** from latest framework features

### Enhanced Controller Features
- ✅ **Advanced search capabilities** across all entities
- ✅ **Comprehensive analytics** and reporting endpoints
- ✅ **Mass operations** for efficient bulk processing
- ✅ **Real-time status tracking** for tables and orders
- ✅ **Enhanced error handling** and logging

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is part of a school assignment for **Escuela Técnica N° 12 D.E. 1°** and is intended for educational purposes.

## 🏫 Academic Information

- **School**: Escuela Técnica N° 12 D.E. 1° "Libertador Gral. José de San Martín" (ET12)
- **Subject**: Desarrollo de Sistemas
- **Academic Year**: 2025
- **Class**: 6°17°
- **Students**: Sofia Colman, Camila Reyes, Ana Martinez, Roque Rivas, Julio Martinez
- **Teachers**: Sergio Mendoza, Adrián Cives
