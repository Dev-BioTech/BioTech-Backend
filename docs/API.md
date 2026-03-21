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

### Pregnancies & Births

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `GET` | `/api/v1/Reproduction/pregnancies` | Get pregnancies by farm | `farmId` (required) | ✅ |
| `GET` | `/api/v1/Reproduction/births` | Get births by farm | `farmId` (required) | ✅ |
| `POST` | `/api/v1/Reproduction/register-birth` | Register birth | - | ✅ |

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

### Herd Management

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `GET` | `/api/v1/breeds` | Get all breeds | - | ✅ |
| `POST` | `/api/v1/breeds` | Create breed | - | ✅ |
| `GET` | `/api/v1/categories` | Get all categories | - | ✅ |
| `POST` | `/api/v1/categories` | Create category | - | ✅ |
| `GET` | `/api/v1/paddocks` | Get paddocks by farm | `farmId` (required) | ✅ |
| `POST` | `/api/v1/paddocks` | Create paddock | - | ✅ |
| `GET` | `/api/v1/batches` | Get batches by farm | `farmId` (required) | ✅ |
| `POST` | `/api/v1/batches` | Create batch | - | ✅ |
| `GET` | `/api/v1/movement-types` | Get movement types | - | ✅ |

---

## 🏥 Health Service

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|

| `POST` | `/api/v1/HealthEvent` | Register event | - | ✅ |
| `GET` | `/api/v1/HealthEvent/farm` | Get by farm (context) | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/HealthEvent/animal/{animalId}` | Get by animal | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/HealthEvent/batch/{batchId}` | Get by batch | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/HealthEvent/type/{type}` | Get by type | `page`, `pageSize` | ✅ |
| `GET` | `/api/v1/HealthEvent/dashboard-stats` | Dashboard stats | - | ✅ |
| `GET` | `/api/v1/HealthEvent/upcoming` | Upcoming events | `limit` (default: 10) | ✅ |
| `GET` | `/api/v1/HealthEvent/recent-treatments` | Recent treatments | `limit` (default: 10) | ✅ |
| `PUT` | `/api/v1/HealthEvent/{id}` | Update health event | - | ✅ |


---

## � Inventory Service


### Products

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|
| `POST` | `/api/v1/Products` | Create product | - | ✅ |
| `GET` | `/api/v1/Products` | Get products | `farmId` (required) | ✅ |
| `GET` | `/api/v1/Products/{id}` | Get product by ID | - | ✅ |
| `PUT` | `/api/v1/Products/{id}` | Update product | - | ✅ |
| `DELETE` | `/api/v1/Products/{id}` | Delete product | - | ✅ |
| `GET` | `/api/v1/Products/farms/{farmId}/low-stock` | Get low stock products | - | ✅ |

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

## � Sales Service

| Method | Endpoint | Description | Query Params | Auth |
|--------|----------|-------------|--------------|------|

| `GET` | `/api/v1/sales` | Get sales by user | - | ✅ |
| `GET` | `/api/v1/sales/{id}` | Get sale by ID | - | ✅ |
| `POST` | `/api/v1/sales` | Create sale | - | ✅ |
| `PUT` | `/api/v1/sales/{id}` | Update sale | - | ✅ |
| `DELETE` | `/api/v1/sales/{id}` | Delete sale | - | ✅ |

#### POST /api/v1/sales (Create Sale)
**Request Body:**
```json
{
  "farmId": 1,
  "animalId": 12345,
  "buyerName": "Juan Perez",
  "saleDate": "2026-03-21T00:00:00Z",
  "amount": 1500.50,
  "notes": "Optional notes here"
}
```

#### PUT /api/v1/sales/{id} (Update Sale)
**Request Body:**
```json
{
  "buyerName": "Juan Perez Updated",
  "saleDate": "2026-03-21T00:00:00Z",
  "amount": 1600.00
}
```


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

## 📝 Integration Notes

### Authentication
- Include JWT token: `Authorization: Bearer <token>`
- Get token from `/api/v1/auth/login`

### API Versioning
- All endpoints use versioning: `/api/v1/[controller]`
- Current version: **v1**
- Future versions will be `/api/v2/[controller]`, etc.

### Farm Context
- Most endpoints filter by user's `farmId` from JWT
- Some require explicit `farmId` query parameter
- New endpoints use RESTful resource patterns: `/api/v1/Products/farms/{farmId}/low-stock`

### HTTP Status Codes
- `200 OK` - Successful GET/PUT
- `201 Created` - Successful POST (includes Location header)
- `204 No Content` - Successful DELETE
- `400 Bad Request` - Validation errors
- `401 Unauthorized` - Authentication required
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

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

### Error Response Format
```json
{
  "success": false,
  "message": "Error message",
  "data": null,
  "errors": ["Error details"]
}
```

---

## 🔄 Microservice Communication

The API follows a microservice architecture where each service manages its own domain:

- **HerdService**: Animal management, breeds, categories, paddocks, batches
- **InventoryService**: Products, inventory, stock management
- **HealthService**: Health events, treatments, vaccinations
- **ReproductionService**: Pregnancies, births, reproduction events
- **SalesService**: Sales transactions and customer management
- **AuthService**: Authentication, user management, farms
- **FeedingService**: Feeding events and nutrition tracking
- **CommercialService**: General transactions and third parties

**Important**: Cross-service communication is handled via HTTP requests through the API Gateway, not direct database access.

---

**Last Updated:** 2025-03-02  
**Version:** 1.0  
**API Version:** v1

