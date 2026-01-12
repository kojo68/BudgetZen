using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;
using BudgetZen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetZen.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly BudgetZenDbContext _dbContext;

    public TransactionRepository(BudgetZenDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetAllAsync(
        Guid userId,
        DateTime? fromUtc,
        DateTime? toUtc,
        Guid? accountId,
        Guid? categoryId,
        string? query,
        int? type,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var baseQuery = _dbContext.Transactions.AsNoTracking().Where(t => t.UserId == userId);

        if (fromUtc.HasValue)
        {
            baseQuery = baseQuery.Where(t => t.OccurredAtUtc >= fromUtc.Value);
        }

        if (toUtc.HasValue)
        {
            baseQuery = baseQuery.Where(t => t.OccurredAtUtc <= toUtc.Value);
        }

        if (accountId.HasValue)
        {
            baseQuery = baseQuery.Where(t => t.AccountId == accountId.Value);
        }

        if (categoryId.HasValue)
        {
            baseQuery = baseQuery.Where(t => t.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            baseQuery = baseQuery.Where(t => t.Description.Contains(query));
        }

        if (type.HasValue)
        {
            baseQuery = baseQuery.Where(t => (int)t.Type == type.Value);
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderByDescending(t => t.OccurredAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<Transaction?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Transactions.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id, cancellationToken);
    }

    public Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        return _dbContext.Transactions.AddAsync(transaction, cancellationToken).AsTask();
    }

    public void Remove(Transaction transaction)
    {
        _dbContext.Transactions.Remove(transaction);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetMonthAsync(Guid userId, int year, int month, CancellationToken cancellationToken)
    {
        var start = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = start.AddMonths(1).AddTicks(-1);

        return await _dbContext.Transactions
            .AsNoTracking()
            .Where(t => t.UserId == userId && t.OccurredAtUtc >= start && t.OccurredAtUtc <= end)
            .ToListAsync(cancellationToken);
    }
}
