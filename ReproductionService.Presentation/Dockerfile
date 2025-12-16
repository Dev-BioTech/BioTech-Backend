# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY BioTech-Backend.sln .

# Copy all project files
COPY ReproductionService.Application/*.csproj ./ReproductionService.Application/
COPY ReproductionService.Domain/*.csproj ./ReproductionService.Domain/
COPY ReproductionService.Infrastructure/*.csproj ./ReproductionService.Infrastructure/
COPY ReproductionService.Presentation/*.csproj ./ReproductionService.Presentation/

# Restore dependencies
RUN dotnet restore ReproductionService.Presentation/ReproductionService.Presentation.csproj

# Copy all source code
COPY . .

# Build and publish
WORKDIR /src/ReproductionService.Presentation
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ReproductionService.Presentation.dll"]
