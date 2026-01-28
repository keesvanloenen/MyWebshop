namespace MyWebshop.ConsoleApp.Models;

public interface IAuditable<T>
{
    public T AuditRecord { get; set; }
}
