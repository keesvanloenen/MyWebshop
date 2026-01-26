using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyWebshop.ConsoleApp.DAL;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<MyWebshopDbContext>()
            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyWebshop;ConnectRetryCount=0")
            .Options;

        InitializeDb(options);
        DataSeed(options);
        ShowCustomers(options);
    }

    private static void DataSeed(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        var customer1 = new Customer { Name = "Ab" };
        var customer2 = new Customer { Name = "Bo" };
        var customer3 = new Customer { Name = "Cas" };

        context.Customers.AddRange([customer1, customer2, customer3]);
        context.SaveChanges();
    }

    private static void InitializeDb(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    private static void ShowCustomers(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        var customers = context.Customers.Where(c => c.Name.EndsWith('s'));

        foreach (var customer in customers) 
        {
            Console.WriteLine($"[{customer.Id}] {customer.Name}");
        }
        
    }
}
