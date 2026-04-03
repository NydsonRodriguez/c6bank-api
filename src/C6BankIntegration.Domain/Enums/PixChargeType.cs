namespace C6BankIntegration.Domain.Enums;

/// <summary>Tipo de cobrança Pix.</summary>
public enum PixChargeType
{
    /// <summary>Cobrança imediata (sem vencimento fixo).</summary>
    Immediate = 0,
    /// <summary>Cobrança com data de vencimento.</summary>
    DueDate = 1
}
