<div align="center">

# 🧬 BioTech-Backend

### Enterprise-Grade Backend Solution Built with Clean Architecture

![.NET](https://img.shields.io/badge/.NET%2010.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Clean%20Architecture-00ADD8?style=for-the-badge&logo=architecture&logoColor=white)

---

</div>

## 📋 Table of Contents

- [🏗️ Architecture Overview](#️-architecture-overview)
- [� Project Structure](#-project-structure)
- [🎯 Layer Responsibilities](#-layer-responsibilities)
  - [Domain Layer](#1-domain-layer)
  - [Application Layer](#2-application-layer)
  - [Infrastructure Layer](#3-infrastructure-layer)
  - [API Layer](#4-api-layer)
  - [Test Layer](#5-test-layer)
- [🔄 Dependency Flow](#-dependency-flow)
- [🚀 Getting Started](#-getting-started)
- [🛠️ Technologies & Patterns](#️-technologies--patterns)
- [📌 Development Guidelines](#-development-guidelines)
- [📞 Contact](#-contact)

---

## 🏗️ Architecture Overview

This project implements **Clean Architecture** (also known as Onion Architecture or Hexagonal Architecture), a software design philosophy that separates concerns into distinct layers, each with specific responsibilities. The architecture ensures:

- ✅ **Independence of Frameworks**: Business logic doesn't depend on external libraries
- ✅ **Testability**: Business rules can be tested without UI, database, or external services
- ✅ **Independence of UI**: The UI can change without affecting business logic
- ✅ **Independence of Database**: Business rules are not bound to a specific database
- ✅ **Independence of External Services**: Business logic doesn't know about external interfaces

### Architecture Diagram

```
┌─────────────────────────────────────────────────────────┐
│                      API Layer                          │
│  (Controllers, Middlewares, Filters, Extensions)        │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                 Application Layer                       │
│     (Use Cases, DTOs, Interfaces, Validators)           │
└────────────────────┬────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────┐
│                  Domain Layer                           │
│  (Entities, Value Objects, Domain Events, Exceptions)   │
└─────────────────────────────────────────────────────────┘
                     ▲
┌────────────────────┴────────────────────────────────────┐
│               Infrastructure Layer                      │
│  (Persistence, Repositories, External Services, Auth)   │
└─────────────────────────────────────────────────────────┘
```

---

## � Project Structure

```
BioTech-Backend/
│
├── 📁 Domain/                    # Core business logic and entities
│   ├── Entities/                 # Domain entities
│   ├── Enums/                    # Domain enumerations
│   ├── Events/                   # Domain events
│   ├── Exceptions/               # Domain-specific exceptions
│   └── ValueObjects/             # Value objects
│
├── 📁 Application/               # Application business rules
│   ├── Common/                   # Shared application components
│   │   ├── Behaviors/            # Pipeline behaviors (validation, logging)
│   │   ├── Exception/            # Application exceptions
│   │   └── Mappings/             # Object mapping profiles
│   ├── DTOs/                     # Data Transfer Objects
│   ├── Features/                 # Feature-based organization (CQRS)
│   │   └── Animal/               # Example feature
│   │       ├── Commands/         # Write operations
│   │       ├── Queries/          # Read operations
│   │       └── Validators/       # Business rule validators
│   └── Interfaces/               # Application service interfaces
│
├── 📁 Infrastructure/            # External concerns implementation
│   ├── Authentication/           # Authentication & authorization
│   ├── Files/                    # File storage services
│   ├── Persistence/              # Database context & configurations
│   │   └── Configuration/        # Entity configurations
│   ├── Repositories/             # Repository implementations
│   └── Services/                 # External service implementations
│
├── 📁 API/                       # Presentation layer
│   ├── Common/                   # Shared API components
│   ├── Controllers/              # REST API controllers
│   ├── Extensions/               # Service collection extensions
│   ├── Filters/                  # Action filters
│   ├── Mappings/                 # API mapping configurations
│   ├── Middlewares/              # Custom middlewares
│   ├── Program.cs                # Application entry point
│   └── appsettings.json          # Configuration files
│
├── 📁 Test/                      # Unit and integration tests
│
├── 📁 Docker/                    # Docker configuration
│   └── Dockerfile                # Container definition
│
├── compose.yaml                  # Docker Compose orchestration
├── BioTech-Backend.sln          # Solution file
└── README.md                     # This file
```

---

## 🎯 Layer Responsibilities

### 1. **Domain Layer** 
**Location**: `/Domain`

The **innermost layer** containing the core business logic and enterprise rules. This layer has **NO dependencies** on any other layer.

#### 📁 Folder Structure & Responsibilities:

| Folder | Purpose | Description |
|--------|---------|-------------|
| **Entities/** | Domain Entities | Core business objects with identity (e.g., `User`, `Product`, `Order`). These are the heart of the business model and contain business logic. |
| **ValueObjects/** | Value Objects | Immutable objects defined by their attributes rather than identity (e.g., `Address`, `Money`, `Email`). They encapsulate domain concepts. |
| **Enums/** | Enumerations | Domain-specific enumerations that represent fixed sets of constants (e.g., `OrderStatus`, `UserRole`). |
| **Events/** | Domain Events | Events that represent something significant that happened in the domain (e.g., `OrderPlaced`, `UserRegistered`). |
| **Exceptions/** | Domain Exceptions | Custom exceptions specific to business rule violations (e.g., `InvalidEmailException`, `InsufficientStockException`). |

**Key Principles**:
- ✅ Pure business logic
- ✅ No external dependencies
- ✅ Framework-agnostic
- ✅ Highly testable

---

### 2. **Application Layer**
**Location**: `/Application`

Contains **application-specific business rules** and orchestrates the flow of data between the Domain and outer layers. Implements use cases.

#### 📁 Folder Structure & Responsibilities:

| Folder | Purpose | Description |
|--------|---------|-------------|
| **Features/** | Use Cases (CQRS) | Organized by feature, each containing Commands (write operations) and Queries (read operations). Implements the CQRS pattern. |
| **Features/*/Commands/** | Write Operations | Handles create, update, delete operations. Each command represents a single business action. |
| **Features/*/Queries/** | Read Operations | Handles data retrieval operations. Optimized for reading without business logic side effects. |
| **Features/*/Validators/** | Business Validators | FluentValidation rules ensuring business constraints are met before executing commands/queries. |
| **DTOs/** | Data Transfer Objects | Objects used to transfer data between layers. Decouples internal domain models from external contracts. |
| **Interfaces/** | Service Contracts | Defines contracts for services implemented in Infrastructure (e.g., `IEmailService`, `IFileStorage`). |
| **Common/Behaviors/** | Cross-Cutting Concerns | Pipeline behaviors for validation, logging, caching, transaction management. |
| **Common/Mappings/** | Object Mapping | AutoMapper profiles for mapping between entities and DTOs. |
| **Common/Exception/** | Application Exceptions | Application-level exceptions (e.g., `NotFoundException`, `ValidationException`). |

**Key Principles**:
- ✅ Depends only on Domain layer
- ✅ Defines interfaces for Infrastructure
- ✅ Contains use case logic
- ✅ Independent of UI and database

---

### 3. **Infrastructure Layer**
**Location**: `/Infrastructure`

Implements **external concerns** and provides concrete implementations of interfaces defined in the Application layer.

#### 📁 Folder Structure & Responsibilities:

| Folder | Purpose | Description |
|--------|---------|-------------|
| **Persistence/** | Database Context | Entity Framework Core DbContext and database-related configurations. |
| **Persistence/Configuration/** | Entity Configurations | Fluent API configurations for entity mappings, relationships, and constraints. |
| **Repositories/** | Data Access | Concrete implementations of repository interfaces. Handles data persistence and retrieval. |
| **Authentication/** | Auth Services | JWT token generation, password hashing, identity management, and authentication logic. |
| **Services/** | External Services | Implementations of external service interfaces (email, SMS, payment gateways, etc.). |
| **Files/** | File Management | File storage implementations (local storage, cloud storage like AWS S3, Azure Blob). |

**Key Principles**:
- ✅ Implements Application interfaces
- ✅ Depends on Application and Domain
- ✅ Contains all external dependencies
- ✅ Easily replaceable implementations

---

### 4. **API Layer**
**Location**: `/API`

The **presentation layer** that exposes the application through REST APIs. Handles HTTP requests and responses.

#### 📁 Folder Structure & Responsibilities:

| Folder | Purpose | Description |
|--------|---------|-------------|
| **Controllers/** | API Endpoints | REST API controllers that handle HTTP requests and delegate to Application use cases. |
| **Middlewares/** | Request Pipeline | Custom middlewares for exception handling, logging, request/response modification. |
| **Filters/** | Action Filters | Attribute-based filters for authorization, validation, caching, etc. |
| **Extensions/** | Service Registration | Extension methods for dependency injection configuration and service registration. |
| **Mappings/** | API Mappings | Mappings specific to API contracts (request/response models). |
| **Common/** | Shared Components | Shared utilities, constants, and helpers used across the API layer. |
| **Program.cs** | Entry Point | Application startup configuration, middleware pipeline, and service registration. |
| **appsettings.json** | Configuration | Application settings, connection strings, and environment-specific configurations. |

**Key Principles**:
- ✅ Thin layer (minimal logic)
- ✅ Delegates to Application layer
- ✅ Handles HTTP concerns only
- ✅ API versioning and documentation
---

## 🔌 Available Endpoints (Gateway)

Base URL: `http://localhost:5000`

### 🔐 Authentication
| Method | Endpoint | Description | Body |
|--------|----------|-------------|------|
| `POST` | `/api/auth/login` | Obtain JWT Token | `{ "username": "...", "password": "..." }` |

### 🐄 Feeding Service (Requires Bearer Token)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/gateway/feeding/{id}` | Get feeding event by ID |
| `GET` | `/gateway/feeding/farm/{farmId}` | Get events by Farm ID |
| `GET` | `/gateway/feeding/batch/{batchId}` | Get events by Batch ID |
| `GET` | `/gateway/feeding/animal/{animalId}` | Get events by Animal ID |
| `POST` | `/gateway/feeding` | Create new feeding event |
| `PUT` | `/gateway/feeding` | Update feeding event |

---

### 5. **Test Layer**
**Location**: `/Test`

Contains **unit tests, integration tests, and end-to-end tests** to ensure code quality and correctness.

#### Test Strategy:

| Test Type | Target | Description |
|-----------|--------|-------------|
| **Unit Tests** | Domain & Application | Test business logic in isolation using mocks. |
| **Integration Tests** | Infrastructure | Test database operations, external services with real dependencies. |
| **API Tests** | API Controllers | Test HTTP endpoints, request/response handling. |

**Key Principles**:
- ✅ High code coverage
- ✅ Fast execution
- ✅ Isolated and repeatable
- ✅ Test business rules thoroughly

---

## 🔄 Dependency Flow

The dependency rule states that **dependencies only point inward**. Inner layers know nothing about outer layers.

```
API Layer
   ↓ (depends on)
Application Layer
   ↓ (depends on)
Domain Layer
   ↑ (implemented by)
Infrastructure Layer
```

### Dependency Injection

The **API layer** (Program.cs) is responsible for wiring up dependencies:

```csharp
// Domain has no dependencies

// Application depends on Domain
Application → Domain

// Infrastructure depends on Application and Domain
Infrastructure → Application → Domain

// API depends on Application (and transitively on Domain)
API → Application → Domain

// Infrastructure is injected into API at runtime
API → Infrastructure (runtime only)
```

---

## 🚀 Getting Started

### Prerequisites

- ✅ [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- ✅ [PostgreSQL 15+](https://www.postgresql.org/download/)
- ✅ [Docker](https://www.docker.com/get-started) (optional)
- ✅ [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Rider](https://www.jetbrains.com/rider/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-org/BioTech-Backend.git
   cd BioTech-Backend
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection string**
   
   Edit `API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=biotech;Username=postgres;Password=yourpassword"
     }
   }
   ```

4. **Run database migrations**
   ```bash
   dotnet ef database update --project Infrastructure --startup-project API
   ```

5. **Run the application**
   ```bash
   dotnet run --project API
   ```

6. **Access the API**
   - Swagger UI: `https://localhost:5001/swagger`
   - API: `https://localhost:5001/api`

### Using Docker

```bash
# Build and run with Docker Compose
docker-compose up --build

# Stop containers
docker-compose down
```

---

## 🛠️ Technologies & Patterns

### Core Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| **.NET** | 10.0 | Framework |
| **C#** | 12.0 | Programming Language |
| **ASP.NET Core** | 10.0 | Web API Framework |
| **Entity Framework Core** | 10.0 | ORM |
| **PostgreSQL** | 15+ | Database |
| **AutoMapper** | 12.0 | Object Mapping |

### Design Patterns & Principles

- ✅ **Clean Architecture** - Separation of concerns
- ✅ **CQRS** - Command Query Responsibility Segregation
- ✅ **Repository Pattern** - Data access abstraction
- ✅ **Dependency Injection** - Inversion of Control
- ✅ **Mediator Pattern** - Decoupled request handling
- ✅ **Unit of Work** - Transaction management
- ✅ **Domain-Driven Design** - Business-focused modeling
- ✅ **SOLID Principles** - Object-oriented design

### Additional Libraries (Recommended)

- **MediatR** - CQRS and Mediator pattern
- **FluentValidation** - Business rule validation
- **Serilog** - Structured logging
- **JWT Bearer** - Authentication
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Unit testing framework
- **Moq** - Mocking framework

---

## 📌 Development Guidelines

### 🌳 Git Flow - Branch Convention

#### Main Branches

| Branch | Purpose | Description |
|--------|---------|-------------|
| `main` | **Production** | Stable version deployed in production |
| `develop` | **Development** | Integration of new functionalities |

#### Working Branches

| Branch Type | Nomenclature | Purpose |
|-------------|--------------|---------|
| **Feature** | `feature/feature-name` | New backend functionalities |
| **Hotfix** | `hotfix/fix-name` | Critical fixes in production |
| **Release** | `release/vx.x.x` | Version preparation before production |

### 🧩 Commit Convention

**Format**: `<type>(<area>): <brief description>`

**Types**:
- `feat` - New functionality or endpoint
- `fix` - Bug fix
- `docs` - Documentation
- `style` - Formatting without affecting logic
- `refactor` - Code improvement or reorganization
- `test` - Unit / integration tests
- `chore` - Configuration, migrations, dependencies

**Examples**:
```bash
feat(api): add user registration endpoint
fix(database): correct SQL connection issue
test(services): add unit tests for UserService
docs(readme): update installation guide
```

---

## 📘 Best Practices

### Code Organization

1. **Feature-based organization** in Application layer
2. **One class per file** with meaningful names
3. **Async/await** for all I/O operations
4. **Dependency injection** for all services
5. **Interface segregation** - small, focused interfaces

### Error Handling

1. Use **custom exceptions** for business rules
2. Implement **global exception middleware**
3. Return **meaningful error messages**
4. Log exceptions with **correlation IDs**

### Security

1. **Never expose domain entities** directly through APIs
2. Use **DTOs** for all API contracts
3. Implement **authentication and authorization**
4. **Validate all inputs** using FluentValidation
5. Use **parameterized queries** to prevent SQL injection

### Testing

1. **Unit test** all business logic
2. **Integration test** database operations
3. **Mock external dependencies**
4. Maintain **high code coverage** (>80%)
5. Write **readable test names**

---

## � Contact

> 🔴 **These guidelines are MANDATORY for all team members.**

- 💬 Questions should be consulted with the **Scrum Master**
- 📢 Report blockers in the daily standup
- 📝 Document important decisions in the project

---

<div align="center">

### 🌟 Thank you for contributing to the BioTech project!

**Made with ❤️ by the development team**

[![GitHub](https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)](https://github.com)

---

© 2025 BioTech. All rights reserved.

</div>
