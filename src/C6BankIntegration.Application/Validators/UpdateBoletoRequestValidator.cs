using C6BankIntegration.Application.DTOs.Request;
using FluentValidation;

namespace C6BankIntegration.Application.Validators;

/// <summary>Validador para atualização de boletos.</summary>
public sealed class UpdateBoletoRequestValidator : AbstractValidator<UpdateBoletoRequest>
{
    /// <summary>Configura as regras de validação.</summary>
    public UpdateBoletoRequestValidator()
    {
        When(x => x.DueDate.HasValue, () =>
        {
            RuleFor(x => x.DueDate!.Value)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("A nova data de vencimento deve ser maior ou igual a hoje.");
        });

        When(x => x.Amount.HasValue, () =>
        {
            RuleFor(x => x.Amount!.Value)
                .GreaterThan(0).WithMessage("O novo valor do boleto deve ser maior que zero.");
        });

        When(x => x.InterestRate.HasValue, () =>
        {
            RuleFor(x => x.InterestRate!.Value)
                .InclusiveBetween(0, 100).WithMessage("A taxa de juros deve estar entre 0 e 100%.");
        });

        When(x => x.FineRate.HasValue, () =>
        {
            RuleFor(x => x.FineRate!.Value)
                .InclusiveBetween(0, 100).WithMessage("A multa deve estar entre 0 e 100%.");
        });

        When(x => x.DiscountAmount.HasValue, () =>
        {
            RuleFor(x => x.DiscountAmount!.Value)
                .GreaterThanOrEqualTo(0).WithMessage("O desconto não pode ser negativo.");
        });
    }
}
