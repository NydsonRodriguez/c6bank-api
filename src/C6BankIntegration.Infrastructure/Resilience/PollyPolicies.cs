using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace C6BankIntegration.Infrastructure.Resilience;

/// <summary>Políticas de resiliência Polly para chamadas HTTP externas.</summary>
public static class PollyPolicies
{
    /// <summary>
    /// Cria uma política combinada de Retry com exponential backoff + Circuit Breaker + Timeout.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy(ILogger logger)
    {
        var retry = GetRetryPolicy(logger);
        var circuitBreaker = GetCircuitBreakerPolicy(logger);

        return Policy.WrapAsync(retry, circuitBreaker);
    }

    /// <summary>Política de retry com exponential backoff (3 tentativas).</summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    logger.LogWarning(
                        "Tentativa {RetryCount} após {Delay}s. Erro: {Error}",
                        retryCount, timespan.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                });

    /// <summary>Circuit Breaker: abre após 5 falhas consecutivas por 30 segundos.</summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ILogger logger) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, duration) =>
                {
                    logger.LogError(
                        "Circuit Breaker aberto por {Duration}s. Erro: {Error}",
                        duration.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                },
                onReset: () => logger.LogInformation("Circuit Breaker resetado."),
                onHalfOpen: () => logger.LogInformation("Circuit Breaker em half-open."));
}
