# 🏗️ Architecture Overview

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

## 📂 Project Structure

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
│   └── Interfaces/               # Application service interfaces
│
├── 📁 Infrastructure/            # External concerns implementation
│   ├── Authentication/           # Authentication & authorization
│   ├── Files/                    # File storage services
│   ├── Persistence/              # Database context & configurations
│   ├── Repositories/             # Repository implementations
│   └── Services/                 # External service implementations
│
├── 📁 API/                       # Presentation layer
│   ├── Controllers/              # REST API controllers
│   ├── Middlewares/              # Custom middlewares
│   └── Program.cs                # Application entry point
│
├── 📁 Test/                      # Unit and integration tests
│
├── 📁 Docker/                    # Docker configuration
│
├── compose.yaml                  # Docker Compose orchestration
└── BioTech-Backend.sln          # Solution file
```

---

## 🎯 Layer Responsibilities

### 1. **Domain Layer** 
**Location**: `/Domain`
The **innermost layer** containing the core business logic and enterprise rules. This layer has **NO dependencies** on any other layer.

### 2. **Application Layer**
**Location**: `/Application`
Contains **application-specific business rules** and orchestrates the flow of data between the Domain and outer layers. Implements use cases (CQRS).

### 3. **Infrastructure Layer**
**Location**: `/Infrastructure`
Implements **external concerns** (Database, Auth, File Storage) and provides concrete implementations of interfaces defined in the Application layer.

### 4. **API Layer**
**Location**: `/API`
The **presentation layer** that exposes the application through REST APIs. Handles HTTP requests and responses.

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
