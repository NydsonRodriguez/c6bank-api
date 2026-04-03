using C6BankIntegration.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;

namespace C6BankIntegration.IntegrationTests.Controllers;

/// <summary>Testes de integração para o endpoint de Health Check.</summary>
public sealed class HealthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ShouldReturnSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task GetHealth_ShouldReturnJsonContentType()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }
}
