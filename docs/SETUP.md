# Guia de Configuração — C6 Bank Integration API

## Pré-requisitos

| Requisito | Versão Mínima | Link |
|-----------|---------------|------|
| .NET SDK | 8.0 | https://dotnet.microsoft.com/download |
| Docker Desktop | 4.x | https://www.docker.com/products/docker-desktop |
| SQL Server | 2019+ | Ou use o Docker Compose |
| Git | 2.x | https://git-scm.com |

## Configuração Rápida (Docker)

```bash
# Clone o repositório
git clone <url-do-repositório>
cd C6BankIntegration

# Suba os containers
docker-compose up -d

# Acesse o Swagger
open http://localhost:8080/swagger
```

## Configuração Manual

### 1. Banco de Dados

Configure o SQL Server e crie o banco:

```sql
CREATE DATABASE C6BankIntegration;
```

Atualize a connection string em `src/C6BankIntegration.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=C6BankIntegration;User Id=sa;Password=SuaSenha;TrustServerCertificate=true"
  }
}
```

### 2. Credenciais C6 Bank

Use o **User Secrets** para não expor credenciais no código:

```bash
cd src/C6BankIntegration.API

# Inicializar user secrets
dotnet user-secrets init

# Configurar credenciais
dotnet user-secrets set "C6Bank:ClientId" "seu-client-id"
dotnet user-secrets set "C6Bank:ClientSecret" "seu-client-secret"

# Para mTLS (opcional)
dotnet user-secrets set "C6Bank:CertificatePath" "/caminho/para/certificado.pfx"
dotnet user-secrets set "C6Bank:CertificatePassword" "senha-do-certificado"
```

### 3. Migrações EF Core

```bash
# Criar migração inicial
dotnet ef migrations add InitialCreate \
  --project src/C6BankIntegration.Infrastructure \
  --startup-project src/C6BankIntegration.API

# Aplicar ao banco
dotnet ef database update \
  --project src/C6BankIntegration.Infrastructure \
  --startup-project src/C6BankIntegration.API
```

### 4. Executar a API

```bash
dotnet run --project src/C6BankIntegration.API
```

Acesse:
- Swagger UI: http://localhost:5000/swagger
- Health Check: http://localhost:5000/health

## Variáveis de Ambiente

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `C6Bank__BaseUrl` | URL base da API C6 Bank | `https://developers.c6bank.com.br` |
| `C6Bank__ClientId` | Client ID OAuth2 | `seu-client-id` |
| `C6Bank__ClientSecret` | Client Secret OAuth2 | `seu-client-secret` |
| `C6Bank__Environment` | Ambiente (`Sandbox`/`Production`) | `Sandbox` |
| `ConnectionStrings__DefaultConnection` | Connection string SQL Server | `Server=...` |

## Executar Testes

```bash
# Todos os testes
dotnet test

# Com relatório de cobertura
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# Gerar relatório HTML (requer reportgenerator)
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:coverage/**/coverage.cobertura.xml -targetdir:coverage/html -reporttypes:Html
```

## Verificação Pós-Setup

```bash
# Build sem warnings
dotnet build -c Release

# Todos os testes passando
dotnet test

# Docker build
docker build -f src/C6BankIntegration.API/Dockerfile -t c6bank-api .
```
