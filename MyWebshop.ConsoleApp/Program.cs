using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.DAL;

namespace MyWebshop.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<WebshopContext>()
            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Webshop;ConnectRetryCount=0")
            .Options;

        Initialize(options);
    }

    private static void Initialize(DbContextOptions<WebshopContext> options)
    {
        CreateDB(options);
        WebShopInitializer.Seed(options);
        ShowCustomers(options);
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
}
