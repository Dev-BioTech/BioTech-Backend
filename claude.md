# 🧬 BioTech-Backend — Descripción Completa del Proyecto

## 📋 Resumen General

**BioTech-Backend** es un sistema backend empresarial diseñado para la **gestión integral de fincas ganaderas**. Está construido con una **arquitectura de microservicios** usando **Clean Architecture** en **.NET 10 / C#**, con **PostgreSQL** como base de datos y **Docker Compose** para orquestación.

El sistema permite gestionar: autenticación de usuarios, fincas, animales y hatos, salud animal, alimentación, reproducción, inventario de productos, ventas, transacciones comerciales y un asistente de IA integrado.

---

## 🏗️ Arquitectura

### Patrón: Clean Architecture + Microservicios

Cada microservicio sigue una estructura de **4 capas**:

```
ServiceName.Domain        → Entidades, Value Objects, Enums, Interfaces de repositorio
ServiceName.Application   → Commands, Queries, Handlers (CQRS con MediatR), DTOs, Validators
ServiceName.Infrastructure→ Implementación de repositorios, DbContext (EF Core), Migraciones
ServiceName.Presentation  → Controllers (API REST), Program.cs, Middlewares, Dockerfile
```

### Patrones de Diseño Utilizados

| Patrón | Uso |
|---|---|
| **CQRS** | Separación de Commands (escritura) y Queries (lectura) con MediatR |
| **Mediator** | MediatR para desacoplar controllers de la lógica de negocio |
| **Repository** | Interfaces en Domain, implementaciones en Infrastructure |
| **API Gateway** | Ocelot para enrutamiento centralizado y autenticación |
| **Multi-Tenancy** | Aislamiento por `FarmId` extraído del JWT (contexto de finca) |
| **FluentValidation** | Validación de requests en la capa Application |

---

## 🔧 Stack Tecnológico

| Tecnología | Versión / Detalle |
|---|---|
| **.NET** | 10.0 |
| **C#** | Último estable |
| **PostgreSQL** | Clever Cloud (puerto 50013) |
| **Entity Framework Core** | ORM, Code-First con migraciones |
| **MediatR** | CQRS y patrón Mediator |
| **FluentValidation** | Validación de DTOs/Commands |
| **JWT** | Autenticación Bearer Token |
| **Ocelot** | API Gateway para routing |
| **Docker Compose** | Orquestación de contenedores |
| **Swagger/OpenAPI** | Documentación interactiva de APIs |

---

## 📦 Microservicios

### 1. 🔐 AuthService (Autenticación y Fincas)
- **Responsabilidad**: Login, registro de usuarios, perfiles, gestión de fincas (CRUD).
- **Entidades**: `User`, `Farm`, `Role`, `Permission`, `RolePermission`, `Tenant`, `UserFarmRole`
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `AUTH_DB`

### 2. 🐄 HerdService (Gestión de Hato)
- **Responsabilidad**: Registro y gestión de animales, lotes, razas, categorías, potreros, movimientos.
- **Entidades**: `Animal`, `Batch`, `Breed`, `AnimalCategory`, `Paddock`, `AnimalMovement`, `MovementType`
- **Enums**: `AnimalStatus`, `Gender`
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `HERD_DB`

### 3. 🏥 HealthService (Salud Animal)
- **Responsabilidad**: Registro de eventos sanitarios, dashboard de salud, tratamientos recientes, próximos eventos.
- **Entidades**: `HealthEvent`, `HealthEventDetail`, `Disease`, `WithdrawalPeriod`
- **Value Objects**: `Money`
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `HEALTH_DB`

### 4. 🌾 FeedingService (Alimentación)
- **Responsabilidad**: Registro de eventos de alimentación, consultas por finca/lote/animal/producto, recalculación de costos.
- **Entidades**: `FeedingEvent`
- **Value Objects**: `Money`
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `FEEDING_DB`

### 5. 📦 InventoryService (Inventario)
- **Responsabilidad**: Gestión de productos, ítems de inventario, movimientos (Kardex), alertas de stock bajo.
- **Entidades**: `Product`, `InventoryItem`, `InventoryMovement`
- **Enums**: `ProductCategory`, `UnitOfMeasure`, `MovementType`, `MovementDirection`, `MovementConcept`
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `INVENTORY_DB`

### 6. 🐣 ReproductionService (Reproducción)
- **Responsabilidad**: Eventos reproductivos (monta, inseminación, etc.), preñeces, partos.
- **Entidades**: `ReproductionEvent`, `Pregnancy`, `Birth`
- **Enums**: `ReproductionEventType`
- **Value Objects**: `EventDate`, `ExternalId`, `Observation`
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `REPRODUCTION_DB`

### 7. 💰 SalesService (Ventas)
- **Responsabilidad**: CRUD de ventas.
- **Entidades**: `Sale`
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `SALES_DB`

### 8. 🤝 CommercialService (Comercial)
- **Responsabilidad**: Gestión de terceros (proveedores/clientes), transacciones comerciales con detalles de animales y productos.
- **Entidades**: `ThirdParty`, `CommercialTransaction`, `TransactionAnimalDetail`, `TransactionProductDetail`
- **Enums**: `TransactionType`, `PaymentStatus`
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `COMMERCIAL_DB`

### 9. 🤖 AIService (Inteligencia Artificial)
- **Responsabilidad**: Chat inteligente (Gemini/Anthropic), diagnóstico de errores 502, análisis de patrones.
- **Entidades**: `DiagnosticSession`, `ErrorPattern`
- **APIs externas**: Gemini API, Anthropic API
- **Comunicación inter-servicio**: Consulta datos de HerdService, HealthService, FeedingService vía HTTP.
- **Puerto Docker**: 8080 (interno)
- **Base de datos**: `AI_DB` (comparte la misma BD que Auth)

### 10. 🌐 ApiGateway (Puerta de Enlace)
- **Responsabilidad**: Punto de entrada único, enrutamiento con Ocelot, autenticación JWT centralizada.
- **Puerto expuesto**: `5000:8080`
- **Configuración**: `ocelot.json`

---

## 🔒 Autenticación y Seguridad

1. **JWT Bearer Token**: El usuario se autentica vía `POST /api/v1/auth/login` y recibe un token JWT.
2. **Headers del Gateway**: El API Gateway inyecta headers con la información del usuario autenticado:
   - `X-User-Id`: ID del usuario
   - `X-Farm-Id`: ID de la finca seleccionada
   - `X-Gateway-Secret`: Secreto compartido para validar que las peticiones vienen del Gateway
3. **Contexto de Finca**: Cada servicio extrae `FarmId` del token/headers para filtrar datos (multi-tenancy).

---

## 🐳 Despliegue

### Docker Compose
- Todos los servicios corren en la red `biotech-network`.
- Cada servicio tiene su propio `Dockerfile` basado en multi-stage build.
- Variables de entorno configuradas en `.env`.
- Solo el API Gateway expone el puerto `5000`.

### Bases de Datos
- Cada servicio tiene su propia base de datos PostgreSQL en **Clever Cloud** (puerto `50013`).
- Excepción: AIService comparte la BD con AuthService.

---

## 📂 Estructura del Proyecto

```
BioTech-Backend/
├── ApiGateWay/                    # Gateway con Ocelot
├── AuthService.{Domain,Application,Infrastructure,Presentation}/
├── HerdService.{Domain,Application,Infrastructure,Presentation}/
├── HealthService.{Domain,Application,Infrastructure,Presentation}/
├── FeedingService.{Domain,Application,Infrastructure,Presentation}/
├── InventoryService.{Domain,Application,Infrastructure,Presentation}/
├── ReproductionService.{Domain,Application,Infrastructure,Presentation}/
├── SalesService.{Domain,Application,Infrastructure,Presentation}/
├── CommercialService.{Domain,Application,Infrastructure,Presentation}/
├── AIService.{Domain,Application,Infrastructure,Presentation}/
├── Shared.Infrastructure/         # Código compartido (ApiResponse, Common)
├── *.Tests/                       # Proyectos de pruebas unitarias
├── compose.yaml                   # Orquestación Docker
├── .env                           # Variables de entorno
└── BioTech-Backend.sln            # Solución .NET
```

---

## 🧪 Testing

Existen proyectos de prueba para:
- `BioTechBackend.Tests`
- `CommercialService.Tests`
- `HealthService.Tests`
- `HerdService.Tests`
- `InventoryService.Tests`
- `ReproductionService.Tests`
- `SalesService.Tests`

---

## 📡 Formato de Respuesta Estándar

Todos los endpoints utilizan el wrapper `ApiResponse<T>`:

```json
{
    "success": true,
    "message": "Operación exitosa",
    "data": { ... },
    "errors": []
}
```

En caso de error:
```json
{
    "success": false,
    "message": "Descripción del error",
    "data": null,
    "errors": ["Detalle del error 1", "Detalle del error 2"]
}
```
