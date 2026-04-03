using System.Text.Json.Serialization;

namespace C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;

/// <summary>Requisição para criação/atualização de boleto na API C6 Bank.</summary>
public sealed class C6BoletoRequest
{
    /// <summary>Valor do boleto.</summary>
    [JsonPropertyName("valor")]
    public decimal Valor { get; set; }

    /// <summary>Data de vencimento no formato yyyy-MM-dd.</summary>
    [JsonPropertyName("dataVencimento")]
    public string DataVencimento { get; set; } = string.Empty;

    /// <summary>CPF ou CNPJ do pagador.</summary>
    [JsonPropertyName("pagadorDocumento")]
    public string PagadorDocumento { get; set; } = string.Empty;

    /// <summary>Nome do pagador.</summary>
    [JsonPropertyName("pagadorNome")]
    public string PagadorNome { get; set; } = string.Empty;

    /// <summary>Taxa de juros mensal (%).</summary>
    [JsonPropertyName("juros")]
    public decimal Juros { get; set; }

    /// <summary>Percentual de multa por atraso (%).</summary>
    [JsonPropertyName("multa")]
    public decimal Multa { get; set; }

    /// <summary>Valor de desconto.</summary>
    [JsonPropertyName("desconto")]
    public decimal Desconto { get; set; }

    /// <summary>Descrição do boleto.</summary>
    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }

    /// <summary>Nosso número.</summary>
    [JsonPropertyName("nossoNumero")]
    public string? NossoNumero { get; set; }
}
