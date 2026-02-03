# AI Service

## Dependencies
This service communicates with the following microservices:

| Service | Env Variable | Default Local Port | Description |
|---------|--------------|-------------------|-------------|
| **HerdService** | `HERD_SERVICE_URL` | `5048` | Used to retrieve animal information. |
| **HealthService** | `HEALTH_SERVICE_URL` | `5158` | Used to analyze health records. |
| **FeedingService** | `FEEDING_SERVICE_URL` | `5102` | Used to analyze nutrition data. |

## Configuration
Ensure these variables are set in your `.env` file or environment:
```env
HERD_SERVICE_URL=http://localhost:5048
HEALTH_SERVICE_URL=http://localhost:5158
FEEDING_SERVICE_URL=http://localhost:5102
```
