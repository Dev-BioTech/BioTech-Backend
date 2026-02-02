# API Gateway

## Dependencies
This is the entry point for the system and routes requests to all microservices.

| Service | Env Variable | Default Local Port | Description |
|---------|--------------|-------------------|-------------|
| **AuthService** | `AUTH_SERVICE_URL` | `5237` | Authentication and Token Issuance. |
| **CommercialService** | `COMMERCIAL_SERVICE_URL` | `5238` | Transaction management. |
| **FeedingService** | `FEEDING_SERVICE_URL` | `5102` | Feeding event management. |
| **HealthService** | `HEALTH_SERVICE_URL` | `5158` | Health record management. |
| **HerdService** | `HERD_SERVICE_URL` | `5048` | Animal and Herd management. |
| **InventoryService** | `INVENTORY_SERVICE_URL` | `5261` | Inventory management. |
| **ReproductionService** | `REPRODUCTION_SERVICE_URL` | `5047` | Reproduction event management. |

## Configuration
Ensure these variables are set in your `.env` file or environment:
```env
AUTH_SERVICE_URL=http://localhost:5237
COMMERCIAL_SERVICE_URL=http://localhost:5238
FEEDING_SERVICE_URL=http://localhost:5102
HEALTH_SERVICE_URL=http://localhost:5158
HERD_SERVICE_URL=http://localhost:5048
INVENTORY_SERVICE_URL=http://localhost:5261
REPRODUCTION_SERVICE_URL=http://localhost:5047
```
