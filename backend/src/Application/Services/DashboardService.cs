using System.Globalization;
using BudgetZen.Application.Dtos;
using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Enums;

namespace BudgetZen.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public DashboardService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<DashboardSummaryDto> GetMonthlySummaryAsync(Guid userId, int year, int month, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetMonthAsync(userId, year, month, cancellationToken);
        var categories = await _categoryRepository.GetAllAsync(userId, cancellationToken);
        var categoryLookup = categories.ToDictionary(c => c.Id, c => c.Name);

        var totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
        var totalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

        var byCategory = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => t.CategoryId)
            .Select(group => new CategorySpendDto
            {
                CategoryId = group.Key,
                CategoryName = categoryLookup.TryGetValue(group.Key, out var name) ? name : "Inconnu",
                Total = group.Sum(t => t.Amount)
            })
            .OrderByDescending(item => item.Total)
            .ToList();

        var calendar = CultureInfo.InvariantCulture.Calendar;
        var byWeek = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => calendar.GetWeekOfYear(t.OccurredAtUtc, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
            .Select(group => new WeeklySpendDto
            {
                Week = group.Key,
                Total = group.Sum(t => t.Amount)
            })
            .OrderBy(item => item.Week)
            .ToList();

        return new DashboardSummaryDto
        {
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            Net = totalIncome - totalExpense,
            ByCategory = byCategory,
            ByWeek = byWeek
        };
    }
}
