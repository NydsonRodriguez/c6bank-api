using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Enums;
using C6BankIntegration.Domain.ValueObjects;
using FluentAssertions;

namespace C6BankIntegration.UnitTests.Domain.Entities;

/// <summary>Testes unitários para a entidade PixCharge.</summary>
public sealed class PixChargeTests
{
    [Fact]
    public void CreateImmediate_WithValidData_ShouldCreatePixCharge()
    {
        // Act
        var pixCharge = PixCharge.CreateImmediate(
            "12345678909",
            new Money(150m),
            3600);

        // Assert
        pixCharge.Id.Should().NotBeEmpty();
        pixCharge.Txid.Should().NotBeNullOrEmpty();
        pixCharge.Txid.Should().HaveLength(35);
        pixCharge.ChargeType.Should().Be(PixChargeType.Immediate);
        pixCharge.ExpirationSeconds.Should().Be(3600);
        pixCharge.Status.Should().Be("ATIVA");
        pixCharge.DueDate.Should().BeNull();
    }

    [Fact]
    public void CreateWithDueDate_WithValidData_ShouldCreatePixCharge()
    {
        // Arrange
        var dueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));

        // Act
        var pixCharge = PixCharge.CreateWithDueDate(
            "empresa@pix.com",
            new Money(500m),
            dueDate);

        // Assert
        pixCharge.ChargeType.Should().Be(PixChargeType.DueDate);
        pixCharge.DueDate.Should().Be(dueDate);
        pixCharge.ExpirationSeconds.Should().BeNull();
    }

    [Fact]
    public void CreateImmediate_WithDebtorInfo_ShouldSetDebtorData()
    {
        // Act
        var pixCharge = PixCharge.CreateImmediate(
            "chave@pix.com",
            new Money(100m),
            debtorDocument: new Document("52998224725"),
            debtorName: "João da Silva");

        // Assert
        pixCharge.DebtorDocument.Should().NotBeNull();
        pixCharge.DebtorDocument!.IsCpf.Should().BeTrue();
        pixCharge.DebtorName.Should().Be("João da Silva");
    }

    [Fact]
    public void SetExternalData_ShouldUpdateLocationAndQrCode()
    {
        // Arrange
        var pixCharge = PixCharge.CreateImmediate("chave@pix.com", new Money(100m));

        // Act
        pixCharge.SetExternalData(
            "https://pix.c6bank.com.br/qr/v2/abc123",
            "00020126580014br.gov.bcb.pix0136abc123",
            "ATIVA");

        // Assert
        pixCharge.Location.Should().NotBeNullOrEmpty();
        pixCharge.QrCodePayload.Should().NotBeNullOrEmpty();
        pixCharge.Status.Should().Be("ATIVA");
    }
}
