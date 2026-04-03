namespace C6BankIntegration.Domain.Interfaces.Services;

/// <summary>Contrato para o serviço de Webhooks do C6 Bank.</summary>
public interface IC6BankWebhookService
{
    /// <summary>Registra um webhook para uma chave Pix.</summary>
    Task CreateAsync(string pixKey, string webhookUrl, CancellationToken cancellationToken = default);

    /// <summary>Remove o webhook de uma chave Pix.</summary>
    Task DeleteAsync(string pixKey, CancellationToken cancellationToken = default);
}
