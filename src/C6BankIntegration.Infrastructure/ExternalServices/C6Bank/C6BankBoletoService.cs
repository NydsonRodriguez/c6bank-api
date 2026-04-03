using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Services;
using C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;
using Microsoft.Extensions.Logging;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank;

/// <summary>Implementação do serviço de Boletos para a API do C6 Bank.</summary>
public sealed class C6BankBoletoService : IC6BankBoletoService
{
    private readonly C6BankHttpClient _httpClient;
    private readonly IC6BankAuthService _authService;
    private readonly ILogger<C6BankBoletoService> _logger;

    /// <summary>Inicializa o serviço de boletos.</summary>
    public C6BankBoletoService(
        C6BankHttpClient httpClient,
        IC6BankAuthService authService,
        ILogger<C6BankBoletoService> logger)
    {
        _httpClient = httpClient;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>Cria um boleto na API do C6 Bank.</summary>
    public async Task<BoletoServiceResult> CreateAsync(Boleto boleto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Criando boleto para {PayerName} no C6 Bank.", boleto.PayerName);

        var token = await _authService.GetAccessTokenAsync(cancellationToken);
        var request = MapToC6Request(boleto);

        var response = await _httpClient.PostAsync<C6BoletoRequest, C6BoletoResponse>(
            "/boletos", request, token, cancellationToken);

        _logger.LogInformation("Boleto criado com ID externo {ExternalId}.", response.Id);

        return new BoletoServiceResult(response.Id, response.LinhaDigitavel, response.CodigoBarras);
    }

    /// <summary>Atualiza um boleto existente na API do C6 Bank.</summary>
    public async Task UpdateAsync(Boleto boleto, BoletoUpdateData data, CancellationToken cancellationToken = default)
    {
        if (boleto.ExternalId is null) return;

        _logger.LogInformation("Atualizando boleto {ExternalId} no C6 Bank.", boleto.ExternalId);

        var token = await _authService.GetAccessTokenAsync(cancellationToken);
        var request = MapToC6Request(boleto);

        if (data.DueDate.HasValue) request.DataVencimento = data.DueDate.Value.ToString("yyyy-MM-dd");
        if (data.Amount.HasValue) request.Valor = data.Amount.Value;
        if (data.InterestRate.HasValue) request.Juros = data.InterestRate.Value;
        if (data.FineRate.HasValue) request.Multa = data.FineRate.Value;
        if (data.DiscountAmount.HasValue) request.Desconto = data.DiscountAmount.Value;
        if (data.Description is not null) request.Descricao = data.Description;

        await _httpClient.PatchAsync<C6BoletoRequest, C6BoletoResponse>(
            $"/boletos/{boleto.ExternalId}", request, token, cancellationToken);
    }

    /// <summary>Cancela um boleto na API do C6 Bank.</summary>
    public async Task CancelAsync(Boleto boleto, CancellationToken cancellationToken = default)
    {
        if (boleto.ExternalId is null) return;

        _logger.LogInformation("Cancelando boleto {ExternalId} no C6 Bank.", boleto.ExternalId);

        var token = await _authService.GetAccessTokenAsync(cancellationToken);
        await _httpClient.DeleteAsync($"/boletos/{boleto.ExternalId}", token, cancellationToken);
    }

    private static C6BoletoRequest MapToC6Request(Boleto boleto) => new()
    {
        Valor = boleto.Amount.Value,
        DataVencimento = boleto.DueDate.ToString("yyyy-MM-dd"),
        PagadorDocumento = boleto.PayerDocument.Value,
        PagadorNome = boleto.PayerName,
        Juros = boleto.InterestRate,
        Multa = boleto.FineRate,
        Desconto = boleto.DiscountAmount,
        Descricao = boleto.Description,
        NossoNumero = boleto.NossoNumero
    };
}
