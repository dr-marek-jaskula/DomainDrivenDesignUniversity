using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Customer : Person
{
    private Customer(
        Guid id, 
        FirstName firstName, 
        LastName lastName, 
        Gender gender, 
        DateOnly? dateOfBirth, 
        PhoneNumber contactNumber, 
        Email email, 
        Address? address, 
        User? user, 
        Rank rank) 
        : base(id, firstName, lastName, gender, dateOfBirth, contactNumber, email, address, user)
    {
        Rank = rank;
    }

    public Rank Rank { get; private set; }
    public List<Order> Orders { get; set; } = new();

    public static Customer Create(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email,
        Address? address,
        User? user,
        Rank rank)
    {
        var customer = new Customer(
            id,
            firstName,
            lastName,
            gender,
            dateOfBirth,
            contactNumber,
            email,
            address,
            user,
            rank);

        customer.RaiseDomainEvent(new CustomerRegisteredDomainEvent(Guid.NewGuid(), customer.Id));

        return customer;
    }
}