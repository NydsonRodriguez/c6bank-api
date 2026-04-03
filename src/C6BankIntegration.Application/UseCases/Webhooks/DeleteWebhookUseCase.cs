using C6BankIntegration.Application.Interfaces;
using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;

namespace C6BankIntegration.Application.UseCases.Webhooks;

/// <summary>Caso de uso para remoção de webhooks.</summary>
public sealed class DeleteWebhookUseCase : IUseCase<string, bool>
{
    private readonly IWebhookRepository _webhookRepository;
    private readonly IC6BankWebhookService _webhookService;

    /// <summary>Inicializa o caso de uso.</summary>
    public DeleteWebhookUseCase(IWebhookRepository webhookRepository, IC6BankWebhookService webhookService)
    {
        _webhookRepository = webhookRepository;
        _webhookService = webhookService;
    }

    /// <summary>Remove o webhook associado a uma chave Pix.</summary>
    public async Task<bool> ExecuteAsync(string request, CancellationToken cancellationToken = default)
    {
        var webhook = await _webhookRepository.GetByPixKeyAsync(request, cancellationToken)
            ?? throw new BusinessRuleException($"Webhook para a chave Pix {request} não encontrado.", "WEBHOOK_NOT_FOUND");

        await _webhookService.DeleteAsync(request, cancellationToken);
        await _webhookRepository.RemoveAsync(webhook, cancellationToken);

        return true;
    }
}
