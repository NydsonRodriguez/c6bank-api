using Asp.Versioning;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.UseCases.Boletos;
using C6BankIntegration.Application.UseCases.Pix;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace C6BankIntegration.API.Controllers.v1;

/// <summary>Controller para operações com cobranças Pix via C6 Bank.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pix")]
[Produces("application/json")]
[SwaggerTag("Operações de Cobrança Pix")]
public sealed class PixController : ControllerBase
{
    private readonly CreatePixImmediateChargeUseCase _createImmediateUseCase;
    private readonly CreatePixDueDateChargeUseCase _createDueDateUseCase;
    private readonly GetPixChargeUseCase _getPixChargeUseCase;
    private readonly ListPixChargesUseCase _listPixChargesUseCase;

    /// <summary>Inicializa o controller com os casos de uso necessários.</summary>
    public PixController(
        CreatePixImmediateChargeUseCase createImmediateUseCase,
        CreatePixDueDateChargeUseCase createDueDateUseCase,
        GetPixChargeUseCase getPixChargeUseCase,
        ListPixChargesUseCase listPixChargesUseCase)
    {
        _createImmediateUseCase = createImmediateUseCase;
        _createDueDateUseCase = createDueDateUseCase;
        _getPixChargeUseCase = getPixChargeUseCase;
        _listPixChargesUseCase = listPixChargesUseCase;
    }

    /// <summary>Cria uma cobrança Pix imediata.</summary>
    /// <param name="request">Dados da cobrança.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Cobrança criada com QR Code.</returns>
    /// <response code="201">Cobrança criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="422">Erro de validação.</response>
    [HttpPost("cob")]
    [ProducesResponseType(typeof(PixChargeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateImmediateCharge(
        [FromBody] CreatePixChargeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _createImmediateUseCase.ExecuteAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetImmediateCharge), new { txid = result.Txid }, result);
    }

    /// <summary>Consulta uma cobrança Pix imediata pelo txid.</summary>
    /// <param name="txid">Identificador único da transação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Dados da cobrança.</returns>
    /// <response code="200">Cobrança encontrada.</response>
    /// <response code="404">Cobrança não encontrada.</response>
    [HttpGet("cob/{txid}")]
    [ProducesResponseType(typeof(PixChargeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImmediateCharge(
        [FromRoute] string txid,
        CancellationToken cancellationToken)
    {
        var result = await _getPixChargeUseCase.ExecuteAsync(txid, cancellationToken);
        return Ok(result);
    }

    /// <summary>Lista cobranças Pix imediatas com paginação.</summary>
    /// <param name="page">Número da página.</param>
    /// <param name="pageSize">Itens por página.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de cobranças.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("cob")]
    [ProducesResponseType(typeof(IReadOnlyList<PixChargeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListImmediateCharges(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _listPixChargesUseCase.ExecuteAsync(
            new PaginationParam(page, pageSize), cancellationToken);
        return Ok(result);
    }

    /// <summary>Cria uma cobrança Pix com data de vencimento.</summary>
    /// <param name="request">Dados da cobrança com vencimento.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Cobrança com vencimento criada.</returns>
    /// <response code="201">Cobrança criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="422">Erro de validação.</response>
    [HttpPost("cobv")]
    [ProducesResponseType(typeof(PixChargeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateDueDateCharge(
        [FromBody] CreatePixChargeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _createDueDateUseCase.ExecuteAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetDueDateCharge), new { txid = result.Txid }, result);
    }

    /// <summary>Consulta uma cobrança Pix com vencimento pelo txid.</summary>
    /// <param name="txid">Identificador único da transação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Dados da cobrança com vencimento.</returns>
    /// <response code="200">Cobrança encontrada.</response>
    /// <response code="404">Cobrança não encontrada.</response>
    [HttpGet("cobv/{txid}")]
    [ProducesResponseType(typeof(PixChargeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDueDateCharge(
        [FromRoute] string txid,
        CancellationToken cancellationToken)
    {
        var result = await _getPixChargeUseCase.ExecuteAsync(txid, cancellationToken);
        return Ok(result);
    }

    /// <summary>Lista cobranças Pix com vencimento.</summary>
    /// <param name="page">Número da página.</param>
    /// <param name="pageSize">Itens por página.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de cobranças com vencimento.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet("cobv")]
    [ProducesResponseType(typeof(IReadOnlyList<PixChargeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListDueDateCharges(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _listPixChargesUseCase.ExecuteAsync(
            new PaginationParam(page, pageSize), cancellationToken);
        return Ok(result);
    }
}
