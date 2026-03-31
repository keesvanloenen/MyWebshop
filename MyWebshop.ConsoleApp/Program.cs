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
        // ShowCustomers(options);
        ShowProducts(options);
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

        var customers = context.Customers.Include(c => c.Orders);

        foreach (var c in customers)
        {
            Console.WriteLine($"{c.Id} - {c.Name}, Credit Limit: { c.CreditLimit }, Phone: { c.PhoneNumber }");

            foreach(var order in c.Orders)
            {
                Console.WriteLine($"\t - {order.Id} - {order.OrderDate} ({order.TotalAmount})");        
            }
        }
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
}
