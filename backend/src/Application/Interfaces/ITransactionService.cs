using BudgetZen.Application.Dtos;

namespace BudgetZen.Application.Interfaces;

public interface ITransactionService
{
    Task<PagedResult<TransactionDto>> GetAllAsync(Guid userId, TransactionFilter filter, int page, int pageSize, CancellationToken cancellationToken);
    Task<TransactionDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task<TransactionDto> CreateAsync(Guid userId, CreateTransactionRequest request, CancellationToken cancellationToken);
    Task<TransactionDto?> UpdateAsync(Guid userId, Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken);
}
