namespace C6BankIntegration.Domain.Exceptions;

/// <summary>Exceção base para erros de domínio.</summary>
public class DomainException : Exception
{
    /// <summary>Código do erro para identificação programática.</summary>
    public string ErrorCode { get; }

    /// <summary>Inicializa uma nova DomainException.</summary>
    public DomainException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>Inicializa uma nova DomainException com exceção interna.</summary>
    public DomainException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
