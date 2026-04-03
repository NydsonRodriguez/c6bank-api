namespace C6BankIntegration.Domain.Interfaces.Services;

/// <summary>Contrato para o serviço de autenticação OAuth2 do C6 Bank.</summary>
public interface IC6BankAuthService
{
    /// <summary>Obtém um token de acesso válido (com cache automático).</summary>
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>Invalida o token em cache, forçando renovação na próxima chamada.</summary>
    void InvalidateToken();
}
