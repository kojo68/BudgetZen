using BudgetZen.Application.Dtos;
using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;

namespace BudgetZen.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResult<TransactionDto>> GetAllAsync(Guid userId, TransactionFilter filter, int page, int pageSize, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _transactionRepository.GetAllAsync(
            userId,
            filter.FromUtc,
            filter.ToUtc,
            filter.AccountId,
            filter.CategoryId,
            filter.Query,
            filter.Type.HasValue ? (int)filter.Type.Value : null,
            page,
            pageSize,
            cancellationToken);

        return new PagedResult<TransactionDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items.Select(Map).ToList()
        };
    }

    public async Task<TransactionDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(userId, id, cancellationToken);
        return transaction is null ? null : Map(transaction);
    }

    public async Task<TransactionDto> CreateAsync(Guid userId, CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        await EnsureOwnershipAsync(userId, request.AccountId, request.CategoryId, cancellationToken);

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccountId = request.AccountId,
            CategoryId = request.CategoryId,
            Type = request.Type,
            Amount = request.Amount,
            Description = request.Description.Trim(),
            OccurredAtUtc = ToUtc(request.OccurredAtLocal),
            CreatedAtUtc = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _transactionRepository.SaveChangesAsync(cancellationToken);
        return Map(transaction);
    }

    public async Task<TransactionDto?> UpdateAsync(Guid userId, Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(userId, id, cancellationToken);
        if (transaction is null)
        {
            return null;
        }

        await EnsureOwnershipAsync(userId, request.AccountId, request.CategoryId, cancellationToken);

        transaction.AccountId = request.AccountId;
        transaction.CategoryId = request.CategoryId;
        transaction.Type = request.Type;
        transaction.Amount = request.Amount;
        transaction.Description = request.Description.Trim();
        transaction.OccurredAtUtc = ToUtc(request.OccurredAtLocal);

        await _transactionRepository.SaveChangesAsync(cancellationToken);
        return Map(transaction);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(userId, id, cancellationToken);
        if (transaction is null)
        {
            return false;
        }

        _transactionRepository.Remove(transaction);
        await _transactionRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task EnsureOwnershipAsync(Guid userId, Guid accountId, Guid categoryId, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(userId, accountId, cancellationToken);
        var category = await _categoryRepository.GetByIdAsync(userId, categoryId, cancellationToken);
        if (account is null || category is null)
        {
            throw new InvalidOperationException("InvalidAccountOrCategory");
        }
    }

    private static DateTime ToUtc(DateTime local)
    {
        return DateTime.SpecifyKind(local, DateTimeKind.Local).ToUniversalTime();
    }

    private static TransactionDto Map(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            AccountId = transaction.AccountId,
            CategoryId = transaction.CategoryId,
            Type = transaction.Type,
            Amount = transaction.Amount,
            Description = transaction.Description,
            OccurredAtUtc = transaction.OccurredAtUtc,
            CreatedAtUtc = transaction.CreatedAtUtc
        };
    }
}
