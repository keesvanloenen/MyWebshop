using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.DAL;
using MyWebshop.ConsoleApp.Models;

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
        using var context = new WebshopContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var user = new User() { Name = "Romeo", };

        context.Users.Add(user);        // Let the Change Tracker know: "Hey a new user should be added!"
        context.SaveChanges();          // Persist to the database

        var users = context.Users;

        foreach (var u in users)
        {
            Console.WriteLine($"{u.Id} - {u.Name}");
        }
    }
}
