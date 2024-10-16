using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");
        
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
        builder.Property(c => c.Position).HasColumnName("position")
            .IsRequired().HasDefaultValue("");
        builder.Property(c => c.Department).HasColumnName("department")
            .IsRequired().HasDefaultValue("");
        builder.Property(c => c.Salary).HasColumnName("salary")
            .IsRequired().HasDefaultValue(0);
        builder.Property(c => c.Contract).HasColumnName("contract")
            .IsRequired().HasDefaultValue("Контракт сотрудника по умолчанию");
        builder.Property(c => c.Age).HasColumnName("age").HasDefaultValue(0);
        
            
        builder.HasKey(c => c.Id);
    }
}