FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine3.20 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine3.20 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Source/Presentation/BaCS.Presentation.API/BaCS.Presentation.API.csproj", "Source/Presentation/BaCS.Presentation.API/"]
COPY ["Source/Infrastructure/BaCS.Infrastructure.Observability/BaCS.Infrastructure.Observability.csproj", "Source/Infrastructure/BaCS.Infrastructure.Observability/"]
COPY ["Source/Persistence/BaCS.Persistence.PostgreSQL/BaCS.Persistence.PostgreSQL.csproj", "Source/Persistence/BaCS.Persistence.PostgreSQL/"]
COPY ["Source/Persistence/BaCS.Persistence.Minio/BaCS.Persistence.Minio.csproj", "Source/Persistence/BaCS.Persistence.Minio/"]
COPY ["Source/Application/BaCS.Application.Abstractions/BaCS.Application.Abstractions.csproj", "Source/Application/BaCS.Application.Abstractions/"]
COPY ["Source/Application/BaCS.Application.Contracts/BaCS.Application.Contracts.csproj", "Source/Application/BaCS.Application.Contracts/"]
COPY ["Source/Application/BaCS.Application.Handlers/BaCS.Application.Handlers.csproj", "Source/Application/BaCS.Application.Handlers/"]
COPY ["Source/Application/BaCS.Application.Mapping/BaCS.Application.Mapping.csproj", "Source/Application/BaCS.Application.Mapping/"]
COPY ["Source/Application/BaCS.Application.Services/BaCS.Application.Services.csproj", "Source/Application/BaCS.Application.Services/"]
COPY ["Source/Domain/BaCS.Domain.Core/BaCS.Domain.Core.csproj", "Source/Domain/BaCS.Domain.Core/"]
COPY . .
RUN dotnet restore "Source/Presentation/BaCS.Presentation.API/BaCS.Presentation.API.csproj"
RUN dotnet build --tl --no-restore "Source/Presentation/BaCS.Presentation.API/BaCS.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Source/Presentation/BaCS.Presentation.API/BaCS.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaCS.Presentation.API.dll"]
