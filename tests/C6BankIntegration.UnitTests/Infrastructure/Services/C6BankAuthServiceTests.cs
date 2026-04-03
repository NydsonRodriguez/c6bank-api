using C6BankIntegration.Infrastructure.Configuration;
using C6BankIntegration.Infrastructure.ExternalServices.C6Bank;
using C6BankIntegration.Infrastructure.ExternalServices.C6Bank.Models;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;

namespace C6BankIntegration.UnitTests.Infrastructure.Services;

/// <summary>Testes unitários para o serviço de autenticação C6 Bank.</summary>
public sealed class C6BankAuthServiceTests
{
    private readonly Mock<C6BankHttpClient> _httpClientMock;
    private readonly IMemoryCache _memoryCache;
    private readonly Mock<ILogger<C6BankAuthService>> _loggerMock;
    private readonly C6BankAuthService _authService;

    public C6BankAuthServiceTests()
    {
        _httpClientMock = new Mock<C6BankHttpClient>(
            MockBehavior.Loose,
            new HttpClient(),
            Options.Create(new C6BankSettings { ClientId = "x", ClientSecret = "y" }),
            Mock.Of<ILogger<C6BankHttpClient>>());

        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _loggerMock = new Mock<ILogger<C6BankAuthService>>();

        var settings = Options.Create(new C6BankSettings
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret"
        });

        _authService = new C6BankAuthService(
            _httpClientMock.Object,
            settings,
            _memoryCache,
            _loggerMock.Object);
    }

    [Fact]
    public void InvalidateToken_ShouldRemoveTokenFromCache()
    {
        // Arrange - coloca um token no cache manualmente
        _memoryCache.Set("C6Bank_AccessToken", "token-cached");

        // Act
        _authService.InvalidateToken();

        // Assert
        _memoryCache.TryGetValue("C6Bank_AccessToken", out string? _).Should().BeFalse();
    }
}
