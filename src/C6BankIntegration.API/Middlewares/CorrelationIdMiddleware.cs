namespace C6BankIntegration.API.Middlewares;

/// <summary>
/// Middleware que gera ou propaga o header X-Correlation-Id para rastreamento de requisições.
/// </summary>
public sealed class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    /// <summary>Inicializa o middleware.</summary>
    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    /// <summary>Processa a requisição e gerencia o Correlation ID.</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers[CorrelationIdHeader] = correlationId;

        await _next(context);
    }
}
