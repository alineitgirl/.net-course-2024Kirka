using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");
        
        builder.Property(a => a.CurrencyName).HasColumnName("currency_name")
            .HasMaxLength(10)
            .IsRequired()
            .HasDefaultValue("RUP");
        builder.Property(a => a.Amount).HasColumnName("amount")
            .IsRequired().HasDefaultValue(0);
        
        builder.HasKey(x => x.Id);
        builder.HasOne(a => a.Client)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.ClientId);
    }
}