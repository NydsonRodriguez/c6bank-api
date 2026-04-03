namespace C6BankIntegration.Domain.Enums;

/// <summary>Status possíveis de um boleto bancário.</summary>
public enum BoletoStatus
{
    /// <summary>Boleto pendente de registro no banco.</summary>
    Pending = 0,
    /// <summary>Boleto registrado e ativo.</summary>
    Active = 1,
    /// <summary>Boleto pago.</summary>
    Paid = 2,
    /// <summary>Boleto cancelado.</summary>
    Cancelled = 3,
    /// <summary>Boleto vencido sem pagamento.</summary>
    Overdue = 4,
    /// <summary>Boleto em processamento.</summary>
    Processing = 5
}
