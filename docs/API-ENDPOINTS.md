# API Endpoints — C6 Bank Integration API

Base URL: `https://localhost:5001/api/v1`

## Boletos

| Método | Rota | Descrição | Request Body | Response |
|--------|------|-----------|--------------|----------|
| POST | `/boletos` | Criar novo boleto | `CreateBoletoRequest` | `BoletoResponse` (201) |
| GET | `/boletos/{id}` | Consultar boleto por ID | — | `BoletoResponse` (200) |
| GET | `/boletos` | Listar boletos | `?page=1&pageSize=20` | `BoletoResponse[]` (200) |
| PATCH | `/boletos/{id}` | Atualizar boleto | `UpdateBoletoRequest` | `BoletoResponse` (200) |
| DELETE | `/boletos/{id}` | Cancelar boleto | — | 204 NoContent |

### Exemplo: Criar Boleto

**Request:**
```json
POST /api/v1/boletos
{
  "amount": 150.00,
  "dueDate": "2025-12-31",
  "payerDocument": "529.982.247-25",
  "payerName": "João da Silva",
  "interestRate": 1.0,
  "fineRate": 2.0,
  "discountAmount": 0,
  "description": "Pagamento referente ao contrato #123"
}
```

**Response (201):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "externalId": "BOL-2025-001",
  "nossoNumero": "20251231123456789",
  "amount": 150.00,
  "dueDate": "2025-12-31",
  "payerDocument": "52998224725",
  "payerName": "João da Silva",
  "digitableLine": "34191.09008 12345.678901 23456.789012 1 92340000015000",
  "barcode": "03459012345678901234567890123456789012345678",
  "status": "Active",
  "createdAt": "2025-04-01T10:00:00Z"
}
```

## Pix

| Método | Rota | Descrição | Request Body | Response |
|--------|------|-----------|--------------|----------|
| POST | `/pix/cob` | Criar cobrança imediata | `CreatePixChargeRequest` | `PixChargeResponse` (201) |
| GET | `/pix/cob/{txid}` | Consultar cobrança imediata | — | `PixChargeResponse` (200) |
| GET | `/pix/cob` | Listar cobranças imediatas | `?page=1&pageSize=20` | `PixChargeResponse[]` (200) |
| POST | `/pix/cobv` | Criar cobrança com vencimento | `CreatePixChargeRequest` | `PixChargeResponse` (201) |
| GET | `/pix/cobv/{txid}` | Consultar cobrança com vencimento | — | `PixChargeResponse` (200) |
| GET | `/pix/cobv` | Listar cobranças com vencimento | `?page=1&pageSize=20` | `PixChargeResponse[]` (200) |

### Exemplo: Criar Cobrança Pix Imediata

**Request:**
```json
POST /api/v1/pix/cob
{
  "pixKey": "empresa@email.com.br",
  "amount": 200.00,
  "expirationSeconds": 3600,
  "debtorDocument": "52998224725",
  "debtorName": "Maria Santos",
  "additionalInfo": "Pedido #456"
}
```

**Response (201):**
```json
{
  "id": "abc12345-...",
  "txid": "7978c80f4a4dfd2955a2abb09af4a559f",
  "pixKey": "empresa@email.com.br",
  "amount": 200.00,
  "status": "ATIVA",
  "expirationSeconds": 3600,
  "location": "pix.c6bank.com.br/qr/v2/abc123",
  "qrCodePayload": "00020126580014br.gov.bcb.pix...",
  "createdAt": "2025-04-01T10:00:00Z"
}
```

## Webhooks

| Método | Rota | Descrição | Request Body | Response |
|--------|------|-----------|--------------|----------|
| POST | `/webhooks` | Registrar webhook | `CreateWebhookRequest` | `WebhookResponse` (201) |
| GET | `/webhooks/{chave}` | Consultar webhook | — | `WebhookResponse` (200) |
| DELETE | `/webhooks/{chave}` | Remover webhook | — | 204 NoContent |

## Health Check

| Método | Rota | Descrição | Response |
|--------|------|-----------|----------|
| GET | `/health` | Status da aplicação | JSON com status de cada componente |

## Códigos de Erro Padrão

| Código HTTP | ErrorCode | Descrição |
|-------------|-----------|-----------|
| 400 | `INVALID_DOCUMENT` | CPF ou CNPJ inválido |
| 400 | `INVALID_AMOUNT` | Valor inválido |
| 404 | `BOLETO_NOT_FOUND` | Boleto não encontrado |
| 404 | `PIX_CHARGE_NOT_FOUND` | Cobrança Pix não encontrada |
| 404 | `WEBHOOK_NOT_FOUND` | Webhook não encontrado |
| 422 | `VALIDATION_ERROR` | Erros de validação (com detalhes) |
| 422 | `MISSING_DUE_DATE` | Data de vencimento obrigatória |
| 502 | `EXTERNAL_API_ERROR` | Erro na comunicação com C6 Bank |
| 500 | `INTERNAL_ERROR` | Erro interno do servidor |
