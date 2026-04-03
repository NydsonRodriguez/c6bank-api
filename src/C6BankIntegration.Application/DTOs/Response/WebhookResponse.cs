using C6BankIntegration.Domain.Enums;

namespace C6BankIntegration.Application.DTOs.Response;

/// <summary>Dados de resposta de um webhook Pix.</summary>
public sealed record WebhookResponse
{
    /// <summary>ID interno.</summary>
    public Guid Id { get; init; }

    /// <summary>Chave Pix associada ao webhook.</summary>
    public string PixKey { get; init; } = string.Empty;

    /// <summary>URL de callback.</summary>
    public string WebhookUrl { get; init; } = string.Empty;

    /// <summary>Tipos de eventos assinados.</summary>
    public IReadOnlyList<WebhookEventType> EventTypes { get; init; } = [];

    /// <summary>Data de criação.</summary>
    public DateTime CreatedAt { get; init; }
}
