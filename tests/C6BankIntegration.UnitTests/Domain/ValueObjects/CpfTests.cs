using C6BankIntegration.Domain.Exceptions;
using C6BankIntegration.Domain.ValueObjects;
using FluentAssertions;

namespace C6BankIntegration.UnitTests.Domain.ValueObjects;

/// <summary>Testes unitários para o Value Object CPF.</summary>
public sealed class CpfTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    [InlineData("111.444.777-35")]
    public void Constructor_WithValidCpf_ShouldCreateSuccessfully(string cpf)
    {
        // Act
        var result = new Cpf(cpf);

        // Assert
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().HaveLength(11);
        result.Value.All(char.IsDigit).Should().BeTrue();
    }

    [Theory]
    [InlineData("000.000.000-00")]
    [InlineData("111.111.111-11")]
    [InlineData("123.456.789-00")]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("12345678")]
    public void Constructor_WithInvalidCpf_ShouldThrowInvalidDocumentException(string cpf)
    {
        // Act
        var act = () => new Cpf(cpf);

        // Assert
        act.Should().Throw<InvalidDocumentException>()
            .WithMessage("*CPF inválido*");
    }

    [Fact]
    public void IsValid_WithValidCpf_ShouldReturnTrue()
    {
        // Assert
        Cpf.IsValid("52998224725").Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithInvalidCpf_ShouldReturnFalse()
    {
        // Assert
        Cpf.IsValid("12345678900").Should().BeFalse();
    }

    [Fact]
    public void ToFormattedString_ShouldFormatCorrectly()
    {
        // Arrange
        var cpf = new Cpf("52998224725");

        // Act
        var formatted = cpf.ToFormattedString();

        // Assert
        formatted.Should().Be("529.982.247-25");
    }

    [Fact]
    public void Equals_WithSameCpf_ShouldReturnTrue()
    {
        // Arrange
        var cpf1 = new Cpf("52998224725");
        var cpf2 = new Cpf("529.982.247-25");

        // Assert
        cpf1.Should().Be(cpf2);
        (cpf1 == cpf2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentCpf_ShouldReturnFalse()
    {
        // Arrange
        var cpf1 = new Cpf("52998224725");
        var cpf2 = new Cpf("11144477735");

        // Assert
        cpf1.Should().NotBe(cpf2);
        (cpf1 != cpf2).Should().BeTrue();
    }
}
