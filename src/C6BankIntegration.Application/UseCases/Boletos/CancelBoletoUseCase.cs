using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;

namespace C6BankIntegration.Application.UseCases.Boletos;

/// <summary>Caso de uso para cancelamento de boleto.</summary>
public sealed class CancelBoletoUseCase : IUseCase<Guid, bool>
{
    private readonly IBoletoRepository _boletoRepository;
    private readonly IC6BankBoletoService _boletoService;

    /// <summary>Inicializa o caso de uso.</summary>
    public CancelBoletoUseCase(IBoletoRepository boletoRepository, IC6BankBoletoService boletoService)
    {
        _boletoRepository = boletoRepository;
        _boletoService = boletoService;
    }

    /// <summary>Cancela um boleto pelo ID.</summary>
    public async Task<bool> ExecuteAsync(Guid request, CancellationToken cancellationToken = default)
    {
        var boleto = await _boletoRepository.GetByIdAsync(request, cancellationToken)
            ?? throw new BusinessRuleException($"Boleto com ID {request} não encontrado.", "BOLETO_NOT_FOUND");

        if (boleto.ExternalId is not null)
            await _boletoService.CancelAsync(boleto, cancellationToken);

        boleto.Cancel();
        await _boletoRepository.UpdateAsync(boleto, cancellationToken);

        return true;
    }
}
