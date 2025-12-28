# Use the SDK base image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["backend-service/CisApi.sln", "backend-service/"]
COPY ["backend-service/CisApi.Core.Application/CisApi.Core.Application.csproj", "backend-service/CisApi.Core.Application/"]
COPY ["backend-service/Server/Server.csproj", "backend-service/Server/"]
COPY ["backend-service/CisApi.Infrastructure.MySQL/CisApi.Infrastructure.MySQL.csproj", "backend-service/CisApi.Infrastructure.MySQL/"]
COPY ["backend-service/CisApi.Core.Domain/CisApi.Core.Domain.csproj", "backend-service/CisApi.Core.Domain/"]
COPY ["backend-service/CisApi.Presentation/CisApi.Presentation.csproj", "backend-service/CisApi.Presentation/"]
COPY ["backend-service/CisApi.Infrastructure.MySQL.Tests/CisApi.Infrastructure.MySQL.Tests.csproj", "backend-service/CisApi.Infrastructure.MySQL.Tests/"]
COPY ["backend-service/CisApi.Presentation.Tests/CisApi.Presentation.Tests.csproj", "backend-service/CisApi.Presentation.Tests/"]
COPY ["backend-service/CisApi.Infrastructure.UsersApi/CisApi.Infrastructure.UsersApi.csproj", "backend-service/CisApi.Infrastructure.UsersApi/"]
COPY ["backend-service/CisApi.Infrastructure.IoC/CisApi.Infrastructure.IoC.csproj", "backend-service/CisApi.Infrastructure.IoC/"]
COPY ["backend-service/CisApi.DataMigration/CisApi.DataMigration.csproj", "backend-service/CisApi.DataMigration/"]
COPY ["backend-service/CisApi.Infrastructure.MongoDb/CisApi.Infrastructure.MongoDb.csproj", "backend-service/CisApi.Infrastructure.MongoDb/"]

# Restore dependencies
RUN dotnet restore "backend-service/CisApi.sln"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/backend-service/Server"
RUN dotnet build "Server.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.dll"]
