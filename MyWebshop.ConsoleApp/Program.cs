using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyWebshop.ConsoleApp.DAL;
using MyWebshop.ConsoleApp.Models;
using System.Text;

namespace MyWebshop.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var options = BuildContextOptions();

        InitializeDb(options);
        DataSeed(options);
        
        ShowCustomers(options);
        ShowProducts(options);
        ShowProductsMichelAlternative(options);
    }


    private static DbContextOptions<MyWebshopDbContext> BuildContextOptions()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();

        return new DbContextOptionsBuilder<MyWebshopDbContext>()
            .UseSqlServer(config.GetConnectionString("DefaultConn"))
            .Options;
    }

    private static void InitializeDb(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    private static void DataSeed(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        var customer1 = new Customer { Name = "Ab" };
        var customer2 = new Customer { Name = "Bo" };
        var customer3 = new Customer { Name = "Cas" };

        context.Customers.AddRange([customer1, customer2, customer3]);
        context.SaveChanges();

        var physicalProduct1 = new PhysicalProduct { Name = "Laptop", Price = 999.99m, Weight = 1.5m };
        var physicalProduct2 = new PhysicalProduct { Name = "Mouse", Price = 19.99m, Weight = 0.1m };
        var digitalProduct1 = new DigitalProduct { Name = "C# for Dummies", Price = 9.99m, FileSizeInMB = 5 };
        var digitalProduct2 = new DigitalProduct { Name = "LINQ Course", Price = 49.99m, FileSizeInMB = 1200 };

        context.Products.AddRange([physicalProduct1, physicalProduct2, digitalProduct1, digitalProduct2]);
        context.SaveChanges();
    }

    private static void ShowCustomers(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        var customers = context.Customers;

        foreach (var customer in customers) 
        {
            Console.WriteLine($"[{customer.Id}] {customer.Name}");
        }
    }

    private static void ShowProducts(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        var products = context.Products.Where(p => p.Price > 100);

        foreach (var product in products)
        {
            Console.Write($"[{product.Id}] {product.Name} - {product.Price:C}");

            //var pp = product as PhysicalProduct;
            //if (pp is not null)
            //{
            //    Console.WriteLine($" | Weight = {pp.Weight}");
            //}
            //var dp = product as DigitalProduct;
            //if (dp is not null)
            //{
            //    Console.WriteLine($" | Size = {dp.FileSizeInMB}");
            //}

            if (product is PhysicalProduct pp)
                Console.WriteLine($" | Weight = {pp.Weight}");
            else if (product is DigitalProduct dp)
                Console.WriteLine($" | Size = {dp.FileSizeInMB}");
        }
    }

    private static void ShowProductsMichelAlternative(DbContextOptions<MyWebshopDbContext> options)
    {
        Console.WriteLine("\n=== Alternatief Michel ===");
        using var context = new MyWebshopDbContext(options);

        var physicalProducts = context.Products.OfType<PhysicalProduct>();
        var digitalProducts = context.Products.OfType<DigitalProduct>();

        Console.WriteLine("\nPhysical Products:");
        foreach (var p in physicalProducts)
        {
            Console.WriteLine($"[{p.Id}] {p.Name} - {p.Price:C} | Weight = {p.Weight}");
        }

        Console.WriteLine("\nDigital Products:");
        foreach (var d in digitalProducts)
        {
            Console.WriteLine($"[{d.Id}] {d.Name} - {d.Price:C} | Size = {d.FileSizeInMB} MB");
        }
    }
}
