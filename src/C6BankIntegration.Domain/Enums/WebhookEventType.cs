namespace C6BankIntegration.Domain.Enums;

/// <summary>Tipos de eventos disponíveis para webhooks Pix.</summary>
public enum WebhookEventType
{
    /// <summary>Pix recebido.</summary>
    PixReceived = 0,
    /// <summary>Cobrança Pix paga.</summary>
    ChargeCompleted = 1,
    /// <summary>Cobrança Pix expirada.</summary>
    ChargeExpired = 2,
    /// <summary>Devolução de Pix.</summary>
    PixRefunded = 3
}
