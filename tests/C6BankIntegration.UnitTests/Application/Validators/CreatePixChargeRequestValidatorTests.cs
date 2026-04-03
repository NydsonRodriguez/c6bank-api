using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace C6BankIntegration.UnitTests.Application.Validators;

/// <summary>Testes unitários para o validador de criação de cobranças Pix.</summary>
public sealed class CreatePixChargeRequestValidatorTests
{
    private readonly CreatePixChargeRequestValidator _validator = new();

    private static CreatePixChargeRequest ValidRequest() => new()
    {
        PixKey = "chave@email.com",
        Amount = 100m,
        ExpirationSeconds = 3600
    };

    [Fact]
    public void Validate_WithValidRequest_ShouldPassValidation()
    {
        // Act
        var result = _validator.TestValidate(ValidRequest());

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyPixKey_ShouldHaveError()
    {
        // Arrange
        var request = ValidRequest() with { PixKey = "" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PixKey);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
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
    public void Validate_WithInvalidDebtorDocument_ShouldHaveError()
    {
        // Arrange
        var request = ValidRequest() with { DebtorDocument = "12345678900" };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DebtorDocument);
    }
}
