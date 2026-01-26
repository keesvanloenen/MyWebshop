using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.DAL.Configurations;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL;

public class MyWebshopDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

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
    }
}
