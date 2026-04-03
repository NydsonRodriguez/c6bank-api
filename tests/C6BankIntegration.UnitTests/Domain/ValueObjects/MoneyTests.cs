using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.ValueObjects;
using FluentAssertions;

namespace C6BankIntegration.UnitTests.Domain.ValueObjects;

/// <summary>Testes unitários para o Value Object Money.</summary>
public sealed class MoneyTests
{
    [Theory]
    [InlineData(100.00)]
    [InlineData(0.01)]
    [InlineData(999999.99)]
    public void Constructor_WithPositiveValue_ShouldCreateSuccessfully(decimal value)
    {
        // Act
        var money = new Money(value);

        // Assert
        money.Value.Should().BeGreaterThan(0);
        money.Currency.Should().Be("BRL");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void Constructor_WithZeroOrNegativeValue_ShouldThrowDomainException(decimal value)
    {
        // Act
        var act = () => new Money(value);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*maior que zero*");
    }

    [Fact]
    public void Constructor_WithMoreThanTwoDecimalPlaces_ShouldRound()
    {
        // Arrange & Act
        var money = new Money(100.555m);

        // Assert
        money.Value.Should().Be(100.56m);
    }

    [Fact]
    public void FromBrl_ShouldCreateWithBrlCurrency()
    {
        // Act
        var money = Money.FromBrl(100m);

        // Assert
        money.Currency.Should().Be("BRL");
        money.Value.Should().Be(100m);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var money1 = new Money(100m);
        var money2 = new Money(100m);

        // Assert
        money1.Should().Be(money2);
        (money1 == money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var money1 = new Money(100m);
        var money2 = new Money(200m);

        // Assert
        money1.Should().NotBe(money2);
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldFormatCorrectly()
    {
        // Arrange
        var money = new Money(1234.56m);

        // Assert
        money.ToString().Should().Be("BRL 1234.56");
    }
}
