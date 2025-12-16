FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ReproductionService.Presentation/ReproductionService.Presentation.csproj", "ReproductionService.Presentation/"]
COPY ["ReproductionService.Application/ReproductionService.Application.csproj", "ReproductionService.Application/"]
COPY ["ReproductionService.Domain/ReproductionService.Domain.csproj", "ReproductionService.Domain/"]
COPY ["ReproductionService.Infrastructure/ReproductionService.Infrastructure.csproj", "ReproductionService.Infrastructure/"]
RUN dotnet restore "ReproductionService.Presentation/ReproductionService.Presentation.csproj"
COPY . .
WORKDIR "/src/ReproductionService.Presentation"
RUN dotnet build "ReproductionService.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReproductionService.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReproductionService.Presentation.dll"]
