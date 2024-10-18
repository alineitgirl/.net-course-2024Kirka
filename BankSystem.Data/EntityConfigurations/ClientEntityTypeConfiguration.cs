using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");
        
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.FirstName).HasColumnName("first_name")
            .IsRequired().HasMaxLength(50);
        builder.Property(c => c.LastName).HasColumnName("last_name")
            .IsRequired().HasMaxLength(50);
        builder.Property(c => c.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
        builder.Property(c => c.PhoneNumber).HasColumnName("phone_number")
            .IsRequired();
        builder.HasIndex(c => c.PhoneNumber).IsUnique();
        builder.Property(c => c.Passport).HasColumnName("passport")
            .IsRequired().HasMaxLength(20);
        builder.HasIndex(c => c.Passport).IsUnique();
        builder.Property(c => c.Adress).HasMaxLength(100);
        builder.Property(c => c.Age).HasColumnName("age");
            
        builder.HasKey(c => c.Id);
        builder.HasMany( c => c.Accounts)
            .WithOne(a => a.Client)
            .HasForeignKey(a => a.ClientId);

    }
}