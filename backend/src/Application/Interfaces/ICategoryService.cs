using BudgetZen.Application.Dtos;

namespace BudgetZen.Application.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<CategoryDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken);
    Task<CategoryDto> CreateAsync(Guid userId, CreateCategoryRequest request, CancellationToken cancellationToken);
    Task<CategoryDto?> UpdateAsync(Guid userId, Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken);
}
