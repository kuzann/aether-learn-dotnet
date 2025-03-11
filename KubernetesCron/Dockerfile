# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore dependencies
COPY *.sln ./
COPY Aether.Kubernetes.Api/Aether.Kubernetes.Api.csproj Aether.Kubernetes.Api/
COPY Aether.Kubernetes.Console/Aether.Kubernetes.Console.csproj Aether.Kubernetes.Console/
COPY Aether.Kubernetes.Cron/Aether.Kubernetes.Cron.csproj Aether.Kubernetes.Cron/
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Default command
CMD ["dotnet", "Aether.Kubernetes.Api.dll"]
