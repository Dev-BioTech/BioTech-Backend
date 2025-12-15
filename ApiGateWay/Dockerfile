# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY BioTech-Backend.sln .

# Copy all project files
COPY ApiGateWay/*.csproj ./ApiGateWay/
COPY AuthService.Application/*.csproj ./AuthService.Application/
COPY AuthService.Domain/*.csproj ./AuthService.Domain/
COPY AuthService.Presentation/*.csproj ./AuthService.Presentation/
COPY Authservice.Infrastructure/*.csproj ./Authservice.Infrastructure/
COPY FeedingService.Application/*.csproj ./FeedingService.Application/
COPY FeedingService.Domain/*.csproj ./FeedingService.Domain/
COPY FeedingService.Infrastructure/*.csproj ./FeedingService.Infrastructure/
COPY FeedingService.Presentation/*.csproj ./FeedingService.Presentation/

# Restore dependencies
RUN dotnet restore ApiGateWay/ApiGateWay.csproj

# Copy all source code
COPY . .

# Build and publish
WORKDIR /src/ApiGateWay
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ApiGateWay.dll"]
