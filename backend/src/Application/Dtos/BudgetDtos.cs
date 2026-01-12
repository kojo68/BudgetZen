namespace BudgetZen.Application.Dtos;

public class BudgetDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public decimal MonthlyLimit { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public class CreateBudgetRequest
{
    public Guid CategoryId { get; set; }
    public decimal MonthlyLimit { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class UpdateBudgetRequest
{
    public decimal MonthlyLimit { get; set; }
}
