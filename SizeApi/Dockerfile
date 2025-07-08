FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Size.Api/Size.Api.csproj", "Size.Api/"]
COPY ["Size.Application/Size.Application.csproj", "Size.Application/"]
COPY ["Size.Domain/Size.Domain.csproj", "Size.Domain/"]
COPY ["Size.Infrastructure/Size.Infrastructure.csproj", "Size.Infrastructure/"]

RUN dotnet restore "Size.Api/Size.Api.csproj"

COPY . .
WORKDIR "/src/Size.Api"
RUN dotnet build "Size.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Size.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Size.Api.dll"]