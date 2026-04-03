using AutoMapper;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Application.UseCases.Boletos;
using C6BankIntegration.Domain.Interfaces.Repositories;

namespace C6BankIntegration.Application.UseCases.Pix;

/// <summary>Caso de uso para listagem de cobranças Pix.</summary>
public sealed class ListPixChargesUseCase : IUseCase<PaginationParam, IReadOnlyList<PixChargeResponse>>
{
    private readonly IPixChargeRepository _pixChargeRepository;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public ListPixChargesUseCase(IPixChargeRepository pixChargeRepository, IMapper mapper)
    {
        _pixChargeRepository = pixChargeRepository;
        _mapper = mapper;
    }

    /// <summary>Lista cobranças Pix com paginação.</summary>
    public async Task<IReadOnlyList<PixChargeResponse>> ExecuteAsync(
        PaginationParam request,
        CancellationToken cancellationToken = default)
    {
        var charges = await _pixChargeRepository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        return _mapper.Map<IReadOnlyList<PixChargeResponse>>(charges);
    }
}
