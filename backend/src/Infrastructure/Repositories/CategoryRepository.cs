using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;
using BudgetZen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetZen.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly BudgetZenDbContext _dbContext;

    public CategoryRepository(BudgetZenDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Category?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Categories.FirstOrDefaultAsync(c => c.UserId == userId && c.Id == id, cancellationToken);
    }

    public Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        return _dbContext.Categories.AddAsync(category, cancellationToken).AsTask();
    }

    public void Remove(Category category)
    {
        _dbContext.Categories.Remove(category);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
