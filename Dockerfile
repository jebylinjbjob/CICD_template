# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["PortalApi.sln", "./"]
COPY ["PortalApi/src/PortalApi.Domain.Shared/PortalApi.Domain.Shared.csproj", "PortalApi/src/PortalApi.Domain.Shared/"]
COPY ["PortalApi/src/PortalApi.Domain/PortalApi.Domain.csproj", "PortalApi/src/PortalApi.Domain/"]
COPY ["PortalApi/src/PortalApi.Application.Contracts/PortalApi.Application.Contracts.csproj", "PortalApi/src/PortalApi.Application.Contracts/"]
COPY ["PortalApi/src/PortalApi.Application/PortalApi.Application.csproj", "PortalApi/src/PortalApi.Application/"]
COPY ["PortalApi/src/PortalApi.EntityFrameworkCore/PortalApi.EntityFrameworkCore.csproj", "PortalApi/src/PortalApi.EntityFrameworkCore/"]
COPY ["PortalApi/src/PortalApi.HttpApi/PortalApi.HttpApi.csproj", "PortalApi/src/PortalApi.HttpApi/"]
COPY ["PortalApi/src/PortalApi.HttpApi.Client/PortalApi.HttpApi.Client.csproj", "PortalApi/src/PortalApi.HttpApi.Client/"]
COPY ["PortalApi/src/PortalApi.HttpApi.Host/PortalApi.HttpApi.Host.csproj", "PortalApi/src/PortalApi.HttpApi.Host/"]
COPY ["PortalApi/src/PortalApi.DbMigrator/PortalApi.DbMigrator.csproj", "PortalApi/src/PortalApi.DbMigrator/"]

# Restore dependencies
RUN dotnet restore "PortalApi.sln"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/PortalApi/src/PortalApi.HttpApi.Host"
RUN dotnet build "PortalApi.HttpApi.Host.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PortalApi.HttpApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files from publish stage
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "PortalApi.HttpApi.Host.dll"]
