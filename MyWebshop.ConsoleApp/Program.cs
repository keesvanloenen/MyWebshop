using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.DAL;
using MyWebshop.ConsoleApp.Models;
using System.Text;

namespace MyWebshop.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var options = new DbContextOptionsBuilder<WebshopContext>()
            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Webshop;ConnectRetryCount=0;Integrated Security=true;")
            .Options;

        Initialize(options);
    }

    private static void Initialize(DbContextOptions<WebshopContext> options)
    {
        CreateDB(options);
        WebShopInitializer.Seed(options);
        ShowCustomers(options);
        //ShowProducts(options);
    }

    private static void CreateDB(DbContextOptions<WebshopContext> options)
    {
        using var context = new WebshopContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    private static void ShowCustomers(DbContextOptions<WebshopContext> options)
    {
        using var context = new WebshopContext(options);

        var customers = context.Customers;

        // First(): found entity or exception
        // FirstOrDefault(): found entity or null
        Customer? customer = customers.FirstOrDefault(c => c.Name.StartsWith("Bot"));
        //if (customer == null)
        //{
        //    return;
        //}

        Console.WriteLine(customer?.Name ?? "n/a");
        // Single(): exception when not found & exception when more than 1
        // SingleOrDefault(): found entity or exception when more than 1
        Customer? customerB = customers.Find(3);

        Console.WriteLine(customerB?.Name);

        // ------------------------------------------------------------------------------

        //var klanten = context.Customers.ToList().Where(c => IsVowelName(c.Name));

        string deQuery = context.Customers
            .Where(k => k.Name.Length < 3)
            .OrderByDescending(k => k.Name)
            .ThenByDescending(k => k.PhoneNumber)
            .ToQueryString();

        Console.WriteLine(deQuery);
   }

    private static void ShowProducts(DbContextOptions<WebshopContext> options)
    {
        using var context = new WebshopContext(options);

        var products = context.Products;

        foreach (var product in products)
        {
            Console.Write($"[{product.Id}] {product.Name} - € {product.Price:F2}");


            if (product is PhysicalProduct pp)
                Console.WriteLine($", WEIGHT: {pp.Weight} kg");
            else if (product is DigitalProduct dp)
                Console.WriteLine($", SIZE: {dp.FileSizeInMB} MB");
        }
    }

    private static bool IsVowelName(string name)
    {
        return name.Contains('a') || name.Contains('e');
    }
}
