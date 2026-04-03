using Bogus;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace C6BankIntegration.UnitTests.Application.Validators;

/// <summary>Testes unitários para o validador de criação de boletos.</summary>
public sealed class CreateBoletoRequestValidatorTests
{
    private readonly CreateBoletoRequestValidator _validator = new();

    private static CreateBoletoRequest ValidRequest() => new()
    {
        Amount = 100m,
        DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
        PayerDocument = "52998224725",
        PayerName = "João da Silva",
        InterestRate = 1.0m,
        FineRate = 2.0m
    };

    [Fact]
    public void Validate_WithValidRequest_ShouldPassValidation()
    {
        // Act
        var result = _validator.TestValidate(ValidRequest());

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_WithInvalidAmount_ShouldHaveError(decimal amount)
    {
        // Arrange
        var request = ValidRequest() with { Amount = amount };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Validate_WithPastDueDate_ShouldHaveError()
    {
        // Arrange
        var request = ValidRequest() with
        {
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DueDate);
    }

    [Theory]
    [InlineData("12345678900")]  // CPF inválido
    [InlineData("00000000000000")]  // CNPJ inválido
    [InlineData("abc")]
    [InlineData("")]
    public void Validate_WithInvalidDocument_ShouldHaveError(string document)
    {
        // Arrange
        var request = ValidRequest() with { PayerDocument = document };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PayerDocument);
    }

    [Theory]
    [InlineData("52998224725")]  // CPF válido
    [InlineData("11222333000181")]  // CNPJ válido
    public void Validate_WithValidDocument_ShouldNotHaveError(string document)
    {
        // Arrange
        var request = ValidRequest() with { PayerDocument = document };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PayerDocument);
    }

    [Fact]
    public void Validate_WithEmptyPayerName_ShouldHaveError()
    {
        // Arrange
        var request = ValidRequest() with { PayerName = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PayerName);
    }
}
