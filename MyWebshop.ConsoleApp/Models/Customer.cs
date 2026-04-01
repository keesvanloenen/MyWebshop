namespace MyWebshop.ConsoleApp.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public decimal? CreditLimit { get; set; }
    public DateTime? CreatedAt { get; set; }
    public byte[] RowVersion { get; set; } = null!;

    // Navigation Property 👇
    public List<Order> Orders { get; set; } = [];
}
