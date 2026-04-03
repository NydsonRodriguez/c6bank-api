using System.Text.Json.Serialization;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;

/// <summary>Resposta de autenticação OAuth2 do C6 Bank.</summary>
public sealed class C6TokenResponse
{
    /// <summary>Token de acesso JWT.</summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>Tipo do token.</summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";

    /// <summary>Tempo de expiração em segundos.</summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>Escopo do token.</summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
}
