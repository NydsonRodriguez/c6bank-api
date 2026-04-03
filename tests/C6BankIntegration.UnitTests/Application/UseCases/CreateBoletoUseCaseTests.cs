using AutoMapper;
using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Application.DTOs.Response;
using C6BankIntegration.Application.Mappings;
using C6BankIntegration.Application.UseCases.Boletos;
using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;
using FluentAssertions;
using Moq;

namespace C6BankIntegration.UnitTests.Application.UseCases;

/// <summary>Testes unitários para o caso de uso de criação de boleto.</summary>
public sealed class CreateBoletoUseCaseTests
{
    private readonly Mock<IBoletoRepository> _boletoRepositoryMock = new();
    private readonly Mock<IC6BankBoletoService> _boletoServiceMock = new();
    private readonly IMapper _mapper;
    private readonly CreateBoletoUseCase _useCase;

    public CreateBoletoUseCaseTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _useCase = new CreateBoletoUseCase(
            _boletoRepositoryMock.Object,
            _boletoServiceMock.Object,
            _mapper);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRequest_ShouldCreateBoleto()
    {
        // Arrange
        var request = new CreateBoletoRequest
        {
            Amount = 150m,
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(10)),
            PayerDocument = "52998224725",
            PayerName = "João Silva"
        };

        var serviceResult = new BoletoServiceResult("ext-001", "34191.09008 12345.678901", "03459012345678");

        _boletoServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<Boleto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceResult);

        _boletoRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Boleto>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Amount.Should().Be(150m);
        result.PayerName.Should().Be("João Silva");
        result.ExternalId.Should().Be("ext-001");

        _boletoServiceMock.Verify(s => s.CreateAsync(It.IsAny<Boleto>(), It.IsAny<CancellationToken>()), Times.Once);
        _boletoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Boleto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceThrows_ShouldPropagateException()
    {
        // Arrange
        var request = new CreateBoletoRequest
        {
            Amount = 100m,
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5)),
            PayerDocument = "52998224725",
            PayerName = "Maria Santos"
        };

        _boletoServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<Boleto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Serviço indisponível"));

        // Act
        var act = async () => await _useCase.ExecuteAsync(request);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
        _boletoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Boleto>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
