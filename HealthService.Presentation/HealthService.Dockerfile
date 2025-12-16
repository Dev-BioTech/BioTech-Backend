# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY BioTech-Backend.sln .

# Copy all project files
# Copy all project files
COPY HealthService.Application/*.csproj ./HealthService.Application/
COPY HealthService.Domain/*.csproj ./HealthService.Domain/
COPY HealthService.Infrastructure/*.csproj ./HealthService.Infrastructure/
COPY HealthService.Presentation/*.csproj ./HealthService.Presentation/

# We also need to copy other project files if they are referenced or if sln restore fails without them, 
# but HealthService seems standalone in dependencies. However, dotnet restore on SLN might fail if other projects missing.
# Best practice is to copy all logic. But for this microservice build, we can try to be specific or copy all.
# Let's copy all csproj in repo to be safe like FeedingService did? 
# FeedingService only copied ITSELF (FeedingService.*). The SLN file usually lists all, so 'dotnet restore .sln' might warn or fail.
# FeedingService Dockerfile restores SPECIFIC PROJECT: RUN dotnet restore FeedingService.Presentation/FeedingService.Presentation.csproj
# So we do the same.

# Restore dependencies
RUN dotnet restore HealthService.Presentation/HealthService.Presentation.csproj

# Copy all source code
COPY . .

# Build and publish
WORKDIR /src/HealthService.Presentation
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "HealthService.Presentation.dll"]
