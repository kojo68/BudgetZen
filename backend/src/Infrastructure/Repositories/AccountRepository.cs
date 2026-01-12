using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;
using BudgetZen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetZen.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BudgetZenDbContext _dbContext;

    public AccountRepository(BudgetZenDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Account>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Account?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Accounts.FirstOrDefaultAsync(a => a.UserId == userId && a.Id == id, cancellationToken);
    }

    public Task AddAsync(Account account, CancellationToken cancellationToken)
    {
        return _dbContext.Accounts.AddAsync(account, cancellationToken).AsTask();
    }

    public void Remove(Account account)
    {
        _dbContext.Accounts.Remove(account);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
