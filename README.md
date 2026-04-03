# C6 Bank Integration API

API REST de integração com o **Banco C6 (código 336)**, desenvolvida como demonstração de competências para vaga de **Desenvolvedor Sênior de APIs**.

## Visão Geral

Serviço middleware que encapsula as operações bancárias do C6 Bank em uma arquitetura limpa, testável e bem documentada:

- **Boletos**: emissão, consulta, atualização e cancelamento
- **Pix**: cobranças imediatas e com vencimento
- **Webhooks**: registro e gerenciamento de callbacks Pix

## Tecnologias

| Tecnologia | Versão | Propósito |
|---|---|---|
| .NET | 8.0 (LTS) | Runtime e SDK |
| ASP.NET Core | 8.0 | Web API |
| Entity Framework Core | 8.x | ORM |
| SQL Server | 2022 | Banco de dados |
| FluentValidation | 11.x | Validação de entrada |
| AutoMapper | 13.x | Mapeamento de objetos |
| Polly | 8.x | Resiliência HTTP |
| Serilog | 8.x | Logging estruturado |
| xUnit | 2.x | Testes unitários |
| Moq | 4.x | Mocking em testes |
| Bogus | 35.x | Dados de teste |
| Swashbuckle | 6.x | Swagger/OpenAPI |

## Arquitetura

```
Clean Architecture com 4 camadas:

┌─────────────────────────────────────┐
│            API Layer                │  Controllers, Middlewares, Swagger
├─────────────────────────────────────┤
│         Application Layer           │  Use Cases, DTOs, Validators, Mappings
├─────────────────────────────────────┤
│           Domain Layer              │  Entities, Value Objects, Interfaces
├─────────────────────────────────────┤
│        Infrastructure Layer         │  EF Core, C6 Bank HTTP Client, Polly
└─────────────────────────────────────┘
```

Ver [ARCHITECTURE.md](docs/ARCHITECTURE.md) para diagrama detalhado.

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- SQL Server (ou use o Docker Compose)
- Credenciais da API C6 Bank (sandbox disponível em [developers.c6bank.com.br](https://developers.c6bank.com.br))

## Como Rodar

### Com Docker (recomendado)

```bash
docker-compose up -d
```

A API estará disponível em: http://localhost:8080/swagger

### Sem Docker

1. Configure o SQL Server e atualize a connection string em `appsettings.Development.json`
2. Configure as credenciais do C6 Bank:

```bash
cd src/C6BankIntegration.API
dotnet user-secrets set "C6Bank:ClientId" "seu-client-id"
dotnet user-secrets set "C6Bank:ClientSecret" "seu-client-secret"
```

3. Execute as migrações:

```bash
dotnet ef database update --project src/C6BankIntegration.Infrastructure --startup-project src/C6BankIntegration.API
```

4. Inicie a API:

```bash
dotnet run --project src/C6BankIntegration.API
```

5. Acesse: http://localhost:5000/swagger

## Como Testar

```bash
# Rodar todos os testes
dotnet test

# Rodar com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Rodar apenas unitários
dotnet test tests/C6BankIntegration.UnitTests

# Rodar apenas integração
dotnet test tests/C6BankIntegration.IntegrationTests
```

## Endpoints

Ver [API-ENDPOINTS.md](docs/API-ENDPOINTS.md) para lista completa.

Resumo:

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /api/v1/boletos | Criar boleto |
| GET | /api/v1/boletos/{id} | Consultar boleto |
| PATCH | /api/v1/boletos/{id} | Atualizar boleto |
| DELETE | /api/v1/boletos/{id} | Cancelar boleto |
| POST | /api/v1/pix/cob | Criar cobrança Pix imediata |
| GET | /api/v1/pix/cob/{txid} | Consultar cobrança imediata |
| POST | /api/v1/pix/cobv | Criar cobrança com vencimento |
| POST | /api/v1/webhooks | Registrar webhook |
| DELETE | /api/v1/webhooks/{chave} | Remover webhook |
| GET | /health | Health check |

## Configuração

Ver [SETUP.md](docs/SETUP.md) para guia completo de configuração.

## Licença

MIT
