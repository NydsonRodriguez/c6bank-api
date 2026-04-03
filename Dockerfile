FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

# Copia apenas os .csproj para aproveitar cache de camadas
COPY src/C6BankIntegration.API/C6BankIntegration.API.csproj src/C6BankIntegration.API/
COPY src/C6BankIntegration.Application/C6BankIntegration.Application.csproj src/C6BankIntegration.Application/
COPY src/C6BankIntegration.Domain/C6BankIntegration.Domain.csproj src/C6BankIntegration.Domain/
COPY src/C6BankIntegration.Infrastructure/C6BankIntegration.Infrastructure.csproj src/C6BankIntegration.Infrastructure/

RUN dotnet restore src/C6BankIntegration.API/C6BankIntegration.API.csproj

# Copia o restante do código-fonte
COPY . .

RUN dotnet build src/C6BankIntegration.API/C6BankIntegration.API.csproj \
    -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish src/C6BankIntegration.API/C6BankIntegration.API.csproj \
    -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "C6BankIntegration.API.dll"]
