using C6BankIntegration.Domain.Interfaces.Services;
using C6BankIntegration.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank;

/// <summary>
/// Serviço de autenticação OAuth2 do C6 Bank com cache automático de token.
/// </summary>
public sealed class C6BankAuthService : IC6BankAuthService
{
    private const string TokenCacheKey = "C6Bank_AccessToken";
    private const int TokenExpirationMarginSeconds = 60;

    private readonly C6BankHttpClient _httpClient;
    private readonly C6BankSettings _settings;
    private readonly IMemoryCache _cache;
    private readonly ILogger<C6BankAuthService> _logger;

    /// <summary>Inicializa o serviço de autenticação.</summary>
    public C6BankAuthService(
        C6BankHttpClient httpClient,
        IOptions<C6BankSettings> settings,
        IMemoryCache cache,
        ILogger<C6BankAuthService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    /// <summary>Obtém um token de acesso válido, usando cache quando possível.</summary>
    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(TokenCacheKey, out string? cachedToken) && !string.IsNullOrEmpty(cachedToken))
        {
            _logger.LogDebug("Token OAuth2 obtido do cache.");
            return cachedToken;
        }

        _logger.LogInformation("Solicitando novo token OAuth2 ao C6 Bank.");

        var tokenResponse = await _httpClient.GetTokenAsync(
            _settings.ClientId,
            _settings.ClientSecret,
            cancellationToken);

        var expirationTime = TimeSpan.FromSeconds(tokenResponse.ExpiresIn - TokenExpirationMarginSeconds);

        _cache.Set(TokenCacheKey, tokenResponse.AccessToken, expirationTime);

        _logger.LogInformation("Token OAuth2 obtido e cacheado por {Seconds}s.", expirationTime.TotalSeconds);

        return tokenResponse.AccessToken;
    }

    /// <summary>Invalida o token em cache, forçando renovação na próxima chamada.</summary>
    public void InvalidateToken()
    {
        _cache.Remove(TokenCacheKey);
        _logger.LogInformation("Token OAuth2 removido do cache.");
    }
}
