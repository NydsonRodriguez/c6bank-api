using C6BankIntegration.Domain.Exceptions;

namespace C6BankIntegration.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um valor monetário sempre positivo com precisão de 2 casas decimais.
/// </summary>
public sealed class Money : IEquatable<Money>
{
    /// <summary>Valor monetário.</summary>
    public decimal Value { get; }

    /// <summary>Código da moeda (padrão: BRL).</summary>
    public string Currency { get; }

    /// <summary>Inicializa um novo valor monetário.</summary>
    public Money(decimal value, string currency = "BRL")
    {
        if (value <= 0)
            throw new DomainException("O valor monetário deve ser maior que zero.", "INVALID_AMOUNT");

        Value = Math.Round(value, 2);
        Currency = currency;
    }

    /// <summary>Cria um valor monetário em reais.</summary>
    public static Money FromBrl(decimal value) => new(value, "BRL");

    /// <inheritdoc/>
    public bool Equals(Money? other) =>
        other is not null && Value == other.Value && Currency == other.Currency;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Money);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Value, Currency);

    /// <inheritdoc/>
    public override string ToString() => $"{Currency} {Value:F2}";

    /// <summary>Operador de igualdade.</summary>
    public static bool operator ==(Money? left, Money? right) =>
        left?.Equals(right) ?? right is null;

    /// <summary>Operador de desigualdade.</summary>
    public static bool operator !=(Money? left, Money? right) => !(left == right);
}
