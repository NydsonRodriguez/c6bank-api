using Microsoft.OpenApi.Models;
using System.Reflection;

namespace C6BankIntegration.API.Extensions;

/// <summary>Extensões de configuração do Swagger/OpenAPI.</summary>
public static class SwaggerExtensions
{
    /// <summary>Registra e configura o Swagger com documentação completa.</summary>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "C6 Bank Integration API",
                Version = "v1",
                Description = """
                    API REST de integração com o Banco C6 (código 336).

                    Funcionalidades:
                    - **Boletos**: criação, consulta, atualização e cancelamento
                    - **Pix**: cobranças imediatas e com vencimento
                    - **Webhooks**: registro e gerenciamento de callbacks Pix

                    Autenticação: OAuth2 client_credentials (configurado internamente via C6 Bank API)
                    """,
                Contact = new OpenApiContact
                {
                    Name = "Equipe de Integração",
                    Email = "integracao@empresa.com.br"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT"
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);
            c.EnableAnnotations();
        });

        return services;
    }
}
