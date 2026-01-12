using BudgetZen.Application.Dtos;
using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;

namespace BudgetZen.Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ICategoryRepository _categoryRepository;

    public BudgetService(IBudgetRepository budgetRepository, ICategoryRepository categoryRepository)
    {
        _budgetRepository = budgetRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<BudgetDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        var budgets = await _budgetRepository.GetAllAsync(userId, cancellationToken);
        return budgets.Select(Map).ToList();
    }

    public async Task<BudgetDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetByIdAsync(userId, id, cancellationToken);
        return budget is null ? null : Map(budget);
    }

    public async Task<BudgetDto> CreateAsync(Guid userId, CreateBudgetRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(userId, request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new InvalidOperationException("InvalidCategory");
        }

        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = request.CategoryId,
            MonthlyLimit = request.MonthlyLimit,
            Month = request.Month,
            Year = request.Year,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _budgetRepository.AddAsync(budget, cancellationToken);
        await _budgetRepository.SaveChangesAsync(cancellationToken);
        return Map(budget);
    }

    public async Task<BudgetDto?> UpdateAsync(Guid userId, Guid id, UpdateBudgetRequest request, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetByIdAsync(userId, id, cancellationToken);
        if (budget is null)
        {
            return null;
        }

        budget.MonthlyLimit = request.MonthlyLimit;
        await _budgetRepository.SaveChangesAsync(cancellationToken);
        return Map(budget);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetByIdAsync(userId, id, cancellationToken);
        if (budget is null)
        {
            return false;
        }

        _budgetRepository.Remove(budget);
        await _budgetRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static BudgetDto Map(Budget budget)
    {
        return new BudgetDto
        {
            Id = budget.Id,
            CategoryId = budget.CategoryId,
            MonthlyLimit = budget.MonthlyLimit,
            Month = budget.Month,
            Year = budget.Year,
            CreatedAtUtc = budget.CreatedAtUtc
        };
    }
}
