using BudgetZen.Application.Dtos;

namespace BudgetZen.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetMonthlySummaryAsync(Guid userId, int year, int month, CancellationToken cancellationToken);
}
