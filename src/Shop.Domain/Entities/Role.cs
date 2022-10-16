using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public class Role : Entity
{
    public string Name { get; set; } = string.Empty;
    public virtual List<User> Users { get; set; } = new();
}