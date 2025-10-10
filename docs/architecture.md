# Project Architecture

This document defines the canonical folder structure for the Gunter-Bar monorepo (Backend .NET + Frontend Vite/React).

## Backend (Clean Architecture)

Backend-Bar/
- Backend-Bar.sln
- BarGunter.Domain/
  - Entities/
  - Enums/
  - ValueObjects/ (optional)
  - DomainEvents/ (optional)
- BarGunter.Application/
  - Contracts/
    - IRepositories/
    - IServices/
  - DTOs/
  - Services/
  - Common/
    - Behaviors/ (validation, pipelines)
    - Exceptions/
    - Interfaces/
- BarGunter.Infrastructure/
  - Persistences/
    - BarGunterDbContext.cs
    - Configurations/
    - Repositories/
    - Migrations/
  - Settings/
- BarGunter.API/
  - Controllers/
  - Configuration/
    - DependencyInjection.cs
    - JwtSetup.cs
    - SwaggerSetup.cs
  - Filters/ (optional)
  - Middlewares/ (optional)
  - Properties/
  - appsettings.json
  - appsettings.Development.json
  - Program.cs
- scripts/
  - rebuild.sh
  - migrate.sh
- tests/
  - BarGunter.Domain.Tests/
  - BarGunter.Application.Tests/
  - BarGunter.API.Tests/

Notes:
- EF Core Migrations live in Infrastructure/Persistences/Migrations.
- File-scoped namespaces across all projects.
- API-only configuration lives under BarGunter.API/Configuration.

## Frontend (Vite + React)

Frontend-Bar/
- src/
  - assets/
  - components/
  - pages/
  - hooks/
  - services/
    - api/ (HTTP clients)
  - store/ (optional: Zustand/Redux)
  - types/
  - utils/
  - routes/
  - App.jsx
  - main.jsx
- public/
- package.json
- vite.config.js
- eslint.config.js

## Repository-level hygiene

- Avoid duplicated nested trees (e.g., `Gunter-Bar/Gunter-Bar/...`). Keep a single canonical tree at repo root.
- Use solution references: API -> Application, Infrastructure; Application -> Domain; Infrastructure -> Domain (+ Application for interfaces) without circular refs.

## Mapping to current repo

- Current projects already exist under `Backend-Bar/*` and `Frontend-Bar/*`.
- Recommended next maintenance steps:
  1) Remove the nested duplicate tree `Gunter-Bar/Gunter-Bar/...` to prevent type/namespace confusion.
  2) Group API config into `BarGunter.API/Configuration` (Jwt, Swagger, DI wiring) for clarity.
  3) Ensure migrations reside under `Infrastructure/Persistences/Migrations`.

## Conventions

- C# .NET 8, nullable enabled, file-scoped namespaces (`namespace X;`).
- DTOs are defined in Application/DTOs and used by services/controllers.
- Repositories under Infrastructure implement interfaces from Application.
- Swagger includes JWT bearer security.
