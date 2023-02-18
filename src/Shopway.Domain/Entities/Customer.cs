using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.EntityIds;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Customer : Person
{
    private readonly List<Order> _orders = new();

    private Customer(
        PersonId id,
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

    // Empty constructor in this case is required by EF Core
    private Customer()
    {
    }

    public Rank Rank { get; private set; }
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    public static Customer Create(
        PersonId id,
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

        customer.RaiseDomainEvent(CustomerRegisteredDomainEvent.New(customer.Id));

        return customer;
    }
}