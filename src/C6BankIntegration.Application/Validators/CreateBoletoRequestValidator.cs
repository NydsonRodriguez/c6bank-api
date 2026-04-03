using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Domain.ValueObjects;
using FluentValidation;

namespace C6BankIntegration.Application.Validators;

/// <summary>Validador para a criação de boletos.</summary>
public sealed class CreateBoletoRequestValidator : AbstractValidator<CreateBoletoRequest>
{
    /// <summary>Configura as regras de validação.</summary>
    public CreateBoletoRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor do boleto deve ser maior que zero.");

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("A data de vencimento deve ser maior ou igual a hoje.");

        RuleFor(x => x.PayerDocument)
            .NotEmpty().WithMessage("O documento do pagador é obrigatório.")
            .Must(doc => Cpf.IsValid(doc) || Cnpj.IsValid(doc))
            .WithMessage("O documento do pagador deve ser um CPF ou CNPJ válido.");

        RuleFor(x => x.PayerName)
            .NotEmpty().WithMessage("O nome do pagador é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do pagador deve ter no máximo 100 caracteres.");

        RuleFor(x => x.InterestRate)
            .InclusiveBetween(0, 100).WithMessage("A taxa de juros deve estar entre 0 e 100%.");

        RuleFor(x => x.FineRate)
            .InclusiveBetween(0, 100).WithMessage("A multa deve estar entre 0 e 100%.");

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0).WithMessage("O desconto não pode ser negativo.");
    }
}
