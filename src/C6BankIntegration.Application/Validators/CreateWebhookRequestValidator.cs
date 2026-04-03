using C6BankIntegration.Application.DTOs.Request;
using FluentValidation;

namespace C6BankIntegration.Application.Validators;

/// <summary>Validador para registro de webhooks Pix.</summary>
public sealed class CreateWebhookRequestValidator : AbstractValidator<CreateWebhookRequest>
{
    /// <summary>Configura as regras de validação.</summary>
    public CreateWebhookRequestValidator()
    {
        RuleFor(x => x.PixKey)
            .NotEmpty().WithMessage("A chave Pix é obrigatória.");

        RuleFor(x => x.WebhookUrl)
            .NotEmpty().WithMessage("A URL do webhook é obrigatória.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out Uri? result) &&
                         (result.Scheme == Uri.UriSchemeHttps))
            .WithMessage("A URL do webhook deve ser uma URL HTTPS válida.");
    }
}
