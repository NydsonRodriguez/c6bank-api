namespace C6BankIntegration.API.Extensions;

/// <summary>Extensões de configuração de serviços da API.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Configura opções de serialização JSON padrão.</summary>
    public static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        });
        return services;
    }
}
