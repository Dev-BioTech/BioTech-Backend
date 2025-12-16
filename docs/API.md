# 🔌 Available Endpoints (Gateway)

Base URL: `http://localhost:5000`

This document lists all available API endpoints exposed through the API Gateway, categorized by service.

## 🔐 Authentication Service
> **Base Path**: `/api/Auth` & `/api/v1/Farms`

| Method | Endpoint | Description | Requires Auth |
|--------|----------|-------------|---------------|
| `POST` | `/api/Auth/login` | Login and obtain JWT Token | No |
| `POST` | `/api/Auth/register` | Register a new user | No |
| `GET` | `/api/v1/Farms/{id}` | Get farm details by ID | Yes |
| `GET` | `/api/v1/Farms/tenant/{userId}` | List farms for a specific user | Yes |
| `POST` | `/api/v1/Farms` | Create a new farm | Yes |

## 🤖 AI Service
> **Base Path**: `/api/Chat`

| Method | Endpoint | Description | Requires Auth |
|--------|----------|-------------|---------------|
| `POST` | `/api/Chat/send` | Send a message to the AI chat | Yes |

## 🐄 Feeding Service
> **Base Path**: `/api/v1/FeedingEvents`

| Method | Endpoint | Description | Requires Auth |
|--------|----------|-------------|---------------|
| `GET` | `/api/v1/FeedingEvents/{id}` | Get feeding event details | Yes |
| `POST` | `/api/v1/FeedingEvents` | Record a new feeding event | Yes |
| `PUT` | `/api/v1/FeedingEvents/{id}` | Update a feeding event | Yes |
| `DELETE` | `/api/v1/FeedingEvents/{id}` | Delete a feeding event | Yes |
| `GET` | `/api/v1/FeedingEvents/farm/{farmId}` | List events for a farm | Yes |

## 🧬 Reproduction Service
> **Base Path**: `/api/v1/Reproduction`

| Method | Endpoint | Description | Requires Auth |
|--------|----------|-------------|---------------|
| `GET` | `/api/v1/Reproduction/{id}` | Get reproduction event details | Yes |
| `POST` | `/api/v1/Reproduction` | Record a new reproduction event | Yes |
| `DELETE` | `/api/v1/Reproduction/{id}` | Delete a reproduction event | Yes |

## 🐂 Herd Service (Animals)
> **Base Path**: `/api/v1` (Various resources)

| Method | Endpoint | Description | Requires Auth |
|--------|----------|-------------|---------------|
| **Animals** | `/api/v1/animals` | Manage animals (GET, POST, PUT, DELETE) | Yes |
| **Batches** | `/api/v1/batches` | Manage batches (GET, POST, PUT, DELETE) | Yes |
| **Breeds** | `/api/v1/breeds` | Manage breeds (GET, POST, PUT, DELETE) | Yes |
| **Categories** | `/api/v1/categories` | Manage animal categories (GET, POST, PUT, DELETE) | Yes |
| **Movements** | `/api/v1/movement-types` | Manage movement types (GET, POST, PUT, DELETE) | Yes |
| **Paddocks** | `/api/v1/paddocks` | Manage paddocks (GET, POST, PUT, DELETE) | Yes |

## 🏥 Health Service
> **Base Path**: `/api/HealthEvent`

| Method | Endpoint | Description | Requires Auth |
|--------|----------|-------------|---------------|
| `GET` | `/api/HealthEvent/{id}` | Get health event details | Yes |
| `POST` | `/api/HealthEvent` | Record a new health event | Yes |
| `PUT` | `/api/HealthEvent/{id}` | Update a health event | Yes |
| `DELETE` | `/api/HealthEvent/{id}` | Delete a health event | Yes |

## 📦 Inventory Service
> **Base Path**: `/api` (Various resources)

| Method | Endpoint | Description | Requires Auth |
|--------|----------|-------------|---------------|
| **Products** | `/api/Products` | Manage products (GET, POST, PUT, DELETE) | Yes |
| **Inventory** | `/api/Inventory` | Manage inventory stock (GET, POST, PUT, DELETE) | Yes |
| **Movements** | `/api/InventoryMovements` | Track inventory movements (GET, POST, PUT, DELETE) | Yes |

## 💰 Commercial Service
> **Base Path**: `/api` (Various resources)

| Method | Endpoint | Description | Requires Auth |
|--------|----------|-------------|---------------|
| **Transactions** | `/api/transactions` | Manage commercial transactions (GET, POST, PUT, DELETE) | Yes |
| **Third Parties** | `/api/third-parties` | Manage vendors/customers (GET, POST, PUT, DELETE) | Yes |
