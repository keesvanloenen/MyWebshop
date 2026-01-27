using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyWebshop.ConsoleApp.DAL;
using MyWebshop.ConsoleApp.Models;
using System.Text;

namespace MyWebshop.ConsoleApp;

public static class MyExtensions
{
    public static bool IsVowelName(this string name) =>
        char.ToLower(name[0]) is 'a' or 'e' or 'i' or 'o' or 'u';
}

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var options = BuildContextOptions();

        InitializeDb(options);
        DataSeed(options);

        // ShowCustomers(options);
        ShowCustomer(options);
        // ShowProducts(options);
        // ShowProductsMichelAlternative(options);
        // ShowOrders(options);
        // ShowCategories(options);
        ServerSideClientSide(options);
    }

    private static void ServerSideClientSide(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        // IsVowelName cannot be converted to SQL syntax, so
        // add .AsEnumerable() after the first .Where()
        
        //var customers = context.Customers
        //    .AsNoTracking()
        //    .Where(c => c.Name.Length > 2)
        //    .AsEnumerable()
        //    .Where(c => c.Name.IsVowelName())
        //    .Select(c => new { c.Id, NameUpper = c.Name.ToUpper() });

        // No difference:
        var customersA = context.Customers
            .AsNoTracking()
            .Where(c => c.Name.Length > 2 && c.Name.StartsWith('E'));

        var customersB = context.Customers
            .AsNoTracking()
            .Where(c => c.Name.Length > 2)
            .Where(c => c.Name.StartsWith('E'));


        Console.WriteLine(customersA.ToQueryString());
        Console.WriteLine(customersB.ToQueryString());

        //Console.WriteLine(customers.ToQueryString());

        //foreach (var customer in customers)
        //{
        //    Console.WriteLine($"{customer.Id}: {customer.NameUpper}");
        //}
    }

    //private static bool IsVowelName(string name)
    //{
    //    char firstLetter = char.ToLower(name[0]);
    //    return firstLetter is 'a' or 'e' or 'i' or 'o' or 'u';
    //}

    private static void ShowCustomer(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        // FirstOrDefault: found entity or null
        Customer? customer1 = context.Customers.FirstOrDefault(c => c.Id == 5);
        Console.WriteLine($"Found: {customer1?.Name ?? "nothing"}");

        // First: found entity or exception
        Customer customer2 = context.Customers.First(c => c.Id == 5);
        Console.WriteLine($"Found: {customer2.Name}");

        // SingleOrDefault: found entity or null (exception when more than 1)
        Customer? customer3 = context.Customers.SingleOrDefault(c => c.Name.Length == 6);
        Console.WriteLine($"Found: {customer3?.Name ?? "nothing"}");
        // Change into 2 leads to exception!

        // Single: exception when not found + exception when more than 1
        Customer customer4 = context.Customers.Single(c => c.Name.Length == 6);
        Console.WriteLine($"Found: {customer4.Name}");

        // Find: like FirstOrDefault, but with only an identifier!
        Customer? customer5 = context.Customers.Find(5);
        Console.WriteLine($"Found: {customer5?.Name ?? "nothing"}");
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
        var customer4 = new Customer { Name = "Dik" };
        var customer5 = new Customer { Name = "Eduard" };
        var customer6 = new Customer { Name = "Fe" };


        customer1.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-4), TotalAmount = 450.00m });
        customer1.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-7), TotalAmount = 190});
        customer2.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-1), TotalAmount = 27.50m });

        context.Customers.AddRange([customer1, customer2, customer3, customer4, customer5, customer6]);

        context.SaveChanges();

        var physicalProduct1 = new PhysicalProduct { Name = "Laptop", Price = 999.99m, Weight = 1.5m };
        var physicalProduct2 = new PhysicalProduct { Name = "Mouse", Price = 19.99m, Weight = 0.1m };
        var digitalProduct1 = new DigitalProduct { Name = "C# for Dummies", Price = 9.99m, FileSizeInMB = 5 };
        var digitalProduct2 = new DigitalProduct { Name = "LINQ Course", Price = 49.99m, FileSizeInMB = 1200 };

        context.Products.AddRange([physicalProduct1, physicalProduct2, digitalProduct1, digitalProduct2]);
        context.SaveChanges();

        var electronics = new Category { Name = "Electronics" };
        var software = new Category { Name = "Software" };
        var accessories = new Category { Name = "Accessories" };
        
        context.Categories.AddRange([electronics, software, accessories]);

        context.ProductCategories.AddRange([
            new ProductCategory { 
                Product = physicalProduct1, 
                Category = electronics, 
                AddedOn = DateTime.Now.AddDays(-10) 
            },
            new ProductCategory { 
                Product = physicalProduct2, 
                Category = electronics, 
                AddedOn = DateTime.Now.AddDays(-1) 
            },
            new ProductCategory { 
                Product = physicalProduct2, 
                Category = accessories, 
                AddedOn = DateTime.Now.AddDays(-22) 
            },
            new ProductCategory { 
                Product = digitalProduct1, 
                Category = software, 
                AddedOn = DateTime.Now.AddDays(-7) 
            },
            new ProductCategory { 
                Product = digitalProduct2, 
                Category = software, 
                AddedOn = DateTime.Now.AddDays(-5) 
            }
        ]);

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
    
    private static void ShowOrders(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        var orders = context.Orders.
            Include(o => o.Customer);

        foreach (var order in orders)
        {
            // If CustomerId is a shadow property you would have to use the special `Entry` API (more on day 3):
            // var customerId = context.Entry(order).Property<int>("CustomerId").CurrentValue;
            // Console.WriteLine($"[{order.Id}] {order.OrderDate} {order.TotalAmount} (CustomerId: {customerId}) - Customer: {order.Customer.Name}");

            Console.WriteLine($"[{order.Id}] {order.OrderDate} {order.TotalAmount, 7} (CustomerId: {order.CustomerId}) - Customer: {order.Customer.Name}");
        }

    }

    private static void ShowCategories(DbContextOptions<MyWebshopDbContext> options)
    {
        using var context = new MyWebshopDbContext(options);

        var categories = context.Categories
            .Include(c => c.ProductCategories)
            .ThenInclude(pc => pc.Product);

        foreach (var category in categories)
        {
            Console.WriteLine($"[{category.Id}] {category.Name}");

            foreach (var productCategory in category.ProductCategories)
            {
                var product = productCategory.Product;
                Console.WriteLine($"\t[{product.Id}] {product.Name} {product.Price:C}");
            }
        }
    }
}
