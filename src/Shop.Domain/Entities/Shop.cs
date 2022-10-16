using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public class Shop : AggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public virtual Address? Address { get; set; }
    public int? AddressId { get; set; }
    public virtual List<Employee> Employees { get; set; } = new();
    public virtual List<Order> Orders { get; set; } = new();
    public virtual List<Product> Products { get; set; } = new();
}