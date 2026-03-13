FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SalesService.Presentation/SalesService.Presentation.csproj", "SalesService.Presentation/"]
COPY ["SalesService.Application/SalesService.Application.csproj", "SalesService.Application/"]
COPY ["SalesService.Infrastructure/SalesService.Infrastructure.csproj", "SalesService.Infrastructure/"]
COPY ["SalesService.Domain/SalesService.Domain.csproj", "SalesService.Domain/"]
COPY ["Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared.Infrastructure/"]
RUN dotnet restore "SalesService.Presentation/SalesService.Presentation.csproj"
COPY . .
WORKDIR "/src/SalesService.Presentation"
RUN dotnet build "SalesService.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesService.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesService.Presentation.dll"]
