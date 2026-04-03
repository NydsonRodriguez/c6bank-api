using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.ValueObjects;
using FluentAssertions;

namespace C6BankIntegration.UnitTests.Domain.ValueObjects;

/// <summary>Testes unitários para o Value Object CNPJ.</summary>
public sealed class CnpjTests
{
    [Theory]
    [InlineData("11.222.333/0001-81")]
    [InlineData("11222333000181")]
    public void Constructor_WithValidCnpj_ShouldCreateSuccessfully(string cnpj)
    {
        // Act
        var result = new Cnpj(cnpj);

        // Assert
        result.Value.Should().HaveLength(14);
        result.Value.All(char.IsDigit).Should().BeTrue();
    }

    [Theory]
    [InlineData("00.000.000/0000-00")]
    [InlineData("11.111.111/1111-11")]
    [InlineData("12345678000000")]
    [InlineData("invalid")]
    public void Constructor_WithInvalidCnpj_ShouldThrowInvalidDocumentException(string cnpj)
    {
        // Act
        var act = () => new Cnpj(cnpj);

        // Assert
        act.Should().Throw<InvalidDocumentException>();
    }

    [Fact]
    public void ToFormattedString_ShouldFormatCorrectly()
    {
        // Arrange
        var cnpj = new Cnpj("11222333000181");

        // Act
        var formatted = cnpj.ToFormattedString();

        // Assert
        formatted.Should().Be("11.222.333/0001-81");
    }
}
