using C6BankIntegration.Domain.Exceptions;

namespace C6BankIntegration.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um CNPJ válido com validação dos dígitos verificadores.
/// </summary>
public sealed class Cnpj : IEquatable<Cnpj>
{
    private static readonly int[] FirstWeights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    private static readonly int[] SecondWeights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    /// <summary>CNPJ formatado (apenas dígitos).</summary>
    public string Value { get; }

    /// <summary>Inicializa e valida um CNPJ.</summary>
    public Cnpj(string value)
    {
        string digits = new(value.Where(char.IsDigit).ToArray());

        if (!IsValid(digits))
            throw new InvalidDocumentException($"CNPJ inválido: {value}", "INVALID_CNPJ");

        Value = digits;
    }

    /// <summary>Valida um CNPJ pelo algoritmo dos dígitos verificadores.</summary>
    public static bool IsValid(string cnpj)
    {
        string digits = new(cnpj.Where(char.IsDigit).ToArray());

        if (digits.Length != 14) return false;
        if (digits.Distinct().Count() == 1) return false;

        return ValidateDigit(digits, FirstWeights, 12) &&
               ValidateDigit(digits, SecondWeights, 13);
    }

    private static bool ValidateDigit(string digits, int[] weights, int position)
    {
        int sum = 0;
        for (int i = 0; i < position; i++)
            sum += int.Parse(digits[i].ToString()) * weights[i];

        int remainder = sum % 11;
        int expectedDigit = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(digits[position].ToString()) == expectedDigit;
    }

    /// <summary>Formata o CNPJ no padrão 00.000.000/0000-00.</summary>
    public string ToFormattedString() =>
        $"{Value[..2]}.{Value[2..5]}.{Value[5..8]}/{Value[8..12]}-{Value[12..]}";

    /// <inheritdoc/>
    public bool Equals(Cnpj? other) => other is not null && Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Cnpj);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => Value;
}
