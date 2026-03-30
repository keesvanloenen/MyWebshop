using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL;

public class WebshopContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    // MANIER 2: (de officiële manier, o.a. nodig bij migrations, unit tests etc.)
    public WebshopContext(DbContextOptions<WebshopContext> options) : base(options)
    {
    }

    // MANIER 1: (goed voor een snelle demo)
    //protected override void OnConfiguring(DbContextOptionsBuilder builder)
    //{
    //    builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Webshop;ConnectRetryCount=0");
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Customer>()
            .Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Customer>()
            .Property(c => c.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        modelBuilder.Entity<Customer>()
            .Property(c => c.CreditLimit)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);

        modelBuilder.Entity<Customer>()
            .Property(c => c.CreatedAt)
            .HasColumnName("datetime2(0)")
            .HasDefaultValueSql("SYSDATETIME()")
            .IsRequired();
    }    
}
