using MyWebshop.ConsoleApp.DAL;
using MyWebshop.ConsoleApp.Models;

namespace MyWebshop.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        using (var context = new WebshopContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new User() { Name = "Romeo", };

            context.Users.Add(user);        // Let the Change Tracker know: "Hey a new user should be added!"
            context.SaveChanges();          // Persist to the database

            var users = context.Users;

            foreach(var u in users)
            {
                Console.WriteLine($"{u.Id} - {u.Name}");
            }
        }
    }
}
