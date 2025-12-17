# BioTech API Documentation

**Base URL:** `http://localhost:5000` (Local) | `https://biotech-backend-production.up.railway.app` (Railway)

All endpoints are accessed through the API Gateway. Authentication required unless specified.

---

## 🤖 AI Service

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/Chat` | Send message to AI assistant | ✅ |

---

## 🔐 Auth Service

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/Auth/login` | User login | ❌ |
| `POST` | `/api/Auth/register` | Register new user | ❌ |
| `GET` | `/api/auth/profile` | Get user profile | ✅ |
| `PUT` | `/api/auth/profile` | Update user profile | ✅ |

### Farms

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/Farms` | Create farm | - | ✅ |
| `GET` | `/api/v1/Farms/{id}` | Get farm by ID | - | ✅ |
| `GET` | `/api/v1/Farms/tenant/{userId}` | Get farms by tenant | `includeInactive` | ✅ |

---

## 🍽️ Feeding Service

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/FeedingEvents` | Create feeding event | - | ✅ |
| `GET` | `/api/v1/FeedingEvents/{id}` | Get by ID | - | ✅ |
| `GET` | `/api/v1/FeedingEvents/farm/{farmId}` | Get by farm | `fromDate`, `toDate`, `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/FeedingEvents/batch/{batchId}` | Get by batch | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/FeedingEvents/product/{productId}` | Get by product | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/FeedingEvents/animal/{animalId}` | Get by animal | `page`, `pageSize` | ✅ |
| `POST` | `/api/v1/FeedingEvents/recalculate-cost` | Recalculate cost | - | ✅ |
| `PUT` | `/api/v1/FeedingEvents/{id}/cancel` | Cancel event | - | ✅ |

---

## 🧬 Reproduction Service

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/Reproduction` | Create event | - | ✅ |
| `GET` | `/api/v1/Reproduction/{id}` | Get by ID | - | ✅ |
| `GET` | `/api/v1/Reproduction/animal/{animalId}` | Get by animal | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/Reproduction/farm` | Get by farm (context) | `fromDate`, `toDate`, `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/Reproduction/farm/{farmId}` | Get by farm ID | `fromDate`, `toDate`, `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/Reproduction/type/{type}` | Get by type | `page`, `pageSize` | ✅ |
| `PUT` | `/api/v1/Reproduction/{id}/cancel` | Cancel event | - | ✅ |

---

## 🐄 Herd Service - Animals

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/animals` | Register animal | - | ✅ |
| `GET` | `/api/v1/animals` | Get animals | `farmId`, `status`, `includeInactive` | ✅ |
| `GET` | `/api/v1/animals/{id}` | Get by ID | - | ✅ |
| `PUT` | `/api/v1/animals/{id}` | Update animal | - | ✅ |
| `DELETE` | `/api/v1/animals/{id}` | Delete animal | - | ✅ |
| `POST` | `/api/v1/animals/{id}/movements` | Register movement | - | ✅ |
| `PUT` | `/api/v1/animals/{id}/weight` | Update weight | - | ✅ |
| `PUT` | `/api/v1/animals/{id}/batch` | Move to batch | - | ✅ |
| `PUT` | `/api/v1/animals/{id}/sell` | Mark as sold | - | ✅ |
| `PUT` | `/api/v1/animals/{id}/dead` | Mark as dead | - | ✅ |

**Note:** Herd Service also exposes `/api/v1/batches`, `/api/v1/breeds`, `/api/v1/categories`, `/api/v1/movement-types`, `/api/v1/paddocks` (standard CRUD).

---

## 🏥 Health Service

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/HealthEvent` | Register event | - | ✅ |
| `GET` | `/api/HealthEvent/farm` | Get by farm (context) | `page`, `pageSize` | ✅ |
| `GET` | `/api/HealthEvent/animal/{animalId}` | Get by animal | `page`, `pageSize` | ✅ |
| `GET` | `/api/HealthEvent/batch/{batchId}` | Get by batch | `page`, `pageSize` | ✅ |
| `GET` | `/api/HealthEvent/type/{type}` | Get by type | `page`, `pageSize` | ✅ |
| `GET` | `/api/HealthEvent/dashboard-stats` | Dashboard stats | - | ✅ |
| `GET` | `/api/HealthEvent/upcoming` | Upcoming events | `limit` | ✅ |
| `GET` | `/api/HealthEvent/recent-treatments` | Recent treatments | `limit` | ✅ |

---

## 💼 Commercial Service

### Transactions

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/transactions` | Create transaction | - | ✅ |
| `GET` | `/api/transactions` | Get transactions | `fromDate`, `toDate`, `type`, `page`, `pageSize` | ✅ |
| `GET` | `/api/transactions/{id}` | Get by ID | - | ✅ |
| `GET` | `/api/transactions/{id}/animals` | Get transaction animals | - | ✅ |
| `GET` | `/api/transactions/{id}/products` | Get transaction products | - | ✅ |

### Third Parties

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/third-parties` | Create third party | - | ✅ |
| `PUT` | `/api/third-parties/{id}` | Update third party | - | ✅ |
| `GET` | `/api/third-parties` | Get third parties | `isSupplier`, `isCustomer`, `page`, `pageSize` | ✅ |
| `GET` | `/api/third-parties/{id}` | Get by ID | - | ✅ |

---

## 📦 Inventory Service

### Products

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/Products` | Create product | - | ✅ |
| `GET` | `/api/Products` | Get products | `farmId` (required) | ✅ |
| `GET` | `/api/Products/low-stock` | Get low stock | `farmId` (required) | ✅ |

### Inventory

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/Inventory` | Create inventory item | - | ✅ |
| `GET` | `/api/Inventory/farm/{farmId}` | Get by farm | `page`, `pageSize` | ✅ |

### Inventory Movements

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/InventoryMovements` | Register movement | - | ✅ |
| `GET` | `/api/InventoryMovements/product/{productId}` | Get Kardex | - | ✅ |

---

## 📝 Integration Notes

### Authentication
- Include JWT token: `Authorization: Bearer <token>`
- Get token from `/api/Auth/login`

### Farm Context
- Most endpoints filter by user's `farmId` from JWT
- Some require explicit `farmId` query parameter

### Pagination
- Default: `page=1`, `pageSize=10`
- Example: `?page=2&pageSize=20`

### Date Format
- ISO 8601: `YYYY-MM-DD`
- Example: `fromDate=2024-01-01`

### Response Format
```json
{
  "success": true,
  "message": "Success message",
  "data": {},
  "errors": []
}
```

---

**Last Updated:** 2024-12-16  
**Version:** 1.0
