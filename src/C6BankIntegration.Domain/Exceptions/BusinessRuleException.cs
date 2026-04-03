namespace C6BankIntegration.Domain.Exceptions;

/// <summary>Exceção lançada quando uma regra de negócio é violada.</summary>
public sealed class BusinessRuleException : DomainException
{
    /// <summary>Inicializa uma nova BusinessRuleException.</summary>
    public BusinessRuleException(string message, string errorCode)
        : base(message, errorCode) { }
}
