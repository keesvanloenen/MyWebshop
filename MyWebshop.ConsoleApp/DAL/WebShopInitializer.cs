using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL;

public class WebShopInitializer
{
	public static void Seed(DbContextOptions<WebshopContext> options)
	{
		using var context = new WebshopContext(options);

		var customer = new Customer() { Name = "Romeo", CreditLimit = 150, PhoneNumber = "0612345678"};

        context.Customers.Add(customer);  // Let the Change Tracker know: "Hey a new user should be added!"
        context.SaveChanges();            // Persist to the database

        //var physicalProduct1 = new PhysicalProduct { Name = "Laptop", Price = 999.99m, Weight = 1.5m };
        //var physicalProduct2 = new PhysicalProduct { Name = "Mouse", Price = 19.99m, Weight = 0.1m };
        //var digitalProduct1 = new DigitalProduct { Name = "C# for Dummies", Price = 9.99m, FileSizeInMB = 5 };
        //var digitalProduct2 = new DigitalProduct { Name = "LINQ Course", Price = 49.99m, FileSizeInMB = 1200 };

        //context.Products.AddRange([physicalProduct1, physicalProduct2, digitalProduct1, digitalProduct2]);
        //context.SaveChanges();
    }
}
