using Shopway.Domain.Enums;
using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities.Parents;

//Table-per-type approach
public class Person : AggregateRoot
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }

    //DateOnly property needs a conversion to SQL Server DATE format
    public DateOnly? DateOfBirth { get; set; }

    public string ContactNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public int? AddressId { get; set; }

    //One to one relationship with User table (User, UserId)
    public virtual User? User { get; set; }
}