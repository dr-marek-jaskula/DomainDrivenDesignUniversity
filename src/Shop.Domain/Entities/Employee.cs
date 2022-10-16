using Shopway.Domain.Entities.Parents;

namespace Shopway.Domain.Entities;

public class Employee : Person
{
    //Properties that defines the database relations should be mark as virtual
    public DateTime HireDate { get; set; }

    //One to one relationship with Salary table (Salary, SalaryId)
    public virtual Salary? Salary { get; set; }

    public int? SalaryId { get; set; }

    //One to many relationship with Shop table (Shop, ShopId)
    public virtual Shop? Shop { get; set; }

    public int? ShopId { get; set; }

    //Many to many relationship with customers (rest is in Customer class)
    public virtual List<Customer> Customers { get; set; } = new();

    //Many to many relationship with Reviews (rest is in Reviews class)
    public virtual List<Review> Reviews { get; set; } = new();

    //One to many relationship with same table (ManagerId, Manager, Subordinates)
    public virtual int? ManagerId { get; set; }

    public virtual Employee? Manager { get; set; }
    public virtual List<Employee>? Subordinates { get; set; } = new();

    //WorkItems relations
    public virtual WorkTask? CurrentTask { get; set; }

    public virtual Project? Project { get; set; }
}