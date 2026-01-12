using BudgetZen.Application.Dtos;

namespace BudgetZen.Application.Interfaces;

public interface IAccountService
{
    Task<IReadOnlyList<AccountDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<AccountDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task<AccountDto> CreateAsync(Guid userId, CreateAccountRequest request, CancellationToken cancellationToken);
    Task<AccountDto?> UpdateAsync(Guid userId, Guid id, UpdateAccountRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken);
}
