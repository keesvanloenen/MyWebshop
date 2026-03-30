using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL;

public class WebShopInitializer
{
	public static void Seed(DbContextOptions<WebshopContext> options)
	{
		using var context = new WebshopContext(options);

		var customer1 = new Customer() { Name = "Romeo", CreditLimit = 150, PhoneNumber = "0612345678"};
		var customer2 = new Customer() { Name = "Mo", CreditLimit = 250, PhoneNumber = "0687654321"};

        context.Customers.Add(customer1);  // Let the Change Tracker know: "Hey a new user should be added!"
        context.Customers.Add(customer2);
        context.SaveChanges();            // Persist to the database

        customer1.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-4), TotalAmount = 450.00m });
        customer1.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-7), TotalAmount = 190.00m });
        customer2.Orders.Add(new Order { OrderDate = DateTime.Now.AddDays(-1), TotalAmount = 27.50m });
        context.SaveChanges();
    }
}
