using AutoMapper;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Enums;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;

namespace C6BankIntegration.Application.UseCases.Webhooks;

/// <summary>Caso de uso para registro de webhooks Pix.</summary>
public sealed class CreateWebhookUseCase : IUseCase<CreateWebhookRequest, WebhookResponse>
{
    private readonly IWebhookRepository _webhookRepository;
    private readonly IC6BankWebhookService _webhookService;
    private readonly IMapper _mapper;

    /// <summary>Inicializa o caso de uso.</summary>
    public CreateWebhookUseCase(
        IWebhookRepository webhookRepository,
        IC6BankWebhookService webhookService,
        IMapper mapper)
    {
        _webhookRepository = webhookRepository;
        _webhookService = webhookService;
        _mapper = mapper;
    }

    /// <summary>Cria e registra um novo webhook no C6 Bank.</summary>
    public async Task<WebhookResponse> ExecuteAsync(
        CreateWebhookRequest request,
        CancellationToken cancellationToken = default)
    {
        await _webhookService.CreateAsync(request.PixKey, request.WebhookUrl, cancellationToken);

        var webhook = Webhook.Create(
            request.PixKey,
            request.WebhookUrl,
            [WebhookEventType.PixReceived, WebhookEventType.ChargeCompleted]);

        await _webhookRepository.AddAsync(webhook, cancellationToken);

        return _mapper.Map<WebhookResponse>(webhook);
    }
}
