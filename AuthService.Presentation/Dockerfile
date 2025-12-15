# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY BioTech-Backend.sln .

# Copy all project files
COPY AuthService.Application/*.csproj ./AuthService.Application/
COPY AuthService.Domain/*.csproj ./AuthService.Domain/
COPY AuthService.Presentation/*.csproj ./AuthService.Presentation/
COPY Authservice.Infrastructure/*.csproj ./Authservice.Infrastructure/

# Restore dependencies
RUN dotnet restore AuthService.Presentation/AuthService.Presentation.csproj

# Copy all source code
COPY . .

# Build and publish
WORKDIR /src/AuthService.Presentation
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthService.Presentation.dll"]
