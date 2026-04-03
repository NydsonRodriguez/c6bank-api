namespace C6BankIntegration.Domain.Exceptions;

/// <summary>Exceção lançada quando um documento (CPF/CNPJ) é inválido.</summary>
public sealed class InvalidDocumentException : DomainException
{
    /// <summary>Inicializa uma nova InvalidDocumentException.</summary>
    public InvalidDocumentException(string message, string errorCode)
        : base(message, errorCode) { }
}
