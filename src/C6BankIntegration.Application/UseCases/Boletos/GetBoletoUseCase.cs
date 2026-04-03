using AutoMapper;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.Interfaces.Repositories;

namespace C6BankIntegration.Application.UseCases.Boletos;

/// <summary>Caso de uso para consulta de boleto por ID.</summary>
public sealed class GetBoletoUseCase : IUseCase<Guid, BoletoResponse>
{
    private readonly IBoletoRepository _boletoRepository;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public GetBoletoUseCase(IBoletoRepository boletoRepository, IMapper mapper)
    {
        _boletoRepository = boletoRepository;
        _mapper = mapper;
    }

    /// <summary>Obtém um boleto pelo ID interno.</summary>
    public async Task<BoletoResponse> ExecuteAsync(
        Guid request,
        CancellationToken cancellationToken = default)
    {
        var boleto = await _boletoRepository.GetByIdAsync(request, cancellationToken)
            ?? throw new BusinessRuleException($"Boleto com ID {request} não encontrado.", "BOLETO_NOT_FOUND");

        return _mapper.Map<BoletoResponse>(boleto);
    }
}
