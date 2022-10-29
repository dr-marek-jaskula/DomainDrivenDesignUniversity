using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Employee : Person
{
    private readonly List<WorkItem> _workItems = new();
    private readonly List<Employee> _subordinates = new();

    public DateOnly HireDate { get; private set; }

    //Many to many relationship with customers (rest is in Customer class)
    public List<Customer> Customers { get; private set; } = new();

    //One to many relationship with same table (ManagerId, Manager, Subordinates)
    public Guid? ManagerId { get; private set; }
    public Employee? Manager { get; private set; }

    public IReadOnlyCollection<Employee> Subordinates => _subordinates;

    //WorkItems relations
    public IReadOnlyCollection<WorkItem> WorkItems => _workItems;

    internal Employee
    (
        Guid id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email,
        Address? address,
        User? user,
        DateOnly hireDate
    )
        : base(id, firstName, lastName, gender, dateOfBirth, contactNumber, email, address, user)
    {
        HireDate = hireDate;
    }

    // Empty constructor in this case is required by EF Core
    private Employee()
    {
    }

    public static Employee Create
    (
        Guid id,
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOnly? dateOfBirth,
        PhoneNumber contactNumber,
        Email email,
        Address? address,
        User? user,
        DateOnly hireDate
    )
    {
        var employee = new Employee
        (
            id,
            firstName,
            lastName,
            gender,
            dateOfBirth,
            contactNumber,
            email,
            address,
            user,
            hireDate
        );

        employee.RaiseDomainEvent(new EmployeeRegisteredDomainEvent(Guid.NewGuid(), employee.Id));

        return employee;
    }

    public Result<WorkItem> AssignWrokItem(WorkItem workItem)
    {
        if (_workItems.Contains(workItem))
        {
            return Result.Failure<WorkItem>(DomainErrors.WorkItemError.AlreadyAssigned);
        }

        _workItems.Add(workItem);

        return workItem;
    }
}