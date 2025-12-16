FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HerdService.Presentation/HerdService.Presentation.csproj", "HerdService.Presentation/"]
COPY ["HerdService.Application/HerdService.Application.csproj", "HerdService.Application/"]
COPY ["HerdService.Domain/HerdService.Domain.csproj", "HerdService.Domain/"]
COPY ["HerdService.Infrastructure/HerdService.Infrastructure.csproj", "HerdService.Infrastructure/"]
RUN dotnet restore "HerdService.Presentation/HerdService.Presentation.csproj"
COPY . .
WORKDIR "/src/HerdService.Presentation"
RUN dotnet build "HerdService.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HerdService.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HerdService.Presentation.dll"]
