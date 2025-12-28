FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine3.20 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine3.20 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Source/Presentation/BaCS.Presentation.BusinessProcess/BaCS.Presentation.BusinessProcess.csproj", "Source/Presentation/BaCS.Presentation.BusinessProcess/"]
COPY ["Source/Application/BaCS.Application.Integrations/BaCS.Application.Integrations.csproj", "Source/Application/BaCS.Application.Integrations/"]
COPY ["Source/Application/BaCS.Application.Abstractions/BaCS.Application.Abstractions.csproj", "Source/Application/BaCS.Application.Abstractions/"]
COPY ["Source/Domain/BaCS.Domain.Core/BaCS.Domain.Core.csproj", "Source/Domain/BaCS.Domain.Core/"]
COPY ["Source/Application/BaCS.Application.Contracts/BaCS.Application.Contracts.csproj", "Source/Application/BaCS.Application.Contracts/"]
COPY ["Source/Infrastructure/BaCS.Infrastructure.Workflows/BaCS.Infrastructure.Workflows.csproj", "Source/Infrastructure/BaCS.Infrastructure.Workflows/"]
COPY ["Source/Persistence/BaCS.Persistence.PostgreSQL/BaCS.Persistence.PostgreSQL.csproj", "Source/Persistence/BaCS.Persistence.PostgreSQL/"]
COPY ["Source/Presentation/BaCS.Presentation.BusinessProcess.Blazor/BaCS.Presentation.BusinessProcess.Blazor.csproj", "Source/Presentation/BaCS.Presentation.BusinessProcess.Blazor/"]
COPY . .
RUN dotnet restore "Source/Presentation/BaCS.Presentation.BusinessProcess/BaCS.Presentation.BusinessProcess.csproj"
RUN dotnet build --tl --no-restore "Source/Presentation/BaCS.Presentation.BusinessProcess/BaCS.Presentation.BusinessProcess.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Source/Presentation/BaCS.Presentation.BusinessProcess/BaCS.Presentation.BusinessProcess.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaCS.Presentation.BusinessProcess.dll"]
