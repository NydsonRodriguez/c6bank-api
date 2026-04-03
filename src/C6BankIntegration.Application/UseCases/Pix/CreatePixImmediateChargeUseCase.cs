using AutoMapper;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;
using C6BankIntegration.Domain.ValueObjects;

namespace C6BankIntegration.Application.UseCases.Pix;

/// <summary>Caso de uso para criação de cobrança Pix imediata.</summary>
public sealed class CreatePixImmediateChargeUseCase : IUseCase<CreatePixChargeRequest, PixChargeResponse>
{
    private readonly IPixChargeRepository _pixChargeRepository;
    private readonly IC6BankPixService _pixService;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public CreatePixImmediateChargeUseCase(
        IPixChargeRepository pixChargeRepository,
        IC6BankPixService pixService,
        IMapper mapper)
    {
        _pixChargeRepository = pixChargeRepository;
        _pixService = pixService;
        _mapper = mapper;
    }

    /// <summary>Cria uma cobrança Pix imediata.</summary>
    public async Task<PixChargeResponse> ExecuteAsync(
        CreatePixChargeRequest request,
        CancellationToken cancellationToken = default)
    {
        var amount = new Money(request.Amount);
        Document? debtorDocument = string.IsNullOrEmpty(request.DebtorDocument)
            ? null
            : new Document(request.DebtorDocument);

        var pixCharge = PixCharge.CreateImmediate(
            request.PixKey,
            amount,
            request.ExpirationSeconds,
            debtorDocument,
            request.DebtorName,
            request.AdditionalInfo);

        var externalResult = await _pixService.CreateImmediateChargeAsync(pixCharge, cancellationToken);

        pixCharge.SetExternalData(
            externalResult.Location,
            externalResult.QrCodePayload,
            externalResult.Status);

        await _pixChargeRepository.AddAsync(pixCharge, cancellationToken);

        return _mapper.Map<PixChargeResponse>(pixCharge);
    }
}
