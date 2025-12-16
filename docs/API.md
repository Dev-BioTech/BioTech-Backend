# BioTech API Documentation

Base URL: `http://localhost:5000` (Local) / `https://biotech-backend-production.up.railway.app` (Railway)

This document outlines the available API endpoints exposed through the API Gateway.

## 🤖 AI Service
**Base Path:** `/api/Chat`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Chat` | Send a message to the AI assistant and get a response. |

## 🔐 Auth Service
**Base Path:** `/api/Auth`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Auth/login` | Authenticate a user and receive a JWT. |
| `POST` | `/api/Auth/register` | Register a new user. |
| `GET` | `/api/Auth/profile` | Get the authenticated user's profile. |
| `PUT` | `/api/Auth/profile` | Update the authenticated user's profile. |

### 🚜 Farms
**Base Path:** `/api/Farm`
*(Note: Ocelot maps `/api/Farm` to AuthService. Ensure backend controller matches or routes are updated.)*

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Farm` | Create a new farm. |
| `GET` | `/api/Farm/{id}` | Get a farm by its ID. |
| `GET` | `/api/Farm/tenant/{userId}` | Get all farms associated with a user (tenant). |

## 🍽️ Feeding Service
**Base Path:** `/api/v1/FeedingEvents`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/v1/FeedingEvents/{id}` | Get feeding event by ID. |
| `GET` | `/api/v1/FeedingEvents/farm/{farmId}` | Get feeding events for a farm. |
| `GET` | `/api/v1/FeedingEvents/batch/{batchId}` | Get feeding events for a batch. |
| `GET` | `/api/v1/FeedingEvents/product/{productId}` | Get feeding events for a product. |
| `GET` | `/api/v1/FeedingEvents/animal/{animalId}` | Get feeding events for a specific animal. |
| `POST` | `/api/v1/FeedingEvents` | Create a new feeding event. |
| `POST` | `/api/v1/FeedingEvents/recalculate-cost` | Recalculate the cost of a feeding event. |
| `PUT` | `/api/v1/FeedingEvents/{id}/cancel` | Cancel (soft delete) a feeding event. |

## 🧬 Reproduction Service
**Base Path:** `/api/v1/Reproduction`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/v1/Reproduction/{id}` | Get reproduction event by ID. |
| `GET` | `/api/v1/Reproduction/animal/{animalId}` | Get reproduction events for an animal. |
| `GET` | `/api/v1/Reproduction/farm/{farmId}` | Get reproduction events for a farm. |
| `GET` | `/api/v1/Reproduction/type/{type}` | Get reproduction events by type (e.g., Insemination). |
| `POST` | `/api/v1/Reproduction` | Create a new reproduction event. |
| `DELETE` | `/api/v1/Reproduction/{id}` | Cancel/Delete a reproduction event. |

## � Herd Service (Animals)
**Base Path:** `/api/v1/animals`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/v1/animals` | Get animals (Query params: `farmId`, `status`, `includeInactive`). |
| `GET` | `/api/v1/animals/{id}` | Get animal by ID. |
| `POST` | `/api/v1/animals` | Register a new animal. |
| `PUT` | `/api/v1/animals/{id}` | Update animal details. |
| `DELETE` | `/api/v1/animals/{id}` | Delete an animal. |
| `POST` | `/api/v1/animals/{id}/movements` | Register a movement (e.g., pasture change). |
| `PUT` | `/api/v1/animals/{id}/weight` | Update animal weight. |
| `PUT` | `/api/v1/animals/{id}/batch` | Move animal to a different batch. |
| `PUT` | `/api/v1/animals/{id}/sell` | Mark animal as sold. |
| `PUT` | `/api/v1/animals/{id}/dead` | Mark animal as dead. |

### Other Herd Resources
The Herd service also exposes:
- `/api/v1/batches`
- `/api/v1/breeds`
- `/api/v1/categories`
- `/api/v1/movement-types`
- `/api/v1/paddocks`
*(Assume standard CRUD pattern for these resources)*

## 🏥 Health Service
**Base Path:** `/api/HealthEvent`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/HealthEvent` | Register a new health event. |
| `GET` | `/api/HealthEvent/farm` | Get events by farm (Query: `fromDate`, `toDate`, `eventType`). |
| `GET` | `/api/HealthEvent/animal/{animalId}` | Get events for an animal. |
| `GET` | `/api/HealthEvent/batch/{batchId}` | Get events for a batch. |
| `GET` | `/api/HealthEvent/type/{type}` | Get events by type. |
| `GET` | `/api/HealthEvent/dashboard-stats` | Get health dashboard statistics. |
| `GET` | `/api/HealthEvent/upcoming` | Get upcoming health events/treatments. |
| `GET` | `/api/HealthEvent/recent-treatments` | Get recent treatments. |

## � Commercial Service
**Base Path:** `/api/transactions`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/transactions` | Create a new transaction. |
| `GET` | `/api/transactions` | Get transactions (Query: `farmId`, `type`, `date`). |
| `GET` | `/api/transactions/{id}` | Get transaction by ID. |
| `GET` | `/api/transactions/{id}/animals` | Get animals involved in a transaction. |
| `GET` | `/api/transactions/{id}/products` | Get products involved in a transaction. |

**Base Path:** `/api/third-parties`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/third-parties` | Create a third party (customer/supplier). |
| `PUT` | `/api/third-parties/{id}` | Update a third party. |
| `GET` | `/api/third-parties` | Get third parties. |
| `GET` | `/api/third-parties/{id}` | Get third party by ID. |

## � Inventory Service
**Base Path:** `/api/Products`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Products` | Create a new product. |
| `GET` | `/api/Products` | Get products (Query: `farmId`). |
| `GET` | `/api/Products/low-stock` | Get low stock products. |

**Base Path:** `/api/Inventory`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/Inventory` | Create inventory item. |
| `GET` | `/api/Inventory/farm/{farmId}` | Get inventory items by farm. |

**Base Path:** `/api/InventoryMovements`

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/InventoryMovements` | Register inventory movement (in/out). |
| `GET` | `/api/InventoryMovements/product/{productId}` | Get movement history (Kardex) for a product. |
