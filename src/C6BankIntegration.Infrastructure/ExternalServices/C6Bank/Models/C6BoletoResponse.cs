using System.Text.Json.Serialization;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;

/// <summary>Resposta da API C6 Bank para operações de boleto.</summary>
public sealed class C6BoletoResponse
{
    /// <summary>ID do boleto na API C6 Bank.</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>Linha digitável do boleto.</summary>
    [JsonPropertyName("linhaDigitavel")]
    public string LinhaDigitavel { get; set; } = string.Empty;

    /// <summary>Código de barras.</summary>
    [JsonPropertyName("codigoBarras")]
    public string CodigoBarras { get; set; } = string.Empty;

    /// <summary>Status do boleto.</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>Valor do boleto.</summary>
    [JsonPropertyName("valor")]
    public decimal Valor { get; set; }

    /// <summary>Data de vencimento.</summary>
    [JsonPropertyName("dataVencimento")]
    public string DataVencimento { get; set; } = string.Empty;
}
