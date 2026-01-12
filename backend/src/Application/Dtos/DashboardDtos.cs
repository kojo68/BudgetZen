namespace BudgetZen.Application.Dtos;

public class DashboardSummaryDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Net { get; set; }
    public IReadOnlyList<CategorySpendDto> ByCategory { get; set; } = Array.Empty<CategorySpendDto>();
    public IReadOnlyList<WeeklySpendDto> ByWeek { get; set; } = Array.Empty<WeeklySpendDto>();
}

public class CategorySpendDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

public class WeeklySpendDto
{
    public int Week { get; set; }
    public decimal Total { get; set; }
}
