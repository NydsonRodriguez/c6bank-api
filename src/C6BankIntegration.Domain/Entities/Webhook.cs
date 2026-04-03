using C6BankIntegration.Domain.Enums;

namespace C6BankIntegration.Domain.Entities;

/// <summary>
/// Representa um webhook registrado para notificações de eventos Pix.
/// </summary>
public sealed class Webhook : BaseEntity
{
    /// <summary>Chave Pix associada ao webhook.</summary>
    public string PixKey { get; private set; } = string.Empty;

    /// <summary>URL de callback para receber notificações.</summary>
    public string WebhookUrl { get; private set; } = string.Empty;

    /// <summary>Tipos de eventos assinados.</summary>
    public IReadOnlyList<WebhookEventType> EventTypes { get; private set; } = [];

    private Webhook() { }

    /// <summary>Cria um novo webhook.</summary>
    public static Webhook Create(string pixKey, string webhookUrl, IEnumerable<WebhookEventType> eventTypes)
    {
        return new Webhook
        {
            PixKey = pixKey,
            WebhookUrl = webhookUrl,
            EventTypes = eventTypes.ToList().AsReadOnly()
        };
    }
}
