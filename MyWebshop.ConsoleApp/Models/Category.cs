namespace MyWebshop.ConsoleApp.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation property for many-to-many
    // public ICollection<Product> Products { get; set; } = [];


    // Navigation property for many-to-many with explicit join table
    public List<ProductCategory> ProductCategories { get; set; } = [];
}
