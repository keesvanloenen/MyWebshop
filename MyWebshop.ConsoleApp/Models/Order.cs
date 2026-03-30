namespace MyWebshop.ConsoleApp.Models;

public class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    // Navigation Property 👇
    public Customer Customer { get; set; } = null!;
}
