using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C6BankIntegration.Infrastructure.Persistence.Configurations;

/// <summary>Configuração Fluent API do EF Core para a entidade Webhook.</summary>
public sealed class WebhookConfiguration : IEntityTypeConfiguration<Webhook>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Webhook> builder)
    {
        builder.ToTable("Webhooks");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id).ValueGeneratedNever();
        builder.Property(w => w.PixKey).IsRequired().HasMaxLength(77);
        builder.Property(w => w.WebhookUrl).IsRequired().HasMaxLength(500);
        builder.Property(w => w.CreatedAt).IsRequired();

        builder.Property(w => w.EventTypes)
            .HasConversion(
                v => string.Join(',', v.Select(e => e.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(e => Enum.Parse<WebhookEventType>(e))
                      .ToList()
                      .AsReadOnly())
            .HasMaxLength(200);

        builder.HasIndex(w => w.PixKey).IsUnique();
    }
}
