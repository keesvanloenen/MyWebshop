using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Overwrite discriminator info when choosing Tabler Per Hierarchy (TPH)
        //builder
        //    .HasDiscriminator<string>("ProductType")
        //    .HasValue<PhysicalProduct>("Physical")
        //    .HasValue<DigitalProduct>("Digital");

        //builder.UseTptMappingStrategy();

        builder.UseTpcMappingStrategy();
    }
}
