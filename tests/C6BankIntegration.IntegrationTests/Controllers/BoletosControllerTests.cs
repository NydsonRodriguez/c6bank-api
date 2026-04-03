using C6BankIntegration.Application.DTOs.Request;
using C6BankIntegration.Domain.Interfaces.Services;
using C6BankIntegration.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace C6BankIntegration.IntegrationTests.Controllers;

/// <summary>Testes de integração para o controller de Boletos.</summary>
public sealed class BoletosControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BoletosControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_WithNonExistingId_ShouldReturn404()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/v1/boletos/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task List_ShouldReturn200WithEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/boletos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        (content.Contains("[]") || content.Contains("\"id\"")).Should().BeTrue();
    }

    [Fact]
    public async Task Create_WithInvalidRequest_ShouldReturn422Or400()
    {
        // Arrange
        var invalidRequest = new CreateBoletoRequest
        {
            Amount = -1,
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-10)),
            PayerDocument = "00000000000",
            PayerName = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/boletos", invalidRequest);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.UnprocessableEntity);
    }
}
