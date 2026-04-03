using Asp.Versioning;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.UseCases.Webhooks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace C6BankIntegration.API.Controllers.v1;

/// <summary>Controller para gerenciamento de Webhooks Pix no C6 Bank.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/webhooks")]
[Produces("application/json")]
[SwaggerTag("Gerenciamento de Webhooks Pix")]
public sealed class WebhooksController : ControllerBase
{
    private readonly CreateWebhookUseCase _createWebhookUseCase;
    private readonly GetWebhookUseCase _getWebhookUseCase;
    private readonly DeleteWebhookUseCase _deleteWebhookUseCase;

    /// <summary>Inicializa o controller com os casos de uso necessários.</summary>
    public WebhooksController(
        CreateWebhookUseCase createWebhookUseCase,
        GetWebhookUseCase getWebhookUseCase,
        DeleteWebhookUseCase deleteWebhookUseCase)
    {
        _createWebhookUseCase = createWebhookUseCase;
        _getWebhookUseCase = getWebhookUseCase;
        _deleteWebhookUseCase = deleteWebhookUseCase;
    }

    /// <summary>Registra um webhook para uma chave Pix.</summary>
    /// <param name="request">Dados do webhook (chave Pix e URL de callback).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Webhook registrado.</returns>
    /// <response code="201">Webhook registrado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="422">Erro de validação.</response>
    [HttpPost]
    [ProducesResponseType(typeof(WebhookResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateWebhookRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _createWebhookUseCase.ExecuteAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByPixKey), new { chave = result.PixKey }, result);
    }

    /// <summary>Consulta o webhook associado a uma chave Pix.</summary>
    /// <param name="chave">Chave Pix associada ao webhook.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Dados do webhook.</returns>
    /// <response code="200">Webhook encontrado.</response>
    /// <response code="404">Webhook não encontrado.</response>
    [HttpGet("{chave}")]
    [ProducesResponseType(typeof(WebhookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPixKey(
        [FromRoute] string chave,
        CancellationToken cancellationToken)
    {
        var result = await _getWebhookUseCase.ExecuteAsync(chave, cancellationToken);
        return Ok(result);
    }

    /// <summary>Remove o webhook de uma chave Pix.</summary>
    /// <param name="chave">Chave Pix do webhook a ser removido.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Sem conteúdo.</returns>
    /// <response code="204">Webhook removido com sucesso.</response>
    /// <response code="404">Webhook não encontrado.</response>
    [HttpDelete("{chave}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] string chave,
        CancellationToken cancellationToken)
    {
        await _deleteWebhookUseCase.ExecuteAsync(chave, cancellationToken);
        return NoContent();
    }
}
