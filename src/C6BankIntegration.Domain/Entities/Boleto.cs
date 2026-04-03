using C6BankIntegration.Domain.Enums;
using C6BankIntegration.Domain.ValueObjects;

namespace C6BankIntegration.Domain.Entities;

/// <summary>
/// Representa um boleto bancário no domínio.
/// </summary>
public sealed class Boleto : BaseEntity
{
    /// <summary>Identificador do boleto na API C6 Bank.</summary>
    public string? ExternalId { get; private set; }

    /// <summary>Nosso número (identificador interno do boleto).</summary>
    public string NossoNumero { get; private set; } = string.Empty;

    /// <summary>Valor nominal do boleto.</summary>
    public Money Amount { get; private set; } = null!;

    /// <summary>Data de vencimento do boleto.</summary>
    public DateOnly DueDate { get; private set; }

    /// <summary>Documento do pagador (CPF ou CNPJ).</summary>
    public Document PayerDocument { get; private set; } = null!;

    /// <summary>Nome do pagador.</summary>
    public string PayerName { get; private set; } = string.Empty;

    /// <summary>Linha digitável do boleto.</summary>
    public string? DigitableLine { get; private set; }

    /// <summary>Código de barras do boleto.</summary>
    public string? Barcode { get; private set; }

    /// <summary>Status atual do boleto.</summary>
    public BoletoStatus Status { get; private set; } = BoletoStatus.Pending;

    /// <summary>Taxa de juros mensal (%).</summary>
    public decimal InterestRate { get; private set; }

    /// <summary>Percentual de multa por atraso (%).</summary>
    public decimal FineRate { get; private set; }

    /// <summary>Valor de desconto.</summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>Descrição ou instrução do boleto.</summary>
    public string? Description { get; private set; }

    private Boleto() { }

    /// <summary>
    /// Cria uma nova instância de Boleto com os dados obrigatórios.
    /// </summary>
    public static Boleto Create(
        Money amount,
        DateOnly dueDate,
        Document payerDocument,
        string payerName,
        decimal interestRate = 0,
        decimal fineRate = 0,
        decimal discountAmount = 0,
        string? description = null)
    {
        return new Boleto
        {
            Amount = amount,
            DueDate = dueDate,
            PayerDocument = payerDocument,
            PayerName = payerName,
            InterestRate = interestRate,
            FineRate = fineRate,
            DiscountAmount = discountAmount,
            Description = description,
            NossoNumero = GenerateNossoNumero()
        };
    }

    /// <summary>Registra os dados retornados pela API do C6 Bank após criação.</summary>
    public void SetExternalData(string externalId, string digitableLine, string barcode)
    {
        ExternalId = externalId;
        DigitableLine = digitableLine;
        Barcode = barcode;
        Status = BoletoStatus.Active;
        MarkAsUpdated();
    }

    /// <summary>Atualiza o status do boleto.</summary>
    public void UpdateStatus(BoletoStatus newStatus)
    {
        Status = newStatus;
        MarkAsUpdated();
    }

    /// <summary>Cancela o boleto.</summary>
    public void Cancel()
    {
        Status = BoletoStatus.Cancelled;
        MarkAsUpdated();
    }

    private static string GenerateNossoNumero() =>
        DateTime.UtcNow.ToString("yyyyMMddHHmmss") + Random.Shared.Next(1000, 9999);
}
