FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AIService.Presentation/AIService.Presentation.csproj", "AIService.Presentation/"]
COPY ["AIService.Application/AIService.Application.csproj", "AIService.Application/"]
COPY ["AIService.Infrastructure/AIService.Infrastructure.csproj", "AIService.Infrastructure/"]
COPY ["AIService.Domain/AIService.Domain.csproj", "AIService.Domain/"]
COPY ["Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared.Infrastructure/"]
RUN dotnet restore "AIService.Presentation/AIService.Presentation.csproj"
COPY . .
WORKDIR "/src/AIService.Presentation"
RUN dotnet build "AIService.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AIService.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AIService.Presentation.dll"]
