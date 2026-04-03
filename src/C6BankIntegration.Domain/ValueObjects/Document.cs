using C6BankIntegration.Domain.Exceptions;

namespace C6BankIntegration.Domain.ValueObjects;

/// <summary>
/// Value Object polimórfico que representa um documento (CPF ou CNPJ).
/// </summary>
public sealed class Document : IEquatable<Document>
{
    /// <summary>Número do documento (apenas dígitos).</summary>
    public string Value { get; }

    /// <summary>Tipo do documento.</summary>
    public DocumentType Type { get; }

    /// <summary>Inicializa um Document a partir de um CPF ou CNPJ.</summary>
    public Document(string value)
    {
        string digits = new(value.Where(char.IsDigit).ToArray());

        if (digits.Length == 11 && Cpf.IsValid(digits))
        {
            Value = digits;
            Type = DocumentType.Cpf;
        }
        else if (digits.Length == 14 && Cnpj.IsValid(digits))
        {
            Value = digits;
            Type = DocumentType.Cnpj;
        }
        else
        {
            throw new InvalidDocumentException($"Documento inválido: {value}", "INVALID_DOCUMENT");
        }
    }

    /// <summary>Cria um Document a partir de um CPF.</summary>
    public static Document FromCpf(string cpf) => new(cpf);

    /// <summary>Cria um Document a partir de um CNPJ.</summary>
    public static Document FromCnpj(string cnpj) => new(cnpj);

    /// <summary>Indica se o documento é CPF.</summary>
    public bool IsCpf => Type == DocumentType.Cpf;

    /// <summary>Indica se o documento é CNPJ.</summary>
    public bool IsCnpj => Type == DocumentType.Cnpj;

    /// <inheritdoc/>
    public bool Equals(Document? other) => other is not null && Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Document);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => Value;
}

/// <summary>Tipo de documento.</summary>
public enum DocumentType
{
    /// <summary>Cadastro de Pessoa Física.</summary>
    Cpf,
    /// <summary>Cadastro Nacional de Pessoa Jurídica.</summary>
    Cnpj
}
