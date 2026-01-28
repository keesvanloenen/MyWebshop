using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.DAL.Configurations;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL;

public class MyWebshopDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }        // only requirement for TPH
    public DbSet<PhysicalProduct> PhysicalProducts { get; set; }
    public DbSet<DigitalProduct> DigitalProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    public MyWebshopDbContext(DbContextOptions<MyWebshopDbContext> options) : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyWebshop;ConnectRetryCount=0");
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);     // keep at top

        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
            }
        }

        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAuditable<AuditRecord>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.AuditRecord.CreatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.AuditRecord.LastModifiedAt = now;
            }
        }


        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
            }
        }

        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAuditable<AuditRecord>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.AuditRecord.CreatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.AuditRecord.LastModifiedAt = now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
