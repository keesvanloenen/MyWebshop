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
        //ShowCustomers(options);
        //ShowProducts(options);
        //ShowCustomersAndOrdersEagerLoading(options);
        //ShowCustomersAndOrdersExplicitLoading(options);
        OptimisticConcurrency(options);
    }

    private static void OptimisticConcurrency(DbContextOptions<WebshopContext> options)
    {
        int customerId = 1;

        using var context = new WebshopContext(options);

        // User 1
        var customer = context.Customers.Find(customerId)!;
        Console.WriteLine($"User 1 opgehaald, huidig CreditLimit = {customer.CreditLimit}");

        // User 2
        context.Customers
            .Where(c => c.Id == customerId)
            .ExecuteUpdate(setters => setters.SetProperty(c => c.CreditLimit, 10000m));
        Console.WriteLine("User 2 saved: Credit = 10000");

        try
        {
            customer.CreditLimit = 200m;
            context.SaveChanges();

            /*
            Vera:
            Hoe kan de exception gethrowed worden?
            Dankzij de Concurrency Token!

            Bovenstaande SaveChanges() voert immers onderstaande query uit:

            UPDATE Customers
            SET CreditLimit = 200
            WHERE Id = 1 AND RowVersion = 0x00000000000007D7

            Door de actie van User 2 heeft de RowVersion kolom een nieuwe waarde gekregen en 
            wordt er niets geupdate en een exception gethrowd.

            In de lab wordt geen database-specifieke RowVersion kolom gebruikt,
            maar een zelf aangewezen kolom CreditLimit. Het update-statement ziet er dan ongeveer zo uit:

            UPDATE Customers
            SET CreditLimit = 200
            WHERE Id = 1 AND CreditLimit = < oude_waarde >;     -- oude_waarde is hier 150
            */
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"💀 CONFLICT! {ex.Message}");

            //Console.WriteLine(ex.Entries.Single());

            foreach(var entry in ex.Entries)
            {
                if (entry.Entity is Customer conflictedCustomer)
                {
                    // Haal database values op
                    var dbValues = entry.GetDatabaseValues();

                    if (dbValues == null)
                    {
                        Console.WriteLine("De database values konden niet worden opgehaald gedurende het afhandelen van een concurrency conflict");
                        return;
                    }

                    //// DB WINS ('user 1 wint')
                    //entry.CurrentValues.SetValues(dbValues);
                    //Console.WriteLine("Changes discarded!");

                    // CLIENT WINS ('user 2 wint')
                    entry.OriginalValues.SetValues(dbValues);
                    context.SaveChanges();
                    Console.WriteLine("✔️ Client wins: 10000 opgeslagen");


                }
            }
        }
    }

    private static void ShowCustomersAndOrdersExplicitLoading(DbContextOptions<WebshopContext> options)
    {
        Console.Write("Welk customer id: ");
        var input = Console.ReadLine() ?? string.Empty;

        var customerId = int.Parse(input);

        using var context = new WebshopContext(options);

        Customer? customer = context.Customers.Find(customerId);

        if (customer == null) return;

        context.Entry(customer).Collection(c => c.Orders).Load();

        Console.WriteLine($"Orders for Customer {customerId}:");
        foreach (var order in customer.Orders)
        {
            Console.WriteLine(order.Id + " " + order.OrderDate);
        }




    }

    private static void ShowCustomersAndOrdersEagerLoading(DbContextOptions<WebshopContext> options)
    {
        using var context = new WebshopContext(options);

        var customers = context.Customers
            .Include(c => c.Orders
                            .OrderByDescending(o => o.OrderDate)
                            .Take(1)
             )
            .AsNoTracking()
            .ToQueryString();

        Console.WriteLine(customers);

        //foreach(var customer in customers)
        //{
        //    Console.WriteLine(customer.Name);

        //    foreach(var order in customer.Orders)
        //    {
        //        Console.WriteLine($"      {order.OrderDate}");
        //    }
        //}

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
