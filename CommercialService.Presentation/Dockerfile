# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CommercialService.Presentation/CommercialService.Presentation.csproj", "CommercialService.Presentation/"]
COPY ["CommercialService.Application/CommercialService.Application.csproj", "CommercialService.Application/"]
COPY ["CommercialService.Domain/CommercialService.Domain.csproj", "CommercialService.Domain/"]
COPY ["CommercialService.Infrastructure/CommercialService.Infrastructure.csproj", "CommercialService.Infrastructure/"]
RUN dotnet restore "./CommercialService.Presentation/CommercialService.Presentation.csproj"
COPY . .
WORKDIR "/src/CommercialService.Presentation"
RUN dotnet build "./CommercialService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CommercialService.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommercialService.Presentation.dll"]
