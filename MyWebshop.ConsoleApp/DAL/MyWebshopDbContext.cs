using Microsoft.EntityFrameworkCore;
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
}
