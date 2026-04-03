using C6BankIntegration.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;

namespace C6BankIntegration.IntegrationTests.Controllers;

/// <summary>Testes de integração para o controller de Pix.</summary>
public sealed class PixControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PixControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetImmediateCharge_WithNonExistingTxid_ShouldReturn404()
    {
        // Arrange
        var txid = "txid-inexistente-12345678901234567";

        // Act
        var response = await _client.GetAsync($"/api/v1/pix/cob/{txid}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ListImmediateCharges_ShouldReturn200()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/pix/cob");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListDueDateCharges_ShouldReturn200()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/pix/cobv");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
