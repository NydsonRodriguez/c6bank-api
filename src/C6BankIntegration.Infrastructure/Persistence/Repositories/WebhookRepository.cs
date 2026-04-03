using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace C6BankIntegration.Infrastructure.Persistence.Repositories;

/// <summary>Repositório de Webhooks usando Entity Framework Core.</summary>
public sealed class WebhookRepository : IWebhookRepository
{
    private readonly AppDbContext _context;

    /// <summary>Inicializa o repositório.</summary>
    public WebhookRepository(AppDbContext context) => _context = context;

    /// <inheritdoc/>
    public async Task<Webhook?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Webhooks.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async Task<Webhook?> GetByPixKeyAsync(string pixKey, CancellationToken cancellationToken = default) =>
        await _context.Webhooks.FirstOrDefaultAsync(w => w.PixKey == pixKey, cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Webhook>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Webhooks
            .Where(w => w.IsActive)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task AddAsync(Webhook webhook, CancellationToken cancellationToken = default)
    {
        await _context.Webhooks.AddAsync(webhook, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(Webhook webhook, CancellationToken cancellationToken = default)
    {
        _context.Webhooks.Remove(webhook);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
