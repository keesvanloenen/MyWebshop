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

    }
}
