using C6BankIntegration.Domain.Enums;
using C6BankIntegration.Domain.ValueObjects;

namespace C6BankIntegration.Domain.Entities;

/// <summary>
/// Representa uma cobrança Pix no domínio.
/// </summary>
public sealed class PixCharge : BaseEntity
{
    /// <summary>Identificador único da transação Pix (txid).</summary>
    public string Txid { get; private set; } = string.Empty;

    /// <summary>Chave Pix do recebedor.</summary>
    public string PixKey { get; private set; } = string.Empty;

    /// <summary>Valor da cobrança.</summary>
    public Money Amount { get; private set; } = null!;

    /// <summary>Documento do devedor.</summary>
    public Document? DebtorDocument { get; private set; }

    /// <summary>Nome do devedor.</summary>
    public string? DebtorName { get; private set; }

    /// <summary>Tipo da cobrança Pix.</summary>
    public PixChargeType ChargeType { get; private set; }

    /// <summary>Status da cobrança.</summary>
    public string Status { get; private set; } = "ATIVA";

    /// <summary>Data de criação da cobrança.</summary>
    public DateTime ChargeCreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>Data de expiração (para cobranças imediatas, em segundos).</summary>
    public int? ExpirationSeconds { get; private set; }

    /// <summary>Data de vencimento (para cobranças com vencimento).</summary>
    public DateOnly? DueDate { get; private set; }

    /// <summary>URL de location do QR Code.</summary>
    public string? Location { get; private set; }

    /// <summary>Payload do QR Code.</summary>
    public string? QrCodePayload { get; private set; }

    /// <summary>Informações adicionais da cobrança.</summary>
    public string? AdditionalInfo { get; private set; }

    private PixCharge() { }

    /// <summary>Cria uma cobrança Pix imediata.</summary>
    public static PixCharge CreateImmediate(
        string pixKey,
        Money amount,
        int expirationSeconds = 3600,
        Document? debtorDocument = null,
        string? debtorName = null,
        string? additionalInfo = null)
    {
        return new PixCharge
        {
            Txid = GenerateTxid(),
            PixKey = pixKey,
            Amount = amount,
            ChargeType = PixChargeType.Immediate,
            ExpirationSeconds = expirationSeconds,
            DebtorDocument = debtorDocument,
            DebtorName = debtorName,
            AdditionalInfo = additionalInfo
        };
    }

    /// <summary>Cria uma cobrança Pix com vencimento.</summary>
    public static PixCharge CreateWithDueDate(
        string pixKey,
        Money amount,
        DateOnly dueDate,
        Document? debtorDocument = null,
        string? debtorName = null,
        string? additionalInfo = null)
    {
        return new PixCharge
        {
            Txid = GenerateTxid(),
            PixKey = pixKey,
            Amount = amount,
            ChargeType = PixChargeType.DueDate,
            DueDate = dueDate,
            DebtorDocument = debtorDocument,
            DebtorName = debtorName,
            AdditionalInfo = additionalInfo
        };
    }

    /// <summary>Define os dados externos retornados pela API do C6 Bank.</summary>
    public void SetExternalData(string location, string qrCodePayload, string status)
    {
        Location = location;
        QrCodePayload = qrCodePayload;
        Status = status;
        MarkAsUpdated();
    }

    private static string GenerateTxid() =>
        (Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"))[..35];
}
