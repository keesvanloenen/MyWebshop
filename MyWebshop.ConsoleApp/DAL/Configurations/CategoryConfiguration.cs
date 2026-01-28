using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.OwnsOne(c => c.AuditRecord);

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();   
    }

}
