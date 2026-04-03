using C6BankIntegration.Domain.Entities;

namespace C6BankIntegration.Domain.Interfaces.Services;

/// <summary>Resultado do serviço externo Pix.</summary>
public sealed record PixServiceResult(string Location, string QrCodePayload, string Status);

/// <summary>Contrato para o serviço Pix do C6 Bank.</summary>
public interface IC6BankPixService
{
    /// <summary>Cria uma cobrança Pix imediata.</summary>
    Task<PixServiceResult> CreateImmediateChargeAsync(PixCharge pixCharge, CancellationToken cancellationToken = default);

    /// <summary>Cria uma cobrança Pix com vencimento.</summary>
    Task<PixServiceResult> CreateDueDateChargeAsync(PixCharge pixCharge, CancellationToken cancellationToken = default);
}
