using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Services;
using C6BankIntegration.Domain.ValueObjects;
using C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;
using Microsoft.Extensions.Logging;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank;

/// <summary>Implementação do serviço Pix para a API do C6 Bank.</summary>
public sealed class C6BankPixService : IC6BankPixService
{
    private readonly C6BankHttpClient _httpClient;
    private readonly IC6BankAuthService _authService;
    private readonly ILogger<C6BankPixService> _logger;

    /// <summary>Inicializa o serviço Pix.</summary>
    public C6BankPixService(
        C6BankHttpClient httpClient,
        IC6BankAuthService authService,
        ILogger<C6BankPixService> logger)
    {
        _httpClient = httpClient;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>Cria uma cobrança Pix imediata na API do C6 Bank.</summary>
    public async Task<PixServiceResult> CreateImmediateChargeAsync(
        PixCharge pixCharge,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Criando cobrança Pix imediata {Txid}.", pixCharge.Txid);

        var token = await _authService.GetAccessTokenAsync(cancellationToken);
        var request = MapToC6Request(pixCharge);
        request.Calendario.Expiracao = pixCharge.ExpirationSeconds;

        var response = await _httpClient.PostAsync<C6PixCobRequest, C6PixCobResponse>(
            $"/pix/cob/{pixCharge.Txid}", request, token, cancellationToken);

        return new PixServiceResult(response.Location, response.PixCopiaECola, response.Status);
    }

    /// <summary>Cria uma cobrança Pix com vencimento na API do C6 Bank.</summary>
    public async Task<PixServiceResult> CreateDueDateChargeAsync(
        PixCharge pixCharge,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Criando cobrança Pix com vencimento {Txid}.", pixCharge.Txid);

        var token = await _authService.GetAccessTokenAsync(cancellationToken);
        var request = MapToC6Request(pixCharge);
        request.Calendario.DataDeVencimento = pixCharge.DueDate?.ToString("yyyy-MM-dd");

        var response = await _httpClient.PostAsync<C6PixCobRequest, C6PixCobResponse>(
            $"/pix/cobv/{pixCharge.Txid}", request, token, cancellationToken);

        return new PixServiceResult(response.Location, response.PixCopiaECola, response.Status);
    }

    private static C6PixCobRequest MapToC6Request(PixCharge pixCharge)
    {
        var request = new C6PixCobRequest
        {
            Chave = pixCharge.PixKey,
            Valor = new C6Valor { Original = pixCharge.Amount.Value.ToString("F2") },
            Calendario = new C6Calendario(),
            InfoAdicionais = pixCharge.AdditionalInfo
        };

        if (pixCharge.DebtorDocument is not null)
        {
            request.Devedor = new C6Devedor
            {
                Nome = pixCharge.DebtorName ?? string.Empty
            };

            if (pixCharge.DebtorDocument.IsCpf)
                request.Devedor.Cpf = pixCharge.DebtorDocument.Value;
            else
                request.Devedor.Cnpj = pixCharge.DebtorDocument.Value;
        }

        return request;
    }
}
