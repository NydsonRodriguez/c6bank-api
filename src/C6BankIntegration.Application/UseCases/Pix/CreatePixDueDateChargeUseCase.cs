using AutoMapper;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;
using C6BankIntegration.Domain.ValueObjects;

namespace C6BankIntegration.Application.UseCases.Pix;

/// <summary>Caso de uso para criação de cobrança Pix com vencimento.</summary>
public sealed class CreatePixDueDateChargeUseCase : IUseCase<CreatePixChargeRequest, PixChargeResponse>
{
    private readonly IPixChargeRepository _pixChargeRepository;
    private readonly IC6BankPixService _pixService;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public CreatePixDueDateChargeUseCase(
        IPixChargeRepository pixChargeRepository,
        IC6BankPixService pixService,
        IMapper mapper)
    {
        _pixChargeRepository = pixChargeRepository;
        _pixService = pixService;
        _mapper = mapper;
    }

    /// <summary>Cria uma cobrança Pix com data de vencimento.</summary>
    public async Task<PixChargeResponse> ExecuteAsync(
        CreatePixChargeRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.DueDate.HasValue)
            throw new BusinessRuleException("A data de vencimento é obrigatória para cobranças com vencimento.", "MISSING_DUE_DATE");

        var amount = new Money(request.Amount);
        Document? debtorDocument = string.IsNullOrEmpty(request.DebtorDocument)
            ? null
            : new Document(request.DebtorDocument);

        var pixCharge = PixCharge.CreateWithDueDate(
            request.PixKey,
            amount,
            request.DueDate.Value,
            debtorDocument,
            request.DebtorName,
            request.AdditionalInfo);

        var externalResult = await _pixService.CreateDueDateChargeAsync(pixCharge, cancellationToken);

        pixCharge.SetExternalData(
            externalResult.Location,
            externalResult.QrCodePayload,
            externalResult.Status);

        await _pixChargeRepository.AddAsync(pixCharge, cancellationToken);

        return _mapper.Map<PixChargeResponse>(pixCharge);
    }
}
