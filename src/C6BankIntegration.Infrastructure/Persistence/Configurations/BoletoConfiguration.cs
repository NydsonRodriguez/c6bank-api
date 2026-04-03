using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C6BankIntegration.Infrastructure.Persistence.Configurations;

/// <summary>Configuração Fluent API do EF Core para a entidade Boleto.</summary>
public sealed class BoletoConfiguration : IEntityTypeConfiguration<Boleto>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Boleto> builder)
    {
        builder.ToTable("Boletos");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id).ValueGeneratedNever();
        builder.Property(b => b.NossoNumero).IsRequired().HasMaxLength(30);
        builder.Property(b => b.ExternalId).HasMaxLength(100);
        builder.Property(b => b.PayerName).IsRequired().HasMaxLength(100);
        builder.Property(b => b.DigitableLine).HasMaxLength(60);
        builder.Property(b => b.Barcode).HasMaxLength(50);
        builder.Property(b => b.Description).HasMaxLength(500);
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(b => b.InterestRate).HasPrecision(5, 2);
        builder.Property(b => b.FineRate).HasPrecision(5, 2);
        builder.Property(b => b.DiscountAmount).HasPrecision(18, 2);
        builder.Property(b => b.CreatedAt).IsRequired();
        builder.Property(b => b.UpdatedAt).IsRequired();

        builder.OwnsOne(b => b.Amount, money =>
        {
            money.Property(m => m.Value).HasColumnName("Amount").HasPrecision(18, 2).IsRequired();
            money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });

        builder.OwnsOne(b => b.PayerDocument, doc =>
        {
            doc.Property(d => d.Value).HasColumnName("PayerDocument").HasMaxLength(14).IsRequired();
            doc.Property(d => d.Type).HasColumnName("PayerDocumentType").HasConversion<string>().HasMaxLength(4).IsRequired();
        });

        builder.HasIndex(b => b.ExternalId).IsUnique().HasFilter("[ExternalId] IS NOT NULL");
        builder.HasIndex(b => b.NossoNumero).IsUnique();
    }
}
