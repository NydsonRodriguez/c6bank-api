namespace C6BankIntegration.Application.DTOs.Response;

/// <summary>Resposta padronizada de erro da API.</summary>
public sealed record ApiErrorResponse
{
    /// <summary>Código do erro para identificação programática.</summary>
    public string ErrorCode { get; init; } = string.Empty;

    /// <summary>Mensagem descritiva do erro.</summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>Detalhes adicionais (erros de validação, por exemplo).</summary>
    public IReadOnlyDictionary<string, string[]>? Details { get; init; }

    /// <summary>ID de correlação da requisição.</summary>
    public string? CorrelationId { get; init; }

    /// <summary>Timestamp do erro (UTC).</summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    /// <summary>Cria uma resposta de erro simples.</summary>
    public static ApiErrorResponse Create(string errorCode, string message, string? correlationId = null) =>
        new() { ErrorCode = errorCode, Message = message, CorrelationId = correlationId };

    /// <summary>Cria uma resposta de erro com detalhes de validação.</summary>
    public static ApiErrorResponse CreateValidation(
        IReadOnlyDictionary<string, string[]> details,
        string? correlationId = null) =>
        new()
        {
            ErrorCode = "VALIDATION_ERROR",
            Message = "Um ou mais campos possuem erros de validação.",
            Details = details,
            CorrelationId = correlationId
        };
}
