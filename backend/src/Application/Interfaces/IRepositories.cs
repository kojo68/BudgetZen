using BudgetZen.Domain.Entities;

namespace BudgetZen.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IAccountRepository
{
    Task<IReadOnlyList<Account>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<Account?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task AddAsync(Account account, CancellationToken cancellationToken);
    void Remove(Account account);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task AddAsync(Category category, CancellationToken cancellationToken);
    void Remove(Category category);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public interface ITransactionRepository
{
    Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetAllAsync(
        Guid userId,
        DateTime? fromUtc,
        DateTime? toUtc,
        Guid? accountId,
        Guid? categoryId,
        string? query,
        int? type,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
    Task<Transaction?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    void Remove(Transaction transaction);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Transaction>> GetMonthAsync(Guid userId, int year, int month, CancellationToken cancellationToken);
}

public interface IBudgetRepository
{
    Task<IReadOnlyList<Budget>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<Budget?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task AddAsync(Budget budget, CancellationToken cancellationToken);
    void Remove(Budget budget);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user, DateTime expiresAtUtc);
}

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
