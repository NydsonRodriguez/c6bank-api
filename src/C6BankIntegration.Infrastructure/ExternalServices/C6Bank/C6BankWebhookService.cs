using C6BankIntegration.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank;

/// <summary>Implementação do serviço de Webhooks para a API do C6 Bank.</summary>
public sealed class C6BankWebhookService : IC6BankWebhookService
{
    private readonly C6BankHttpClient _httpClient;
    private readonly IC6BankAuthService _authService;
    private readonly ILogger<C6BankWebhookService> _logger;

    /// <summary>Inicializa o serviço de webhooks.</summary>
    public C6BankWebhookService(
        C6BankHttpClient httpClient,
        IC6BankAuthService authService,
        ILogger<C6BankWebhookService> logger)
    {
        _httpClient = httpClient;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>Registra um webhook para uma chave Pix no C6 Bank.</summary>
    public async Task CreateAsync(string pixKey, string webhookUrl, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Registrando webhook para chave Pix {PixKey}.", pixKey);

        var token = await _authService.GetAccessTokenAsync(cancellationToken);
        var body = new { webhookUrl };

        await _httpClient.PostAsync<object, object>(
            $"/webhooks/{pixKey}", body, token, cancellationToken);
    }

    /// <summary>Remove o webhook de uma chave Pix no C6 Bank.</summary>
    public async Task DeleteAsync(string pixKey, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Removendo webhook da chave Pix {PixKey}.", pixKey);

        var token = await _authService.GetAccessTokenAsync(cancellationToken);
        await _httpClient.DeleteAsync($"/webhooks/{pixKey}", token, cancellationToken);
    }
}
