using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;
using BudgetZen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetZen.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly BudgetZenDbContext _dbContext;

    public BudgetRepository(BudgetZenDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Budget>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Budgets
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Year)
            .ThenByDescending(b => b.Month)
            .ToListAsync(cancellationToken);
    }

    public Task<Budget?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Budgets.FirstOrDefaultAsync(b => b.UserId == userId && b.Id == id, cancellationToken);
    }

    public Task AddAsync(Budget budget, CancellationToken cancellationToken)
    {
        return _dbContext.Budgets.AddAsync(budget, cancellationToken).AsTask();
    }

    public void Remove(Budget budget)
    {
        _dbContext.Budgets.Remove(budget);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
