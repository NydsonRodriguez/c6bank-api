using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Annotations;

namespace C6BankIntegration.API.Controllers.v1;

/// <summary>Controller para verificação de saúde da aplicação.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/health")]
[Produces("application/json")]
[SwaggerTag("Health Check")]
public sealed class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    /// <summary>Inicializa o controller de health check.</summary>
    public HealthController(HealthCheckService healthCheckService)
        => _healthCheckService = healthCheckService;

    /// <summary>Retorna o status de saúde da aplicação e suas dependências.</summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Status de saúde detalhado.</returns>
    /// <response code="200">Aplicação saudável.</response>
    /// <response code="503">Aplicação ou dependência com falha.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetHealth(CancellationToken cancellationToken)
    {
        var report = await _healthCheckService.CheckHealthAsync(cancellationToken);

        var result = new
        {
            Status = report.Status.ToString(),
            Duration = report.TotalDuration,
            Checks = report.Entries.Select(e => new
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Duration = e.Value.Duration,
                Description = e.Value.Description
            })
        };

        return report.Status == HealthStatus.Healthy ? Ok(result) : StatusCode(503, result);
    }
}
