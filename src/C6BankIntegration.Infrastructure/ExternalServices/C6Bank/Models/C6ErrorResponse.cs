using System.Text.Json.Serialization;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;

/// <summary>Resposta de erro da API C6 Bank.</summary>
public sealed class C6ErrorResponse
{
    /// <summary>Tipo do erro.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>Título do erro.</summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>Status HTTP do erro.</summary>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>Detalhes do erro.</summary>
    [JsonPropertyName("detail")]
    public string? Detail { get; set; }

    /// <summary>Violações de validação.</summary>
    [JsonPropertyName("violacoes")]
    public IList<C6Violacao>? Violacoes { get; set; }
}

/// <summary>Detalhe de violação de validação.</summary>
public sealed class C6Violacao
{
    /// <summary>Razão da violação.</summary>
    [JsonPropertyName("razao")]
    public string Razao { get; set; } = string.Empty;

    /// <summary>Propriedade que causou a violação.</summary>
    [JsonPropertyName("propriedade")]
    public string Propriedade { get; set; } = string.Empty;
}
