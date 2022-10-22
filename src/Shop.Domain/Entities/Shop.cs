using Shopway.Domain.Primitives;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Shop : AggregateRoot
{
    public Shop(Guid id, string name, string? description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public Address? Address { get; private set; }
    public int? AddressId { get; private set; }
    public List<Employee> Employees { get; private set; } = new();
    public List<Order> Orders { get; private set; } = new();
    public List<Product> Products { get; private set; } = new();
}