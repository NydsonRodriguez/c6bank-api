namespace C6BankIntegration.Application.DTOs.Request;

/// <summary>Dados para criação de um novo boleto bancário.</summary>
public sealed record CreateBoletoRequest
{
    /// <summary>Valor do boleto em reais. Deve ser maior que zero.</summary>
    /// <example>150.00</example>
    public decimal Amount { get; init; }

    /// <summary>Data de vencimento no formato yyyy-MM-dd.</summary>
    /// <example>2025-12-31</example>
    public DateOnly DueDate { get; init; }

    /// <summary>CPF ou CNPJ do pagador (apenas dígitos ou formatado).</summary>
    /// <example>12345678909</example>
    public string PayerDocument { get; init; } = string.Empty;

    /// <summary>Nome completo do pagador.</summary>
    /// <example>João da Silva</example>
    public string PayerName { get; init; } = string.Empty;

    /// <summary>Taxa de juros mensal em percentual. Padrão: 0.</summary>
    /// <example>1.5</example>
    public decimal InterestRate { get; init; }

    /// <summary>Percentual de multa por atraso. Padrão: 0.</summary>
    /// <example>2.0</example>
    public decimal FineRate { get; init; }

    /// <summary>Valor de desconto em reais. Padrão: 0.</summary>
    /// <example>10.00</example>
    public decimal DiscountAmount { get; init; }

    /// <summary>Descrição ou instrução do boleto.</summary>
    /// <example>Pagamento referente ao contrato 123</example>
    public string? Description { get; init; }
}
