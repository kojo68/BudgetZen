namespace BudgetZen.Domain.Entities;

public class Budget
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal MonthlyLimit { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public User? User { get; set; }
    public Category? Category { get; set; }
}
