namespace C6BankIntegration.Application.DTOs.Request;

/// <summary>Dados para criação de uma cobrança Pix.</summary>
public sealed record CreatePixChargeRequest
{
    /// <summary>Chave Pix do recebedor (CPF, CNPJ, e-mail, telefone ou chave aleatória).</summary>
    /// <example>12345678909</example>
    public string PixKey { get; init; } = string.Empty;

    /// <summary>Valor da cobrança em reais. Deve ser maior que zero.</summary>
    /// <example>200.00</example>
    public decimal Amount { get; init; }

    /// <summary>CPF ou CNPJ do devedor (opcional).</summary>
    public string? DebtorDocument { get; init; }

    /// <summary>Nome do devedor (opcional).</summary>
    public string? DebtorName { get; init; }

    /// <summary>Tempo de expiração em segundos (para cobranças imediatas). Padrão: 3600.</summary>
    /// <example>3600</example>
    public int ExpirationSeconds { get; init; } = 3600;

    /// <summary>Data de vencimento (para cobranças com vencimento).</summary>
    public DateOnly? DueDate { get; init; }

    /// <summary>Informações adicionais da cobrança.</summary>
    public string? AdditionalInfo { get; init; }
}
