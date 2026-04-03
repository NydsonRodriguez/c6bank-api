using C6BankIntegration.Domain.Entities;
using C6BankIntegration.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace C6BankIntegration.Infrastructure.Persistence;

/// <summary>Contexto de banco de dados da aplicação usando Entity Framework Core.</summary>
public sealed class AppDbContext : DbContext
{
    /// <summary>Boletos registrados.</summary>
    public DbSet<Boleto> Boletos => Set<Boleto>();

    /// <summary>Cobranças Pix.</summary>
    public DbSet<PixCharge> PixCharges => Set<PixCharge>();

    /// <summary>Webhooks registrados.</summary>
    public DbSet<Webhook> Webhooks => Set<Webhook>();

    /// <summary>Inicializa o contexto com as opções fornecidas.</summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
