using AutoMapper;
using C6BankIntegration.Application.Mappings;
using C6BankIntegration.Application.UseCases.Boletos;
using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace C6BankIntegration.UnitTests.Application.UseCases;

/// <summary>Testes unitários para o caso de uso de consulta de boleto.</summary>
public sealed class GetBoletoUseCaseTests
{
    private readonly Mock<IBoletoRepository> _repositoryMock = new();
    private readonly IMapper _mapper;
    private readonly GetBoletoUseCase _useCase;

    public GetBoletoUseCaseTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _useCase = new GetBoletoUseCase(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task ExecuteAsync_WithExistingId_ShouldReturnBoleto()
    {
        // Arrange
        var boleto = Boleto.Create(
            new Money(200m),
            DateOnly.FromDateTime(DateTime.Today.AddDays(15)),
            new Document("52998224725"),
            "Carlos Pereira");

        _repositoryMock
            .Setup(r => r.GetByIdAsync(boleto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(boleto);

        // Act
        var result = await _useCase.ExecuteAsync(boleto.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(boleto.Id);
        result.Amount.Should().Be(200m);
        result.PayerName.Should().Be("Carlos Pereira");
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistingId_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(nonExistingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Boleto?)null);

        // Act
        var act = async () => await _useCase.ExecuteAsync(nonExistingId);

        // Assert
        await act.Should().ThrowAsync<C6BankIntegration.Domain.Exceptions.BusinessRuleException>()
            .WithMessage($"*{nonExistingId}*");
    }
}
