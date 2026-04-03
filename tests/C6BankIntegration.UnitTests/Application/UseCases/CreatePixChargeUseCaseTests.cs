using AutoMapper;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.Mappings;
using C6BankIntegration.Application.UseCases.Pix;
using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;
using FluentAssertions;
using Moq;

namespace C6BankIntegration.UnitTests.Application.UseCases;

/// <summary>Testes unitários para o caso de uso de criação de cobrança Pix.</summary>
public sealed class CreatePixChargeUseCaseTests
{
    private readonly Mock<IPixChargeRepository> _repositoryMock = new();
    private readonly Mock<IC6BankPixService> _pixServiceMock = new();
    private readonly IMapper _mapper;
    private readonly CreatePixImmediateChargeUseCase _useCase;

    public CreatePixChargeUseCaseTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _useCase = new CreatePixImmediateChargeUseCase(
            _repositoryMock.Object,
            _pixServiceMock.Object,
            _mapper);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRequest_ShouldCreatePixCharge()
    {
        // Arrange
        var request = new CreatePixChargeRequest
        {
            PixKey = "chave@pix.com",
            Amount = 200m,
            ExpirationSeconds = 3600,
            DebtorName = "Ana Costa"
        };

        var serviceResult = new PixServiceResult(
            "https://pix.c6bank.com.br/qr/v2/abc",
            "00020126580014br.gov.bcb.pix",
            "ATIVA");

        _pixServiceMock
            .Setup(s => s.CreateImmediateChargeAsync(It.IsAny<PixCharge>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceResult);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<PixCharge>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Amount.Should().Be(200m);
        result.Txid.Should().NotBeNullOrEmpty();
        result.Status.Should().Be("ATIVA");

        _pixServiceMock.Verify(s => s.CreateImmediateChargeAsync(It.IsAny<PixCharge>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<PixCharge>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
