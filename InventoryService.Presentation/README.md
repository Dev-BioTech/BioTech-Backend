# Inventory Service

## Dependencies
This service communicates with the following microservices:

| Service | Env Variable | Default Local Port | Description |
|---------|--------------|-------------------|-------------|
| **HerdService** | `HERD_SERVICE_URL` | `5048` | Used to validate animal existence when managing inventory items. |

## Configuration
Ensure this variable is set in your `.env` file or environment:
```env
HERD_SERVICE_URL=http://localhost:5048
```
