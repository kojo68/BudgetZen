using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;
using BudgetZen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetZen.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BudgetZenDbContext _dbContext;

    public UserRepository(BudgetZenDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        return _dbContext.Users.AddAsync(user, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
