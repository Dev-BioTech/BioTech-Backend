# BioTech API Documentation

**Base URL:** `http://localhost:5000` (Local) | `https://biotech-backend-production.up.railway.app` (Railway)

All endpoints are accessed through the API Gateway. Authentication required unless specified.

---

## 🤖 AI Service

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/v1/chat` | Send message to AI assistant | ✅ |
| `POST` | `/api/v1/diagnostic/analyze-error` | Analyze error | ✅ |

---

## 🔐 Auth Service

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/v1/auth/login` | User login | ❌ |
| `POST` | `/api/v1/auth/register` | Register new user | ❌ |
| `GET` | `/api/v1/auth/profile` | Get user profile | ✅ |
| `PUT` | `/api/v1/auth/profile` | Update user profile | ✅ |

### Farms

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/farms` | Create farm | - | ✅ |
| `GET` | `/api/v1/farms/{id}` | Get farm by ID | - | ✅ |
| `GET` | `/api/v1/farms/tenant/{userId}` | Get farms by tenant | `includeInactive` | ✅ |

---

## 🍽️ Feeding Service

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/feeding-events` | Create feeding event | - | ✅ |
| `GET` | `/api/v1/feeding-events/{id}` | Get by ID | - | ✅ |
| `GET` | `/api/v1/feeding-events/farm/{farmId}` | Get by farm | `fromDate`, `toDate`, `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/feeding-events/batch/{batchId}` | Get by batch | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/feeding-events/product/{productId}` | Get by product | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/feeding-events/animal/{animalId}` | Get by animal | `page`, `pageSize` | ✅ |
| `POST` | `/api/v1/feeding-events/recalculate-cost` | Recalculate cost | - | ✅ |
| `PUT` | `/api/v1/feeding-events/{id}/cancel` | Cancel event | - | ✅ |

---

## 🧬 Reproduction Service

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/reproduction` | Create event | - | ✅ |
| `GET` | `/api/v1/reproduction/{id}` | Get by ID | - | ✅ |
| `GET` | `/api/v1/reproduction/animal/{animalId}` | Get by animal | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/reproduction/farm` | Get by farm (context) | `fromDate`, `toDate`, `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/reproduction/farm/{farmId}` | Get by farm ID | `fromDate`, `toDate`, `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/reproduction/type/{type}` | Get by type | `page`, `pageSize` | ✅ |
| `PUT` | `/api/v1/reproduction/{id}/cancel` | Cancel event | - | ✅ |

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
| `POST` | `/api/v1/health-event` | Register event | - | ✅ |
| `GET` | `/api/v1/health-event/farm` | Get by farm (context) | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/health-event/animal/{animalId}` | Get by animal | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/health-event/batch/{batchId}` | Get by batch | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/health-event/type/{type}` | Get by type | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/health-event/dashboard-stats` | Dashboard stats | - | ✅ |
| `GET` | `/api/v1/health-event/upcoming` | Upcoming events | `limit` | ✅ |
| `GET` | `/api/v1/health-event/recent-treatments` | Recent treatments | `limit` | ✅ |

---

## 💼 Commercial Service

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/transactions` | Create transaction | - | ✅ |
| `GET` | `/api/v1/transactions` | Get transactions | `fromDate`, `toDate`, `type`, `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/transactions/{id}` | Get by ID | - | ✅ |
| `GET` | `/api/v1/transactions/{id}/animals` | Get transaction animals | - | ✅ |
| `GET` | `/api/v1/transactions/{id}/products` | Get transaction products | - | ✅ |
| `POST` | `/api/v1/third-parties` | Create third party | - | ✅ |
| `PUT` | `/api/v1/third-parties/{id}` | Update third party | - | ✅ |
| `GET` | `/api/v1/third-parties` | Get third parties | `isSupplier`, `isCustomer`, `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/third-parties/{id}` | Get by ID | - | ✅ |

---

## 📦 Inventory Service

### Products

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/products` | Create product | - | ✅ |
| `GET` | `/api/v1/products` | Get products | `farmId` (required) | ✅ |
| `GET` | `/api/v1/products/low-stock` | Get low stock | `farmId` (required) | ✅ |

### Inventory

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/inventory` | Create inventory item | - | ✅ |
| `GET` | `/api/v1/inventory/farm/{farmId}` | Get by farm | `page`, `pageSize` | ✅ |

### Inventory Movements

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/inventory-movements` | Register movement | - | ✅ |
| `GET` | `/api/v1/inventory-movements/product/{productId}` | Get Kardex | - | ✅ |

---

## 📝 Integration Notes

### Authentication
- Include JWT token: `Authorization: Bearer <token>`
- Get token from `/api/v1/auth/login`

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

**Last Updated:** 2026-02-23  
**Version:** 1.1 (Standardized v1 Routes)
