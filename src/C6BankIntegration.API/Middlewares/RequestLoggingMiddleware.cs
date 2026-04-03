using System.Diagnostics;

namespace C6BankIntegration.API.Middlewares;

/// <summary>
/// Middleware de logging de requisições HTTP.
/// Registra método, path, status e tempo de execução de cada requisição.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    /// <summary>Inicializa o middleware.</summary>
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>Processa a requisição e registra os dados de log.</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Items["CorrelationId"]?.ToString();
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Requisição iniciada: {Method} {Path} | CorrelationId: {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            correlationId);

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation(
            "Requisição concluída: {Method} {Path} | Status: {StatusCode} | Tempo: {ElapsedMs}ms | CorrelationId: {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            correlationId);
    }
}
