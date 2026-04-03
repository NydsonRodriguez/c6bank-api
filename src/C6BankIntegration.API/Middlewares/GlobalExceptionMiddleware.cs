using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace C6BankIntegration.API.Middlewares;

/// <summary>
/// Middleware de tratamento global de exceções.
/// Captura todas as exceções não tratadas e retorna respostas padronizadas.
/// </summary>
public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>Inicializa o middleware.</summary>
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>Processa a requisição e captura exceções.</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.Items["CorrelationId"]?.ToString();

        _logger.LogError(exception,
            "Exceção não tratada. CorrelationId: {CorrelationId}, Path: {Path}",
            correlationId, context.Request.Path);

        var (statusCode, error) = exception switch
        {
            BusinessRuleException ex when ex.ErrorCode.EndsWith("NOT_FOUND") =>
                (HttpStatusCode.NotFound, ApiErrorResponse.Create(ex.ErrorCode, ex.Message, correlationId)),

            BusinessRuleException ex =>
                (HttpStatusCode.UnprocessableEntity, ApiErrorResponse.Create(ex.ErrorCode, ex.Message, correlationId)),

            DomainException ex =>
                (HttpStatusCode.BadRequest, ApiErrorResponse.Create(ex.ErrorCode, ex.Message, correlationId)),

            HttpRequestException ex =>
                (HttpStatusCode.BadGateway, ApiErrorResponse.Create("EXTERNAL_API_ERROR",
                    "Erro na comunicação com a API do C6 Bank.", correlationId)),

            _ =>
                (HttpStatusCode.InternalServerError, ApiErrorResponse.Create("INTERNAL_ERROR",
                    "Ocorreu um erro interno. Por favor, tente novamente.", correlationId))
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(error, JsonOptions));
    }
}
