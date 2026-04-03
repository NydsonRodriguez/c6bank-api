using AutoMapper;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.Interfaces.Repositories;

namespace C6BankIntegration.Application.UseCases.Webhooks;

/// <summary>Caso de uso para consulta de webhook por chave Pix.</summary>
public sealed class GetWebhookUseCase : IUseCase<string, WebhookResponse>
{
    private readonly IWebhookRepository _webhookRepository;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public GetWebhookUseCase(IWebhookRepository webhookRepository, IMapper mapper)
    {
        _webhookRepository = webhookRepository;
        _mapper = mapper;
    }

    /// <summary>Obtém um webhook pela chave Pix.</summary>
    public async Task<WebhookResponse> ExecuteAsync(string request, CancellationToken cancellationToken = default)
    {
        var webhook = await _webhookRepository.GetByPixKeyAsync(request, cancellationToken)
            ?? throw new BusinessRuleException($"Webhook para a chave Pix {request} não encontrado.", "WEBHOOK_NOT_FOUND");

        return _mapper.Map<WebhookResponse>(webhook);
    }
}
