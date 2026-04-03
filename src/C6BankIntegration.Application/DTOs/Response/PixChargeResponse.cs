using C6BankIntegration.Domain.Enums;

namespace C6BankIntegration.Application.DTOs.Response;

/// <summary>Dados de resposta de uma cobrança Pix.</summary>
public sealed record PixChargeResponse
{
    /// <summary>ID interno.</summary>
    public Guid Id { get; init; }

    /// <summary>Identificador da transação Pix.</summary>
    public string Txid { get; init; } = string.Empty;

    /// <summary>Chave Pix do recebedor.</summary>
    public string PixKey { get; init; } = string.Empty;

    /// <summary>Valor da cobrança.</summary>
    public decimal Amount { get; init; }

    /// <summary>Documento do devedor.</summary>
    public string? DebtorDocument { get; init; }

    /// <summary>Nome do devedor.</summary>
    public string? DebtorName { get; init; }

    /// <summary>Tipo da cobrança Pix.</summary>
    public PixChargeType ChargeType { get; init; }

    /// <summary>Status da cobrança.</summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>Data de expiração (cobranças imediatas).</summary>
    public int? ExpirationSeconds { get; init; }

    /// <summary>Data de vencimento (cobranças com vencimento).</summary>
    public DateOnly? DueDate { get; init; }

    /// <summary>URL do QR Code.</summary>
    public string? Location { get; init; }

    /// <summary>Payload do QR Code (copia e cola).</summary>
    public string? QrCodePayload { get; init; }

    /// <summary>Informações adicionais.</summary>
    public string? AdditionalInfo { get; init; }

    /// <summary>Data de criação.</summary>
    public DateTime CreatedAt { get; init; }
}
