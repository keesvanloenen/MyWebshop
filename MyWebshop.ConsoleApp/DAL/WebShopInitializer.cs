using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL;

public class WebShopInitializer
{
	public static void Seed(DbContextOptions<WebshopContext> options)
	{
		using var context = new WebshopContext(options);

		var customer1 = new Customer() { Name = "Ab", CreditLimit = 2000, PhoneNumber = "0611111111"};
		var customer2 = new Customer() { Name = "Bo", CreditLimit = 2000, PhoneNumber = "0622222222"};
		var customer3 = new Customer() { Name = "Cas", CreditLimit = 1800, PhoneNumber = "0633333333"};
		var customer4 = new Customer() { Name = "Dik", CreditLimit = 1800, PhoneNumber = "0644444444"};
		var customer5 = new Customer() { Name = "Ed", CreditLimit = 1600, PhoneNumber = "0655555555"};
		var customer6 = new Customer() { Name = "Fe", CreditLimit = 1600, PhoneNumber = "0666666666"};
       
        context.Customers.AddRange([customer1, customer2, customer3, customer4, customer5, customer6]);  // Let the Change Tracker know: "Hey a new user should be added!"
        
       
        context.SaveChanges();            // Persist to the database

        customer1.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-4), TotalAmount = 450.00m });
        customer1.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-7), TotalAmount = 190.00m });
        customer2.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-1), TotalAmount = 27.50m });
        context.SaveChanges();

        var physicalProduct1 = new PhysicalProduct { Name = "Laptop", Price = 999.99m, Weight = 1.5m };
        var physicalProduct2 = new PhysicalProduct { Name = "Mouse", Price = 19.99m, Weight = 0.1m };
        var digitalProduct1 = new DigitalProduct { Name = "C# for Dummies", Price = 9.99m, FileSizeInMB = 5 };
        var digitalProduct2 = new DigitalProduct { Name = "LINQ Course", Price = 49.99m, FileSizeInMB = 1200 };

        context.Products.AddRange([physicalProduct1, physicalProduct2, digitalProduct1, digitalProduct2]);
        context.SaveChanges();


    }
}
