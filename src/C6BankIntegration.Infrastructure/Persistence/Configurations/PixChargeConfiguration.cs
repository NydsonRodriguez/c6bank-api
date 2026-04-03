using C6BankIntegration.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace C6BankIntegration.Infrastructure.Persistence.Configurations;

/// <summary>Configuração Fluent API do EF Core para a entidade PixCharge.</summary>
public sealed class PixChargeConfiguration : IEntityTypeConfiguration<PixCharge>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PixCharge> builder)
    {
        builder.ToTable("PixCharges");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Txid).IsRequired().HasMaxLength(35);
        builder.Property(p => p.PixKey).IsRequired().HasMaxLength(77);
        builder.Property(p => p.Status).IsRequired().HasMaxLength(20);
        builder.Property(p => p.DebtorName).HasMaxLength(100);
        builder.Property(p => p.Location).HasMaxLength(500);
        builder.Property(p => p.QrCodePayload).HasMaxLength(2000);
        builder.Property(p => p.AdditionalInfo).HasMaxLength(500);
        builder.Property(p => p.ChargeType).HasConversion<string>().HasMaxLength(20);
        builder.Property(p => p.CreatedAt).IsRequired();

        builder.OwnsOne(p => p.Amount, money =>
        {
            money.Property(m => m.Value).HasColumnName("Amount").HasPrecision(18, 2).IsRequired();
            money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });

        builder.OwnsOne(p => p.DebtorDocument, doc =>
        {
            doc.Property(d => d.Value).HasColumnName("DebtorDocument").HasMaxLength(14);
            doc.Property(d => d.Type).HasColumnName("DebtorDocumentType").HasConversion<string>().HasMaxLength(4);
        });

        builder.HasIndex(p => p.Txid).IsUnique();
    }
}
