# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy solution and all referenced project files
COPY *.sln ./

COPY ComplytekTest.API/ComplytekTest.API.csproj ComplytekTest.API/
COPY ComplytekTest.API.Application/ComplytekTest.API.Application.csproj ComplytekTest.API.Application/
COPY ComplytekTest.API.Infrastructure/ComplytekTest.API.Infrastructure.csproj ComplytekTest.API.Infrastructure/
COPY ComplytekTest.API.Core/ComplytekTest.API.Core.csproj ComplytekTest.API.Core/
COPY ComplytekTest.API.Test/ComplytekTest.API.Test.csproj ComplytekTest.API.Test/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . ./

# Publish the main API project
RUN dotnet publish ComplytekTest.API/ComplytekTest.API.csproj -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published output
COPY --from=build-env /app/out ./

# Optional: curl for healthchecks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

EXPOSE 8080
ENTRYPOINT ["dotnet", "ComplytekTest.API.dll"]
