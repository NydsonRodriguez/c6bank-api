using C6BankIntegration.Domain.Exceptions;

namespace C6BankIntegration.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um CPF válido com validação dos dígitos verificadores.
/// </summary>
public sealed class Cpf : IEquatable<Cpf>
{
    /// <summary>CPF formatado (apenas dígitos).</summary>
    public string Value { get; }

    /// <summary>Inicializa e valida um CPF.</summary>
    public Cpf(string value)
    {
        string digits = new(value.Where(char.IsDigit).ToArray());

        if (!IsValid(digits))
            throw new InvalidDocumentException($"CPF inválido: {value}", "INVALID_CPF");

        Value = digits;
    }

    /// <summary>Valida um CPF pelo algoritmo dos dígitos verificadores.</summary>
    public static bool IsValid(string cpf)
    {
        string digits = new(cpf.Where(char.IsDigit).ToArray());

        if (digits.Length != 11) return false;
        if (digits.Distinct().Count() == 1) return false;

        return ValidateDigit(digits, 9) && ValidateDigit(digits, 10);
    }

    private static bool ValidateDigit(string digits, int position)
    {
        int sum = 0;
        for (int i = 0; i < position; i++)
            sum += int.Parse(digits[i].ToString()) * (position + 1 - i);

        int remainder = sum % 11;
        int expectedDigit = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(digits[position].ToString()) == expectedDigit;
    }

    /// <summary>Formata o CPF no padrão 000.000.000-00.</summary>
    public string ToFormattedString() =>
        $"{Value[..3]}.{Value[3..6]}.{Value[6..9]}-{Value[9..]}";

    /// <inheritdoc/>
    public bool Equals(Cpf? other) => other is not null && Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Cpf);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => Value;
}
