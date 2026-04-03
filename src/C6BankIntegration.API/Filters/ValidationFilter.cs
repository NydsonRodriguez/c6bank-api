using C6BankIntegration.Application.DTOs.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace C6BankIntegration.API.Filters;

/// <summary>Filtro de validação automática usando FluentValidation.</summary>
public sealed class ValidationFilter<T> : IAsyncActionFilter where T : class
{
    private readonly IValidator<T> _validator;

    /// <summary>Inicializa o filtro com o validator correspondente.</summary>
    public ValidationFilter(IValidator<T> validator) => _validator = validator;

    /// <inheritdoc/>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var argument = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

        if (argument is not null)
        {
            var result = await _validator.ValidateAsync(argument);

            if (!result.IsValid)
            {
                var correlationId = context.HttpContext.Items["CorrelationId"]?.ToString();
                var details = result.ToDictionary();

                var readOnlyDetails = details.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value);

                var error = ApiErrorResponse.CreateValidation(readOnlyDetails, correlationId);
                context.Result = new UnprocessableEntityObjectResult(error);
                return;
            }
        }

        await next();
    }
}
