namespace C6BankIntegration.Domain.Entities;

/// <summary>
/// Entidade base com propriedades comuns a todas as entidades do domínio.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>Identificador único da entidade.</summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <summary>Data e hora de criação (UTC).</summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>Data e hora da última atualização (UTC).</summary>
    public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>Indica se a entidade está ativa.</summary>
    public bool IsActive { get; protected set; } = true;

    /// <summary>Atualiza o timestamp de última modificação.</summary>
    protected void MarkAsUpdated() => UpdatedAt = DateTime.UtcNow;

    /// <summary>Desativa a entidade (soft delete).</summary>
    public virtual void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }
}
