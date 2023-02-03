using Shopway.Domain.BaseTypes;
using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Entities.Parents;

//Table-per-type approach
public class Person : AggregateRoot<PersonId>, IAuditableEntity
{
    protected Person
    (
        PersonId id, 
        FirstName firstName, 
        LastName lastName, 
        Gender gender, 
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email, 
        Address? address, 
        User? user
    )
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        PhoneNumber = contactNumber;
        Email = email;
        Address = address;
        User = user;
    }

    // Empty constructor in this case is required by EF Core
    protected Person()
    {
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Gender Gender { get; private set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }

    //DateOnly property needs a conversion to SQL Server DATE format
    public DateOnly? DateOfBirth { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }
    public Email Email { get; private set; }
    public Address? Address { get; private set; }

    //One to one relationship with User table (User, UserId)
    public User? User { get; private set; }

    public static Person Create(
        PersonId id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email,
        Address? address,
        User? user)
    {
        var person = new Person(
            id,
            firstName,
            lastName,
            gender,
            dateOfBirth,
            contactNumber,
            email,
            address,
            user);

        return person;
    }
}