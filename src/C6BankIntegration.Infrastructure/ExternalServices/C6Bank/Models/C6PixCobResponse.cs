using System.Text.Json.Serialization;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;

/// <summary>Resposta da API C6 Bank para operações de cobrança Pix.</summary>
public sealed class C6PixCobResponse
{
    /// <summary>Identificador da transação.</summary>
    [JsonPropertyName("txid")]
    public string Txid { get; set; } = string.Empty;

    /// <summary>Status da cobrança.</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>URL de location do QR Code.</summary>
    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    /// <summary>Payload do QR Code (copia e cola).</summary>
    [JsonPropertyName("pixCopiaECola")]
    public string PixCopiaECola { get; set; } = string.Empty;

    /// <summary>Calendário da cobrança.</summary>
    [JsonPropertyName("calendario")]
    public C6Calendario? Calendario { get; set; }

    /// <summary>Valor da cobrança.</summary>
    [JsonPropertyName("valor")]
    public C6Valor? Valor { get; set; }
}
