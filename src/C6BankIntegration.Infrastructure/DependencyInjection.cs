using C6BankIntegration.Domain.Interfaces.Repositories;
using C6BankIntegration.Domain.Interfaces.Services;
using C6BankIntegration.Infrastructure.Configuration;
using C6BankIntegration.Infrastructure.ExternalServices.C6Bank;
using C6BankIntegration.Infrastructure.Persistence;
using C6BankIntegration.Infrastructure.Persistence.Repositories;
using C6BankIntegration.Infrastructure.Resilience;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace C6BankIntegration.Infrastructure;

/// <summary>Extensões para registro das dependências da camada Infrastructure.</summary>
public static class DependencyInjection
{
    /// <summary>Registra todos os serviços da camada Infrastructure no container de DI.</summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<C6BankSettings>(configuration.GetSection(C6BankSettings.SectionName));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(maxRetryCount: 3)));

        services.AddMemoryCache();

        RegisterHttpClient(services);
        RegisterRepositories(services);
        RegisterServices(services);

        return services;
    }

    private static void RegisterHttpClient(IServiceCollection services)
    {
        services.AddHttpClient<C6BankHttpClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<C6BankSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
        })
        .AddPolicyHandler((sp, _) =>
        {
            var logger = sp.GetRequiredService<ILogger<C6BankHttpClient>>();
            return PollyPolicies.GetCombinedPolicy(logger);
        });
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IBoletoRepository, BoletoRepository>();
        services.AddScoped<IPixChargeRepository, PixChargeRepository>();
        services.AddScoped<IWebhookRepository, WebhookRepository>();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IC6BankAuthService, C6BankAuthService>();
        services.AddScoped<IC6BankBoletoService, C6BankBoletoService>();
        services.AddScoped<IC6BankPixService, C6BankPixService>();
        services.AddScoped<IC6BankWebhookService, C6BankWebhookService>();
    }
}
