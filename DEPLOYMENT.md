# 🚀 Deployment Checklist

This guide details the environment variables and configurations required to deploy the BioTech Backend services to a cloud provider like **Railway** or **Clever Cloud**.

## 1. Global Configuration (All Services)

Every microservice (Auth, Health, Herd, etc.) and the API Gateway needs these variables to be set in your cloud provider's dashboard.

| Variable Name | Description | Example Value |
| :--- | :--- | :--- |
| `POSTGRESQL_ADDON_URI` | **(Critical)** The full connection string to your production database. Railway/Clever Cloud often injects this automatically. | `postgresql://user:pass@host:port/dbname` |
| `JWT_SECRET` | **(Critical)** A long, random string used to sign tokens. **Must be the same across ALL services.** | `your-256-bit-secret-key...` |
| `JWT_ISSUER` | Identify who issued the token. | `BioTech` |
| `JWT_AUDIENCE` | Identify who the token is for. | `BioTech` |
| `GATEWAY_SECRET` | **(Critical)** A shared secret key that protects internal service routes. **Must be the same across ALL services.** | `internal-secret-key-123` |
| `ASPNETCORE_ENVIRONMENT` | Set the environment to Production. | `Production` |

> **Note:** You do **not** need to set `PORT`. The application is configured to automatically listen on the port provided by the cloud platform.

## 2. API Gateway Configuration

The **ApiGateway** requires additional variables to know where your other services are running. In a localized environment (like Docker Compose), these are internal names. In a PaaS (like Railway), these are the public or private URLs of your deployed services.

| Variable Name | Description | Example (Railway/Public) |
| :--- | :--- | :--- |
| `AUTH_SERVICE_URL` | URL of the Auth Service | `https://biotech-auth.up.railway.app` |
| `HEALTH_SERVICE_URL` | URL of the Health Service | `https://biotech-health.up.railway.app` |
| `FEEDING_SERVICE_URL` | URL of the Feeding Service | `https://biotech-feeding.up.railway.app` |
| `HERD_SERVICE_URL` | URL of the Herd Service | `https://biotech-herd.up.railway.app` |
| `INVENTORY_SERVICE_URL` | URL of the Inventory Service | `https://biotech-inventory.up.railway.app` |
| `REPRODUCTION_SERVICE_URL` | URL of the Reproduction Service | `https://biotech-reproduction.up.railway.app` |
| `COMMERCIAL_SERVICE_URL` | URL of the Commercial Service | `https://biotech-commercial.up.railway.app` |
| `AI_SERVICE_URL` | URL of the AI Service | `https://biotech-ai.up.railway.app` |

## 3. CORS Configuration (Frontend Access)

To allow your frontend (e.g., Vercel) to talk to your Gateway and Services, you need to configure `AllowedOrigins`.

In .NET, lists are configured using double underscores for index:

*   `AllowedOrigins__0` = `https://your-frontend.vercel.app`
*   `AllowedOrigins__1` = `http://localhost:3000` (Optional, for local testing against prod)

## 4. Verification Steps

1.  **Push your code** to GitHub.
2.  **Create/Connect projects** in Railway/Clever Cloud.
3.  **Add the Variables** listed above to each service.
4.  **Deploy**.
5.  **Test** by hitting the Gateway's Health/Auth endpoint.

## Local Development (Restored)

I have restored your local configuration so you can continue working without issues:
*   Local ports (5237, etc.) are respected.
*   `ApiGateWay/.env` maps these ports for you automatically.
*   Conflicting `PORT=5001` entries in your local `.env` files have been disabled.
