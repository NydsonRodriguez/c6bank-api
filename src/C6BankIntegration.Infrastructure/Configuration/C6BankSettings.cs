namespace C6BankIntegration.Infrastructure.Configuration;

/// <summary>Configurações de integração com a API do C6 Bank.</summary>
public sealed class C6BankSettings
{
    /// <summary>Seção de configuração no appsettings.json.</summary>
    public const string SectionName = "C6Bank";

    /// <summary>URL base da API do C6 Bank.</summary>
    public string BaseUrl { get; set; } = "https://developers.c6bank.com.br";

    /// <summary>Versão da API.</summary>
    public string ApiVersion { get; set; } = "v1";

    /// <summary>Client ID OAuth2.</summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>Client Secret OAuth2.</summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>Caminho para o certificado mTLS (.pfx).</summary>
    public string? CertificatePath { get; set; }

    /// <summary>Senha do certificado mTLS.</summary>
    public string? CertificatePassword { get; set; }

    /// <summary>Ambiente (Sandbox ou Production).</summary>
    public string Environment { get; set; } = "Sandbox";

    /// <summary>Timeout das requisições em segundos.</summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>Indica se é ambiente sandbox.</summary>
    public bool IsSandbox => Environment.Equals("Sandbox", StringComparison.OrdinalIgnoreCase);
}
