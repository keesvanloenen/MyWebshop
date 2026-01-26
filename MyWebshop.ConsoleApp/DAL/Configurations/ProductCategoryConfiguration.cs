using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        // Composite primary key
        builder.HasKey(pc => new { pc.CategoryId, pc.ProductId });

        // Relationship: Product -> ProductCategory
        builder.HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(p => p.ProductId);

        // Relationship: Category -> ProductCategory
        builder.HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(c => c.CategoryId);

        // Additional field configuration
        builder.Property(pc => pc.AddedOn)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();
    }
}
