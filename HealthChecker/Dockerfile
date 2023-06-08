FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HealthChecker/HealthChecker.csproj", "HealthChecker/"]
RUN dotnet restore "HealthChecker/HealthChecker.csproj"
COPY . .
WORKDIR "/src/HealthChecker"
RUN dotnet build "HealthChecker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthChecker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthChecker.dll"]
