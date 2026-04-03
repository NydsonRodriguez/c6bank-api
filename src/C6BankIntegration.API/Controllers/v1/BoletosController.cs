using Asp.Versioning;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.UseCases.Boletos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace C6BankIntegration.API.Controllers.v1;

/// <summary>Controller para operações com Boletos Bancários via C6 Bank.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/boletos")]
[Produces("application/json")]
[SwaggerTag("Operações de Boleto Bancário")]
public sealed class BoletosController : ControllerBase
{
    private readonly CreateBoletoUseCase _createBoletoUseCase;
    private readonly GetBoletoUseCase _getBoletoUseCase;
    private readonly UpdateBoletoUseCase _updateBoletoUseCase;
    private readonly CancelBoletoUseCase _cancelBoletoUseCase;
    private readonly ListBoletosUseCase _listBoletosUseCase;

    /// <summary>Inicializa o controller com os casos de uso necessários.</summary>
    public BoletosController(
        CreateBoletoUseCase createBoletoUseCase,
        GetBoletoUseCase getBoletoUseCase,
        UpdateBoletoUseCase updateBoletoUseCase,
        CancelBoletoUseCase cancelBoletoUseCase,
        ListBoletosUseCase listBoletosUseCase)
    {
        _createBoletoUseCase = createBoletoUseCase;
        _getBoletoUseCase = getBoletoUseCase;
        _updateBoletoUseCase = updateBoletoUseCase;
        _cancelBoletoUseCase = cancelBoletoUseCase;
        _listBoletosUseCase = listBoletosUseCase;
    }

    /// <summary>Cria um novo boleto bancário.</summary>
    /// <param name="request">Dados do boleto a ser criado.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Boleto criado com linha digitável e código de barras.</returns>
    /// <response code="201">Boleto criado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="422">Erro de validação ou regra de negócio.</response>
    /// <response code="500">Erro interno do servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(BoletoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBoletoRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _createBoletoUseCase.ExecuteAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Consulta um boleto pelo ID interno.</summary>
    /// <param name="id">ID interno do boleto (GUID).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Dados do boleto.</returns>
    /// <response code="200">Boleto encontrado.</response>
    /// <response code="404">Boleto não encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BoletoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _getBoletoUseCase.ExecuteAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>Lista boletos com paginação.</summary>
    /// <param name="page">Número da página (padrão: 1).</param>
    /// <param name="pageSize">Itens por página (padrão: 20).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de boletos.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BoletoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _listBoletosUseCase.ExecuteAsync(
            new PaginationParam(page, pageSize), cancellationToken);
        return Ok(result);
    }

    /// <summary>Atualiza um boleto existente.</summary>
    /// <param name="id">ID interno do boleto.</param>
    /// <param name="request">Campos a serem atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Boleto atualizado.</returns>
    /// <response code="200">Boleto atualizado com sucesso.</response>
    /// <response code="404">Boleto não encontrado.</response>
    /// <response code="422">Erro de validação.</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(BoletoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateBoletoRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _updateBoletoUseCase.ExecuteAsync(
            new UpdateBoletoParam(id, request), cancellationToken);
        return Ok(result);
    }

    /// <summary>Cancela um boleto.</summary>
    /// <param name="id">ID interno do boleto.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Sem conteúdo.</returns>
    /// <response code="204">Boleto cancelado com sucesso.</response>
    /// <response code="404">Boleto não encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        await _cancelBoletoUseCase.ExecuteAsync(id, cancellationToken);
        return NoContent();
    }
}
