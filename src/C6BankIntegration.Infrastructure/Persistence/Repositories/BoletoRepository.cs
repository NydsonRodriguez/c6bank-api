using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace C6BankIntegration.Infrastructure.Persistence.Repositories;

/// <summary>Repositório de Boletos usando Entity Framework Core.</summary>
public sealed class BoletoRepository : IBoletoRepository
{
    private readonly AppDbContext _context;

    /// <summary>Inicializa o repositório.</summary>
    public BoletoRepository(AppDbContext context) => _context = context;

    /// <inheritdoc/>
    public async Task<Boleto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Boletos.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async Task<Boleto?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default) =>
        await _context.Boletos.FirstOrDefaultAsync(b => b.ExternalId == externalId, cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Boleto>> GetAllAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default) =>
        await _context.Boletos
            .Where(b => b.IsActive)
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task AddAsync(Boleto boleto, CancellationToken cancellationToken = default)
    {
        await _context.Boletos.AddAsync(boleto, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Boleto boleto, CancellationToken cancellationToken = default)
    {
        _context.Boletos.Update(boleto);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Boletos.AnyAsync(b => b.Id == id, cancellationToken);
}
