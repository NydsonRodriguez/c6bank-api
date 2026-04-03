using AutoMapper;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.Interfaces.Repositories;

namespace C6BankIntegration.Application.UseCases.Pix;

/// <summary>Caso de uso para consulta de cobrança Pix por txid.</summary>
public sealed class GetPixChargeUseCase : IUseCase<string, PixChargeResponse>
{
    private readonly IPixChargeRepository _pixChargeRepository;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public GetPixChargeUseCase(IPixChargeRepository pixChargeRepository, IMapper mapper)
    {
        _pixChargeRepository = pixChargeRepository;
        _mapper = mapper;
    }

    /// <summary>Obtém uma cobrança Pix pelo txid.</summary>
    public async Task<PixChargeResponse> ExecuteAsync(string request, CancellationToken cancellationToken = default)
    {
        var pixCharge = await _pixChargeRepository.GetByTxidAsync(request, cancellationToken)
            ?? throw new BusinessRuleException($"Cobrança Pix com txid {request} não encontrada.", "PIX_CHARGE_NOT_FOUND");

        return _mapper.Map<PixChargeResponse>(pixCharge);
    }
}
