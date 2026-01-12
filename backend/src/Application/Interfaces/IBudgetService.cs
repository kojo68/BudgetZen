using BudgetZen.Application.Dtos;

namespace BudgetZen.Application.Interfaces;

public interface IBudgetService
{
    Task<IReadOnlyList<BudgetDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<BudgetDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task<BudgetDto> CreateAsync(Guid userId, CreateBudgetRequest request, CancellationToken cancellationToken);
    Task<BudgetDto?> UpdateAsync(Guid userId, Guid id, UpdateBudgetRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken);
}
