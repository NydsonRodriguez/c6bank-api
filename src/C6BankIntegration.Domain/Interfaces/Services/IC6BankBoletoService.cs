using C6BankIntegration.Domain.Entities;

namespace C6BankIntegration.Domain.Interfaces.Services;

/// <summary>Resultado do serviço externo de boleto.</summary>
public sealed record BoletoServiceResult(string ExternalId, string DigitableLine, string Barcode);

/// <summary>Dados para atualização de boleto no serviço externo.</summary>
public sealed record BoletoUpdateData(
    DateOnly? DueDate,
    decimal? Amount,
    decimal? InterestRate,
    decimal? FineRate,
    decimal? DiscountAmount,
    string? Description);

/// <summary>Contrato para o serviço de Boletos do C6 Bank.</summary>
public interface IC6BankBoletoService
{
    /// <summary>Cria um boleto na API do C6 Bank.</summary>
    Task<BoletoServiceResult> CreateAsync(Boleto boleto, CancellationToken cancellationToken = default);

    /// <summary>Atualiza um boleto existente.</summary>
    Task UpdateAsync(Boleto boleto, BoletoUpdateData data, CancellationToken cancellationToken = default);

    /// <summary>Cancela um boleto.</summary>
    Task CancelAsync(Boleto boleto, CancellationToken cancellationToken = default);
}
