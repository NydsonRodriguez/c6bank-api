using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Domain.ValueObjects;
using FluentValidation;

namespace C6BankIntegration.Application.Validators;

/// <summary>Validador para criação de cobranças Pix.</summary>
public sealed class CreatePixChargeRequestValidator : AbstractValidator<CreatePixChargeRequest>
{
    /// <summary>Configura as regras de validação.</summary>
    public CreatePixChargeRequestValidator()
    {
        RuleFor(x => x.PixKey)
            .NotEmpty().WithMessage("A chave Pix é obrigatória.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor da cobrança deve ser maior que zero.");

        When(x => !string.IsNullOrEmpty(x.DebtorDocument), () =>
        {
            RuleFor(x => x.DebtorDocument!)
                .Must(doc => Cpf.IsValid(doc) || Cnpj.IsValid(doc))
                .WithMessage("O documento do devedor deve ser um CPF ou CNPJ válido.");
        });

        RuleFor(x => x.ExpirationSeconds)
            .GreaterThan(0).WithMessage("O tempo de expiração deve ser maior que zero.")
            .LessThanOrEqualTo(86400).WithMessage("O tempo de expiração deve ser no máximo 86400 segundos (24 horas).");

        When(x => x.DueDate.HasValue, () =>
        {
            RuleFor(x => x.DueDate!.Value)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("A data de vencimento deve ser maior ou igual a hoje.");
        });
    }
}
