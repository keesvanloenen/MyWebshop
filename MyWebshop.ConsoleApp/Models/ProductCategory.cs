namespace MyWebshop.ConsoleApp.Models;

public class ProductCategory
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

 
    // Additional field: this is why we need an explicit join table
    public DateTime AddedOn { get; set; }
}