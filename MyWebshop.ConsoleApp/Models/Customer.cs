namespace MyWebshop.ConsoleApp.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PhoneNumber { get; set; }    // ? = optional
    public decimal CreditLimit { get; set; }
    public DateTime CreatedAt { get; set; }
}
