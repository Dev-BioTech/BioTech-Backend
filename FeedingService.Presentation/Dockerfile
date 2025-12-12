FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FeedingService.Presentation/FeedingService.Presentation.csproj", "FeedingService.Presentation/"]
COPY ["FeedingService.Application/FeedingService.Application.csproj", "FeedingService.Application/"]
COPY ["FeedingService.Domain/FeedingService.Domain.csproj", "FeedingService.Domain/"]
COPY ["FeedingService.Infrastructure/FeedingService.Infrastructure.csproj", "FeedingService.Infrastructure/"]
RUN dotnet restore "FeedingService.Presentation/FeedingService.Presentation.csproj"
COPY . .
WORKDIR "/src/FeedingService.Presentation"
RUN dotnet build "FeedingService.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeedingService.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeedingService.Presentation.dll"]
