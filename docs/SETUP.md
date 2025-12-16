# 🚀 Getting Started

### Prerequisites

- ✅ [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ [PostgreSQL 15+](https://www.postgresql.org/download/)
- ✅ [Docker](https://www.docker.com/get-started) (optional)
- ✅ [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Rider](https://www.jetbrains.com/rider/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-org/BioTech-Backend.git
   cd BioTech-Backend
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection string**
   
   Edit `API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=biotech;Username=postgres;Password=yourpassword"
     }
   }
   ```

4. **Run database migrations**
   ```bash
   dotnet ef database update --project Infrastructure --startup-project API
   ```

5. **Run the application**
   ```bash
   dotnet run --project API
   ```

6. **Access the API**
   - Swagger UI: `https://localhost:5001/swagger`
   - API: `https://localhost:5001/api`

### Using Docker

```bash
# Build and run with Docker Compose
docker-compose up --build

# Stop containers
docker-compose down
```
