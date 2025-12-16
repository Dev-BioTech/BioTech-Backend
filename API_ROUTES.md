# API Routes Documentation

This document lists all available API endpoints for the BioTech Backend microservices exposed via the API Gateway.

## Global Parameters
For list-returning endpoints, the following query parameters are supported for pagination:
- `page` (int, default: 1): The page number to retrieve.
- `pageSize` (int, default: 10): The number of items per page (max: 10).

---

## 1. FeedingService
**Base URL**: `/api/v1/FeedingEvents`

### Endpoints
- `POST /` - Create a new feeding event.
- `GET /farm/{farmId}` - Get feeding events by Farm ID.
- `GET /batch/{batchId}` - Get feeding events by Batch ID.
- `GET /product/{productId}` - Get feeding events by Product ID.
- `GET /animal/{animalId}` - Get feeding events by Animal ID.
- `POST /recalculate-cost` - Recalculate total costs.
- `PUT /{id}/cancel` - Cancel (soft delete) a feeding event.

---

## 2. ReproductionService
**Base URL**: `/api/v1/Reproduction`

### Endpoints
- `POST /` - Create a new reproduction event.
- `GET /animal/{animalId}` - Get reproduction events by Animal ID.
- `GET /farm/{farmId}` - Get reproduction events by Farm ID.
- `GET /type/{type}` - Get reproduction events by Event Type.
- `GET /{id}` - Get a specific reproduction event by ID.
- `PUT /{id}/cancel` - Cancel (soft delete) a reproduction event.

---

## 3. CommercialService
**Base URL**: `/api`

### Transactions
**Base URL**: `/api/transactions`
- `POST /` - Create a new transaction.
- `GET /` - Get transactions (Filters: `farmId`, `fromDate`, `toDate`, `type`).
- `GET /{id}` - Get transaction details by ID.
- `GET /{id}/animals` - Get animals associated with a transaction.
- `GET /{id}/products` - Get products associated with a transaction.

### Third Parties
**Base URL**: `/api/third-parties`
- `POST /` - Create a new third party.
- `PUT /{id}` - Update a third party.
- `GET /` - Get third parties (Filters: `farmId`, `isSupplier`, `isCustomer`).
- `GET /{id}` - Get third party details by ID.

---

## 4. HealthService
**Base URL**: `/api/HealthEvent`

### Endpoints
- `POST /` - Create a new health event.
- `GET /farm/{farmId}` - Get health events by Farm ID.
- `GET /animal/{animalId}` - Get health events by Animal ID.
- `GET /batch/{batchId}` - Get health events by Batch ID.
- `GET /type/{type}` - Get health events by Event Type.

---

## 5. HerdService
**Base URL**: `/api/v1`

### Animals
**Base URL**: `/api/v1/animals`
- `POST /` - Create a new animal.
- `POST /{id}/movements` - Register an animal movement.

### Batches
**Base URL**: `/api/v1/batches`
- `POST /` - Create a new batch.

### Breeds
**Base URL**: `/api/v1/breeds`
- `POST /` - Create a new breed.

### Categories
**Base URL**: `/api/v1/categories`
- `POST /` - Create a new category.

### Paddocks
**Base URL**: `/api/v1/paddocks`
- `POST /` - Create a new paddock.

### Movement Types
**Base URL**: `/api/v1/movement-types`
- `POST /` - Create a new movement type.

---

## 6. AuthService
**Base URL**: `/api/Auth`

### Endpoints
- `POST /login` - Authenticate a user and retrieve a JWT token.
- `POST /register` - Register a new user.
