namespace C6BankIntegration.Application.DTOs.Request;

/// <summary>Dados para atualização de um boleto existente.</summary>
public sealed record UpdateBoletoRequest
{
    /// <summary>Nova data de vencimento. Deve ser maior ou igual a hoje.</summary>
    public DateOnly? DueDate { get; init; }

    /// <summary>Novo valor do boleto. Deve ser maior que zero.</summary>
    public decimal? Amount { get; init; }

    /// <summary>Nova taxa de juros mensal (%).</summary>
    public decimal? InterestRate { get; init; }

    /// <summary>Novo percentual de multa (%).</summary>
    public decimal? FineRate { get; init; }

    /// <summary>Novo valor de desconto.</summary>
    public decimal? DiscountAmount { get; init; }

    /// <summary>Nova descrição do boleto.</summary>
    public string? Description { get; init; }
}
