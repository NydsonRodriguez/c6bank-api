using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Enums;
using C6BankIntegration.Domain.ValueObjects;
using FluentAssertions;

namespace C6BankIntegration.UnitTests.Domain.Entities;

/// <summary>Testes unitários para a entidade Boleto.</summary>
public sealed class BoletoTests
{
    private static Boleto CreateValidBoleto() => Boleto.Create(
        new Money(100m),
        DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
        new Document("52998224725"),
        "João Silva");

    [Fact]
    public void Create_WithValidData_ShouldCreateBoleto()
    {
        // Act
        var boleto = CreateValidBoleto();

        // Assert
        boleto.Id.Should().NotBeEmpty();
        boleto.Amount.Value.Should().Be(100m);
        boleto.Status.Should().Be(BoletoStatus.Pending);
        boleto.IsActive.Should().BeTrue();
        boleto.NossoNumero.Should().NotBeNullOrEmpty();
        boleto.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void SetExternalData_ShouldUpdateBoletoWithExternalInfo()
    {
        // Arrange
        var boleto = CreateValidBoleto();

        // Act
        boleto.SetExternalData("ext-123", "34191.09008 12345.678901 23456.789012 1 92340000010000", "03459012345678901234567890");

        // Assert
        boleto.ExternalId.Should().Be("ext-123");
        boleto.DigitableLine.Should().NotBeNullOrEmpty();
        boleto.Barcode.Should().NotBeNullOrEmpty();
        boleto.Status.Should().Be(BoletoStatus.Active);
    }

    [Fact]
    public void Cancel_ShouldChangeBoletoStatusToCancelled()
    {
        // Arrange
        var boleto = CreateValidBoleto();

        // Act
        boleto.Cancel();

        // Assert
        boleto.Status.Should().Be(BoletoStatus.Cancelled);
        boleto.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void UpdateStatus_ShouldChangeStatus()
    {
        // Arrange
        var boleto = CreateValidBoleto();

        // Act
        boleto.UpdateStatus(BoletoStatus.Paid);

        // Assert
        boleto.Status.Should().Be(BoletoStatus.Paid);
    }

    [Fact]
    public void Create_WithOptionalParams_ShouldSetCorrectly()
    {
        // Act
        var boleto = Boleto.Create(
            new Money(250m),
            DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
            new Document("11222333000181"),
            "Empresa LTDA",
            interestRate: 1.5m,
            fineRate: 2.0m,
            discountAmount: 10m,
            description: "Parcela 1/12");

        // Assert
        boleto.InterestRate.Should().Be(1.5m);
        boleto.FineRate.Should().Be(2.0m);
        boleto.DiscountAmount.Should().Be(10m);
        boleto.Description.Should().Be("Parcela 1/12");
        boleto.PayerDocument.IsCnpj.Should().BeTrue();
    }
}
