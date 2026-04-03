using AutoMapper;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;
using C6BankIntegration.Domain.ValueObjects;

namespace C6BankIntegration.Application.UseCases.Boletos;

/// <summary>Caso de uso para criação de novos boletos bancários.</summary>
public sealed class CreateBoletoUseCase : IUseCase<CreateBoletoRequest, BoletoResponse>
{
    private readonly IBoletoRepository _boletoRepository;
    private readonly IC6BankBoletoService _boletoService;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso com suas dependências.</summary>
    public CreateBoletoUseCase(
        IBoletoRepository boletoRepository,
        IC6BankBoletoService boletoService,
        IMapper mapper)
    {
        _boletoRepository = boletoRepository;
        _boletoService = boletoService;
        _mapper = mapper;
    }

    /// <summary>Cria um novo boleto e registra no C6 Bank.</summary>
    public async Task<BoletoResponse> ExecuteAsync(
        CreateBoletoRequest request,
        CancellationToken cancellationToken = default)
    {
        var document = new Document(request.PayerDocument);
        var amount = new Money(request.Amount);

        var boleto = Boleto.Create(
            amount,
            request.DueDate,
            document,
            request.PayerName,
            request.InterestRate,
            request.FineRate,
            request.DiscountAmount,
            request.Description);

        var externalResult = await _boletoService.CreateAsync(boleto, cancellationToken);

        boleto.SetExternalData(
            externalResult.ExternalId,
            externalResult.DigitableLine,
            externalResult.Barcode);

        await _boletoRepository.AddAsync(boleto, cancellationToken);

        return _mapper.Map<BoletoResponse>(boleto);
    }
}
