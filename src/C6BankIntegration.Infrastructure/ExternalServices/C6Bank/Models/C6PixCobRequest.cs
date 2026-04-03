using System.Text.Json.Serialization;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;

/// <summary>Requisição de cobrança Pix para a API C6 Bank.</summary>
public sealed class C6PixCobRequest
{
    /// <summary>Calendário da cobrança.</summary>
    [JsonPropertyName("calendario")]
    public C6Calendario Calendario { get; set; } = new();

    /// <summary>Devedor da cobrança.</summary>
    [JsonPropertyName("devedor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public C6Devedor? Devedor { get; set; }

    /// <summary>Valor da cobrança.</summary>
    [JsonPropertyName("valor")]
    public C6Valor Valor { get; set; } = new();

    /// <summary>Chave Pix do recebedor.</summary>
    [JsonPropertyName("chave")]
    public string Chave { get; set; } = string.Empty;

    /// <summary>Informações adicionais.</summary>
    [JsonPropertyName("infoAdicionais")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InfoAdicionais { get; set; }
}

/// <summary>Calendário de cobrança Pix.</summary>
public sealed class C6Calendario
{
    /// <summary>Expiração em segundos (cobranças imediatas).</summary>
    [JsonPropertyName("expiracao")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Expiracao { get; set; }

    /// <summary>Data de vencimento (cobranças com vencimento).</summary>
    [JsonPropertyName("dataDeVencimento")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DataDeVencimento { get; set; }
}

/// <summary>Devedor da cobrança Pix.</summary>
public sealed class C6Devedor
{
    /// <summary>CPF do devedor.</summary>
    [JsonPropertyName("cpf")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Cpf { get; set; }

    /// <summary>CNPJ do devedor.</summary>
    [JsonPropertyName("cnpj")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Cnpj { get; set; }

    /// <summary>Nome do devedor.</summary>
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
}

/// <summary>Valor da cobrança Pix.</summary>
public sealed class C6Valor
{
    /// <summary>Valor original da cobrança.</summary>
    [JsonPropertyName("original")]
    public string Original { get; set; } = "0.00";
}
