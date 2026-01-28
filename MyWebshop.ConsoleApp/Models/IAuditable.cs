namespace MyWebshop.ConsoleApp.Models;
// <https://medium.com/@bananicabananica/audit-automation-with-ef-core-2f629fb77523>

public interface IAuditable<T>
{
    public T AuditRecord { get; set; }
}
