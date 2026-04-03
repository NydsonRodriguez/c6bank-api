using AutoMapper;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Interfaces.Repositories;

namespace C6BankIntegration.Application.UseCases.Boletos;

/// <summary>Parâmetro de paginação.</summary>
public sealed record PaginationParam(int Page = 1, int PageSize = 20);

/// <summary>Caso de uso para listagem de boletos.</summary>
public sealed class ListBoletosUseCase : IUseCase<PaginationParam, IReadOnlyList<BoletoResponse>>
{
    private readonly IBoletoRepository _boletoRepository;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public ListBoletosUseCase(IBoletoRepository boletoRepository, IMapper mapper)
    {
        _boletoRepository = boletoRepository;
        _mapper = mapper;
    }

    /// <summary>Lista boletos com paginação.</summary>
    public async Task<IReadOnlyList<BoletoResponse>> ExecuteAsync(
        PaginationParam request,
        CancellationToken cancellationToken = default)
    {
        var boletos = await _boletoRepository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        return _mapper.Map<IReadOnlyList<BoletoResponse>>(boletos);
    }
}
