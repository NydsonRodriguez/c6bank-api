using AutoMapper;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;

namespace C6BankIntegration.Application.UseCases.Boletos;

/// <summary>Parâmetro composto para atualização de boleto.</summary>
public sealed record UpdateBoletoParam(Guid Id, UpdateBoletoRequest Request);

/// <summary>Caso de uso para atualização de boletos.</summary>
public sealed class UpdateBoletoUseCase : IUseCase<UpdateBoletoParam, BoletoResponse>
{
    private readonly IBoletoRepository _boletoRepository;
    private readonly IC6BankBoletoService _boletoService;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public UpdateBoletoUseCase(
        IBoletoRepository boletoRepository,
        IC6BankBoletoService boletoService,
        IMapper mapper)
    {
        _boletoRepository = boletoRepository;
        _boletoService = boletoService;
        _mapper = mapper;
    }

    /// <summary>Atualiza um boleto existente.</summary>
    public async Task<BoletoResponse> ExecuteAsync(
        UpdateBoletoParam request,
        CancellationToken cancellationToken = default)
    {
        var boleto = await _boletoRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new BusinessRuleException($"Boleto com ID {request.Id} não encontrado.", "BOLETO_NOT_FOUND");

        var updateData = new BoletoUpdateData(
            request.Request.DueDate,
            request.Request.Amount,
            request.Request.InterestRate,
            request.Request.FineRate,
            request.Request.DiscountAmount,
            request.Request.Description);

        await _boletoService.UpdateAsync(boleto, updateData, cancellationToken);
        await _boletoRepository.UpdateAsync(boleto, cancellationToken);

        return _mapper.Map<BoletoResponse>(boleto);
    }
}
