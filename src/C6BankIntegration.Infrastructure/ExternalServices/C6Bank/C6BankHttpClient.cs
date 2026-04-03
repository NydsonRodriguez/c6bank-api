using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using C6BankIntegration.Infrastructure.Configuration;
using C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank;

/// <summary>
/// Cliente HTTP tipado para comunicação com a API do C6 Bank.
/// Suporta mTLS, OAuth2 e headers padronizados.
/// </summary>
public sealed class C6BankHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly C6BankSettings _settings;
    private readonly ILogger<C6BankHttpClient> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>Inicializa o cliente HTTP com as configurações do C6 Bank.</summary>
    public C6BankHttpClient(
        HttpClient httpClient,
        IOptions<C6BankSettings> settings,
        ILogger<C6BankHttpClient> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
    }

    /// <summary>Realiza uma requisição POST e deserializa a resposta.</summary>
    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest body,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        using var request = CreateRequest(HttpMethod.Post, endpoint, accessToken);
        request.Content = new StringContent(
            JsonSerializer.Serialize(body, JsonOptions),
            Encoding.UTF8,
            "application/json");

        return await SendAsync<TResponse>(request, cancellationToken);
    }

    /// <summary>Realiza uma requisição GET e deserializa a resposta.</summary>
    public async Task<TResponse> GetAsync<TResponse>(
        string endpoint,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        using var request = CreateRequest(HttpMethod.Get, endpoint, accessToken);
        return await SendAsync<TResponse>(request, cancellationToken);
    }

    /// <summary>Realiza uma requisição PATCH e deserializa a resposta.</summary>
    public async Task<TResponse> PatchAsync<TRequest, TResponse>(
        string endpoint,
        TRequest body,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        using var request = CreateRequest(HttpMethod.Patch, endpoint, accessToken);
        request.Content = new StringContent(
            JsonSerializer.Serialize(body, JsonOptions),
            Encoding.UTF8,
            "application/json");

        return await SendAsync<TResponse>(request, cancellationToken);
    }

    /// <summary>Realiza uma requisição DELETE.</summary>
    public async Task DeleteAsync(
        string endpoint,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        using var request = CreateRequest(HttpMethod.Delete, endpoint, accessToken);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            await ThrowApiException(response, cancellationToken);
    }

    /// <summary>Obtém um token OAuth2 via client_credentials.</summary>
    public async Task<C6TokenResponse> GetTokenAsync(
        string clientId,
        string clientSecret,
        CancellationToken cancellationToken = default)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret
        });

        var response = await _httpClient.PostAsync("/token", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<C6TokenResponse>(json, JsonOptions)
            ?? throw new InvalidOperationException("Falha ao deserializar token OAuth2.");
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, string? accessToken)
    {
        var request = new HttpRequestMessage(method, endpoint);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var correlationId = Guid.NewGuid().ToString();
        request.Headers.Add("X-Correlation-Id", correlationId);

        return request;
    }

    private async Task<TResponse> SendAsync<TResponse>(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Enviando {Method} para {Url}", request.Method, request.RequestUri);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            await ThrowApiException(response, cancellationToken);

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TResponse>(json, JsonOptions)
            ?? throw new InvalidOperationException($"Falha ao deserializar resposta de {request.RequestUri}.");
    }

    private static async Task ThrowApiException(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException(
            $"C6 Bank API retornou {(int)response.StatusCode}: {content}",
            null,
            response.StatusCode);
    }
}
