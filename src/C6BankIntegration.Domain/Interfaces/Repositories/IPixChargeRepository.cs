using C6BankIntegration.Domain.Entities;

namespace C6BankIntegration.Domain.Interfaces.Repositories;

/// <summary>Contrato para repositório de cobranças Pix.</summary>
public interface IPixChargeRepository
{
    /// <summary>Obtém uma cobrança Pix pelo ID interno.</summary>
    Task<PixCharge?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Obtém uma cobrança Pix pelo txid.</summary>
    Task<PixCharge?> GetByTxidAsync(string txid, CancellationToken cancellationToken = default);

    /// <summary>Lista cobranças Pix com paginação.</summary>
    Task<IReadOnlyList<PixCharge>> GetAllAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);

    /// <summary>Adiciona uma nova cobrança Pix.</summary>
    Task AddAsync(PixCharge pixCharge, CancellationToken cancellationToken = default);

    /// <summary>Atualiza uma cobrança Pix existente.</summary>
    Task UpdateAsync(PixCharge pixCharge, CancellationToken cancellationToken = default);
}
