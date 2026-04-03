namespace C6BankIntegration.Application.DTOs.Response;

/// <summary>Resposta de autenticação OAuth2.</summary>
public sealed record TokenResponse
{
    /// <summary>Token de acesso.</summary>
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>Tipo do token.</summary>
    public string TokenType { get; init; } = "Bearer";

    /// <summary>Tempo de expiração em segundos.</summary>
    public int ExpiresIn { get; init; }
}
