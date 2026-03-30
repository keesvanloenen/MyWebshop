using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebshop.ConsoleApp.Models;
using System.Reflection.Emit;

namespace MyWebshop.ConsoleApp.DAL.Configurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(c => c.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        builder
            .Property(c => c.CreditLimit)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);

        builder
            .Property(c => c.CreatedAt)
            .HasColumnName("datetime2(0)")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();
    }
}

