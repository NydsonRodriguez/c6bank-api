namespace C6BankIntegration.Application.DTOs.Request;

/// <summary>Dados para registro de um webhook Pix.</summary>
public sealed record CreateWebhookRequest
{
    /// <summary>Chave Pix para associar o webhook.</summary>
    /// <example>12345678909</example>
    public string PixKey { get; init; } = string.Empty;

    /// <summary>URL HTTPS de callback para receber as notificações.</summary>
    /// <example>https://meusite.com.br/webhooks/pix</example>
    public string WebhookUrl { get; init; } = string.Empty;
}
