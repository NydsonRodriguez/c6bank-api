using C6BankIntegration.Application.UseCases.Boletos;
using C6BankIntegration.Application.UseCases.Pix;
using C6BankIntegration.Application.UseCases.Webhooks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace C6BankIntegration.Application;

/// <summary>Extensões para registro das dependências da camada Application.</summary>
public static class DependencyInjection
{
    /// <summary>Registra todos os serviços da camada Application no container de DI.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Boleto use cases
        services.AddScoped<CreateBoletoUseCase>();
        services.AddScoped<GetBoletoUseCase>();
        services.AddScoped<UpdateBoletoUseCase>();
        services.AddScoped<CancelBoletoUseCase>();
        services.AddScoped<ListBoletosUseCase>();

        // Pix use cases
        services.AddScoped<CreatePixImmediateChargeUseCase>();
        services.AddScoped<CreatePixDueDateChargeUseCase>();
        services.AddScoped<GetPixChargeUseCase>();
        services.AddScoped<ListPixChargesUseCase>();

        // Webhook use cases
        services.AddScoped<CreateWebhookUseCase>();
        services.AddScoped<GetWebhookUseCase>();
        services.AddScoped<DeleteWebhookUseCase>();

        return services;
    }
}
