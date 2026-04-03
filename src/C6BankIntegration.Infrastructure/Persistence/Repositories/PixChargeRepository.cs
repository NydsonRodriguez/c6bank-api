using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace C6BankIntegration.Infrastructure.Persistence.Repositories;

/// <summary>Repositório de cobranças Pix usando Entity Framework Core.</summary>
public sealed class PixChargeRepository : IPixChargeRepository
{
    private readonly AppDbContext _context;

    /// <summary>Inicializa o repositório.</summary>
    public PixChargeRepository(AppDbContext context) => _context = context;

    /// <inheritdoc/>
    public async Task<PixCharge?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.PixCharges.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    /// <inheritdoc/>
    public async Task<PixCharge?> GetByTxidAsync(string txid, CancellationToken cancellationToken = default) =>
        await _context.PixCharges.FirstOrDefaultAsync(p => p.Txid == txid, cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<PixCharge>> GetAllAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default) =>
        await _context.PixCharges
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task AddAsync(PixCharge pixCharge, CancellationToken cancellationToken = default)
    {
        await _context.PixCharges.AddAsync(pixCharge, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(PixCharge pixCharge, CancellationToken cancellationToken = default)
    {
        _context.PixCharges.Update(pixCharge);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
