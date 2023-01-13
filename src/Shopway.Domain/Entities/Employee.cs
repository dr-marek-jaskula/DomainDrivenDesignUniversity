using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.Results;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.ValueObjects;
using static Shopway.Domain.Errors.DomainErrors;

namespace Shopway.Domain.Entities;

public sealed class Employee : Person
{
    private readonly List<WorkItem> _workItems = new();
    private readonly List<Employee> _subordinates = new();

    internal Employee(
        PersonId id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email,
        Address? address,
        User? user,
        DateTimeOffset hireDate)
        : base(id, firstName, lastName, gender, dateOfBirth, contactNumber, email, address, user)
    {
        HireDate = hireDate;
    }

    // Empty constructor in this case is required by EF Core
    private Employee()
    {
    }

    public DateTimeOffset HireDate { get; private set; }

    //Many to many relationship with customers (rest is in Customer class)
    public List<Person> Customers { get; private set; } = new();

    //One to many relationship with same table (ManagerId, Manager, Subordinates)
    public PersonId? ManagerId { get; private set; }
    public Employee? Manager { get; private set; }

    public IReadOnlyCollection<Employee> Subordinates => _subordinates.AsReadOnly();

    //WorkItems relations
    public IReadOnlyCollection<WorkItem> WorkItems => _workItems.AsReadOnly();

    public static Employee Create(
        PersonId id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email,
        Address? address,
        User? user,
        DateTimeOffset hireDate)
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

        employee.RaiseDomainEvent(EmployeeRegisteredDomainEvent.New(employee.Id));

        return employee;
    }

    public Result<WorkItem> AssignWrokItem(WorkItem workItem)
    {
        if (_workItems.Contains(workItem))
        {
            return Result.Failure<WorkItem>(WorkItemError.AlreadyAssigned);
        }

        _workItems.Add(workItem);

        return workItem;
    }
}