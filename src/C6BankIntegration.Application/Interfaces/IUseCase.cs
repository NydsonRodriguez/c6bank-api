namespace C6BankIntegration.Application.Interfaces;

/// <summary>Contrato base para todos os casos de uso.</summary>
/// <typeparam name="TRequest">Tipo da requisição de entrada.</typeparam>
/// <typeparam name="TResponse">Tipo da resposta de saída.</typeparam>
public interface IUseCase<TRequest, TResponse>
{
    /// <summary>Executa o caso de uso.</summary>
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}
