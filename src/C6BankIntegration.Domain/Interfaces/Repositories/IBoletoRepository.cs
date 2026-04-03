using C6BankIntegration.Domain.Entities;

namespace C6BankIntegration.Domain.Interfaces.Repositories;

/// <summary>Contrato para repositório de Boletos.</summary>
public interface IBoletoRepository
{
    /// <summary>Obtém um boleto pelo ID interno.</summary>
    Task<Boleto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Obtém um boleto pelo ID externo (C6 Bank).</summary>
    Task<Boleto?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);

    /// <summary>Lista boletos com filtros opcionais de paginação.</summary>
    Task<IReadOnlyList<Boleto>> GetAllAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);

    /// <summary>Adiciona um novo boleto.</summary>
    Task AddAsync(Boleto boleto, CancellationToken cancellationToken = default);

    /// <summary>Atualiza um boleto existente.</summary>
    Task UpdateAsync(Boleto boleto, CancellationToken cancellationToken = default);

    /// <summary>Verifica se existe boleto com o ID informado.</summary>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
