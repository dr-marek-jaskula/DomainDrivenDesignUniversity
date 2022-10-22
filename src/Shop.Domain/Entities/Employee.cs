using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Employee : Person
{
    public DateOnly HireDate { get; private set; }

    //One to many relationship with Shop table (Shop, ShopId)
    public Shop? Shop { get; private set; }
    public Guid? ShopId { get; private set; }

    //Many to many relationship with customers (rest is in Customer class)
    public List<Customer> Customers { get; private set; } = new();

    //Many to many relationship with Reviews (rest is in Reviews class)
    public List<Review> Reviews { get; private set; } = new();

    //One to many relationship with same table (ManagerId, Manager, Subordinates)
    public Guid? ManagerId { get; private set; }

    public Employee? Manager { get; private set; }
    public List<Employee>? Subordinates { get; private set; } = new();

    //WorkItems relations
    public WorkTask? CurrentTask { get; private set; }
    public Project? Project { get; private set; }

    private Employee(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email,
        Address? address,
        User? user,
        DateOnly hireDate)
    : base(id, firstName, lastName, gender, dateOfBirth, contactNumber, email, address, user)
    {
        HireDate = hireDate;
    }

    // Empty constructor in this case is required by EF Core
    private Employee()
    {
    }

    public static Employee Create(
        Guid id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email,
        Address? address,
        User? user,
        DateOnly hireDate)
    {
        var employee = new Employee(
            id,
            firstName,
            lastName,
            gender,
            dateOfBirth,
            contactNumber,
            email,
            address,
            user,
            hireDate);

        employee.RaiseDomainEvent(new EmployeeRegisteredDomainEvent(Guid.NewGuid(), employee.Id));

        return employee;
    }
}