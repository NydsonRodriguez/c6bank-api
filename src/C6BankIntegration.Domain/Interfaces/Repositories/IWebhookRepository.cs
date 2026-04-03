using C6BankIntegration.Domain.Entities;

namespace C6BankIntegration.Domain.Interfaces.Repositories;

/// <summary>Contrato para repositório de Webhooks.</summary>
public interface IWebhookRepository
{
    /// <summary>Obtém um webhook pelo ID interno.</summary>
    Task<Webhook?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Obtém um webhook pela chave Pix.</summary>
    Task<Webhook?> GetByPixKeyAsync(string pixKey, CancellationToken cancellationToken = default);

    /// <summary>Lista todos os webhooks ativos.</summary>
    Task<IReadOnlyList<Webhook>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Adiciona um novo webhook.</summary>
    Task AddAsync(Webhook webhook, CancellationToken cancellationToken = default);

    /// <summary>Remove um webhook.</summary>
    Task RemoveAsync(Webhook webhook, CancellationToken cancellationToken = default);
}
