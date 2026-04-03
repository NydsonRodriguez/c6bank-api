using C6BankIntegration.Domain.Enums;

namespace C6BankIntegration.Application.DTOs.Response;

/// <summary>Dados de resposta de um boleto bancário.</summary>
public sealed record BoletoResponse
{
    /// <summary>ID interno do boleto.</summary>
    public Guid Id { get; init; }

    /// <summary>ID externo no C6 Bank.</summary>
    public string? ExternalId { get; init; }

    /// <summary>Nosso número do boleto.</summary>
    public string NossoNumero { get; init; } = string.Empty;

    /// <summary>Valor do boleto.</summary>
    public decimal Amount { get; init; }

    /// <summary>Data de vencimento.</summary>
    public DateOnly DueDate { get; init; }

    /// <summary>Documento do pagador.</summary>
    public string PayerDocument { get; init; } = string.Empty;

    /// <summary>Nome do pagador.</summary>
    public string PayerName { get; init; } = string.Empty;

    /// <summary>Linha digitável.</summary>
    public string? DigitableLine { get; init; }

    /// <summary>Código de barras.</summary>
    public string? Barcode { get; init; }

    /// <summary>Status do boleto.</summary>
    public BoletoStatus Status { get; init; }

    /// <summary>Taxa de juros mensal (%).</summary>
    public decimal InterestRate { get; init; }

    /// <summary>Percentual de multa (%).</summary>
    public decimal FineRate { get; init; }

    /// <summary>Valor de desconto.</summary>
    public decimal DiscountAmount { get; init; }

    /// <summary>Descrição do boleto.</summary>
    public string? Description { get; init; }

    /// <summary>Data de criação.</summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>Data da última atualização.</summary>
    public DateTime UpdatedAt { get; init; }
}
