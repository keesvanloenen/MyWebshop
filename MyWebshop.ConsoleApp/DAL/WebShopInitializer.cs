using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp.DAL;

public class WebShopInitializer
{
	public static void Seed(DbContextOptions<WebshopContext> options)
	{
		using var context = new WebshopContext(options);

		var user = new User() { Name = "Romeo", };

        context.Users.Add(user);        // Let the Change Tracker know: "Hey a new user should be added!"
        context.SaveChanges();          // Persist to the database

    }
}
