using Microsoft.EntityFrameworkCore;
using MyWebshop.ConsoleApp.Models;
using System.Net.WebSockets;

namespace MyWebshop.ConsoleApp.DAL;

internal class WebshopContext : DbContext
{
    public DbSet<User> Users { get; set; }

    // MANIER 2:
    public WebshopContext(DbContextOptions<WebshopContext> options) : base(options)
    {
    }
    
    // MANIER 1:
    //protected override void OnConfiguring(DbContextOptionsBuilder builder)
    //{
    //    builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Webshop;ConnectRetryCount=0");
    //}


}
