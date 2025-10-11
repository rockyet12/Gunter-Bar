# 🌐 Gunter-Bar API Specification

## Overview
Comprehensive API specification for Gunter-Bar management system built with .NET 9. This document describes all available endpoints, authentication methods, and data schemas.

**Base URL**: `http://localhost:5000/api` (Development)  
**API Version**: v2.0.0  
**Framework**: .NET 9.0 with ASP.NET Core Web API  
**Authentication**: JWT Bearer Token  

---

## 🔐 Authentication

### Authentication Flow
All protected endpoints require a valid JWT Bearer token in the Authorization header:
```
Authorization: Bearer <jwt_token>
```

### Login Endpoint
```http
POST /api/User/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "John Doe",
    "email": "user@example.com",
    "role": "Customer"
  }
}
```

### Roles
- **Admin**: Full system access
- **Employee**: Operational access to products, orders, tickets
- **Customer**: Limited access to own orders and profile

---

## 👥 User Management API

### 🔍 Search Users
```http
GET /api/User/search?term={searchTerm}
Authorization: Bearer <token>
Roles: Admin, Employee
```
Search users by name, email, or role.

### 📊 User Statistics
```http
GET /api/User/stats
Authorization: Bearer <token>
Roles: Admin
```
**Response:**
```json
{
  "totalUsers": 150,
  "activeUsers": 142,
  "inactiveUsers": 8,
  "roleDistribution": {
    "Admin": 2,
    "Employee": 12,
    "Customer": 136
  },
  "recentRegistrations": 15
}
```

### 👤 User Profile
```http
GET /api/User/profile
Authorization: Bearer <token>

PUT /api/User/profile
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Updated Name",
  "email": "new@email.com",
  "phone": "+1234567890"
}
```

### 🔑 Change Password
```http
PUT /api/User/change-password
Authorization: Bearer <token>
Content-Type: application/json

{
  "currentPassword": "oldpass123",
  "newPassword": "newpass456"
}
```

### 🔄 Toggle User Status
```http
PUT /api/User/{id}/toggle-status
Authorization: Bearer <token>
Roles: Admin
```

---

## 📦 Product Management API

### 🎯 Promotional Products
```http
GET /api/Product/promotional
```
**Response:**
```json
[
  {
    "id": 1,
    "name": "Premium Whiskey",
    "originalPrice": 89.99,
    "promotionalPrice": 69.99,
    "discount": 22.22,
    "category": "Spirits",
    "isPromotional": true
  }
]
```

### 📋 Clone Product
```http
POST /api/Product/{id}/clone
Authorization: Bearer <token>
Roles: Admin, Employee
Content-Type: application/json

{
  "newName": "Cloned Product Name",
  "adjustPrice": 5.00
}
```

### 💰 Mass Price Update
```http
PUT /api/Product/mass-price-update
Authorization: Bearer <token>
Roles: Admin
Content-Type: application/json

{
  "categoryId": 1,
  "adjustmentType": "percentage", // or "fixed"
  "adjustmentValue": 10.0,
  "reason": "Seasonal adjustment"
}
```

---

## 🍸 Beverage Management API

### 🍺 Drinks by Type
```http
GET /api/Drink/by-type/{typeId}
```

### 📊 Drink Statistics
```http
GET /api/Drink/stats
Authorization: Bearer <token>
Roles: Admin, Employee
```
**Response:**
```json
{
  "totalDrinks": 89,
  "byCategory": {
    "Beer": 25,
    "Wine": 30,
    "Spirits": 20,
    "Cocktails": 14
  },
  "popularDrinks": [
    {
      "name": "Classic Mojito",
      "orderCount": 156,
      "revenue": 1248.00
    }
  ]
}
```

### 🔥 Popular Drink Types
```http
GET /api/DrinkType/popular
```
**Response:**
```json
[
  {
    "id": 1,
    "name": "Cocktails",
    "orderCount": 342,
    "popularityScore": 8.7
  }
]
```

---

## 🧾 Ticket Management API

### 🔍 Advanced Ticket Filtering
```http
GET /api/Ticket/filter?startDate=2025-10-01&endDate=2025-10-11&tableNumber=5&status=Active
Authorization: Bearer <token>
Roles: Admin, Employee
```

### 📅 Today's Active Tickets
```http
GET /api/Ticket/today-active
Authorization: Bearer <token>
Roles: Admin, Employee
```
**Response:**
```json
[
  {
    "id": 15,
    "tableNumber": 3,
    "openedAt": "2025-10-11T14:30:00Z",
    "totalAmount": 45.50,
    "itemCount": 4,
    "status": "Active"
  }
]
```

### 🔒 Close Ticket
```http
POST /api/Ticket/{id}/close
Authorization: Bearer <token>
Roles: Admin, Employee
Content-Type: application/json

{
  "paymentMethod": "Card",
  "tip": 8.50,
  "notes": "Customer satisfied"
}
```

### 📊 Ticket Statistics
```http
GET /api/Ticket/stats
Authorization: Bearer <token>
Roles: Admin, Employee
```
**Response:**
```json
{
  "todayStats": {
    "totalTickets": 23,
    "activeTickets": 8,
    "closedTickets": 15,
    "averageTicketValue": 32.45,
    "totalRevenue": 746.35
  },
  "tableOccupancy": {
    "totalTables": 12,
    "occupiedTables": 8,
    "occupancyRate": 66.67
  }
}
```

---

## 📋 Order Management API

### 🔍 Orders by Status
```http
GET /api/Order/by-status/{status}
Authorization: Bearer <token>
Roles: Admin, Employee

# Status values: Pending, Processing, Completed, Cancelled
```

### 👤 User Order History
```http
GET /api/Order/user/{userId}
Authorization: Bearer <token>
Roles: Admin, Employee (own orders only for Customers)
```

### 📊 Order Statistics
```http
GET /api/Order/stats
Authorization: Bearer <token>
Roles: Admin, Employee
```
**Response:**
```json
{
  "todayOrders": 45,
  "pendingOrders": 8,
  "processingOrders": 12,
  "completedToday": 25,
  "averageOrderValue": 28.75,
  "totalRevenue": 1293.75,
  "popularItems": [
    {
      "productName": "Classic Burger",
      "orderCount": 15,
      "revenue": 180.00
    }
  ]
}
```

---

## 🛒 Cart Management API

### 🛒 Cart Operations
```http
GET /api/Cart
Authorization: Bearer <token>

POST /api/Cart/add
Authorization: Bearer <token>
Content-Type: application/json

{
  "productId": 1,
  "quantity": 2
}

PUT /api/Cart/update
Authorization: Bearer <token>
Content-Type: application/json

{
  "productId": 1,
  "quantity": 3
}

DELETE /api/Cart/remove/{productId}
Authorization: Bearer <token>
```

---

## 📁 Category Management API

### 📂 Category Operations
```http
GET /api/Category
GET /api/Category/{id}

POST /api/Category
Authorization: Bearer <token>
Roles: Admin, Employee
Content-Type: application/json

{
  "name": "New Category",
  "description": "Category description"
}

PUT /api/Category/{id}
Authorization: Bearer <token>
Roles: Admin, Employee

DELETE /api/Category/{id}
Authorization: Bearer <token>
Roles: Admin
```

---

## 📊 Common Response Schemas

### Success Response
```json
{
  "success": true,
  "data": { /* response data */ },
  "message": "Operation completed successfully"
}
```

### Error Response
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid input provided",
    "details": [
      {
        "field": "email",
        "message": "Email is required"
      }
    ]
  }
}
```

### Pagination Response
```json
{
  "data": [ /* array of items */ ],
  "pagination": {
    "currentPage": 1,
    "totalPages": 10,
    "pageSize": 20,
    "totalItems": 200,
    "hasNext": true,
    "hasPrevious": false
  }
}
```

---

## 🔒 Security Headers

All API responses include security headers:
- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `X-XSS-Protection: 1; mode=block`
- `Strict-Transport-Security: max-age=31536000`

---

## 📈 Rate Limiting

- **Authenticated requests**: 1000 requests per hour
- **Public endpoints**: 100 requests per hour
- **Login attempts**: 5 attempts per 15 minutes

---

## 🚨 Error Codes

| Code | Description |
|------|-------------|
| 400 | Bad Request - Invalid input |
| 401 | Unauthorized - Missing or invalid token |
| 403 | Forbidden - Insufficient permissions |
| 404 | Not Found - Resource doesn't exist |
| 409 | Conflict - Resource already exists |
| 422 | Unprocessable Entity - Validation failed |
| 429 | Too Many Requests - Rate limit exceeded |
| 500 | Internal Server Error - Server error |

---

## 🔄 Versioning

API versioning is handled through headers:
```
API-Version: 2.0
```

Current version: **2.0** (October 2025)
Previous version: **1.0** (April 2025)

---

## 📝 Change Log

### v2.0.0 (October 2025)
- **NEW**: Advanced search endpoints for all entities
- **NEW**: Comprehensive statistics and analytics
- **NEW**: Mass operations for products
- **NEW**: Real-time ticket management
- **UPGRADED**: .NET 9 framework
- **ENHANCED**: All controllers with additional functionality

### v1.0.0 (April 2025)
- Initial API release
- Basic CRUD operations
- JWT authentication
- Core business entities

---

*This specification is automatically kept in sync with Swagger documentation available at `/swagger` endpoint.*
