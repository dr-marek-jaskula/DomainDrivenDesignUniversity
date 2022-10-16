using Shopway.Domain.Primitives;

namespace Shopway.Domain.Entities;

public sealed class Salary_Transfer : Entity
{
    public DateTime Date { get; set; }
    public bool IsDiscretionaryBonus { get; set; }
    public bool IsIncentiveBonus { get; set; }
    public bool IsTaskBonus { get; set; }
    public virtual Salary? Salary { get; set; }
    public int? SalaryId { get; set; }
}