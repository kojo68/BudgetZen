using BudgetZen.Application.Dtos;
using BudgetZen.Application.Interfaces;
using BudgetZen.Domain.Entities;

namespace BudgetZen.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(userId, cancellationToken);
        return categories.Select(Map).ToList();
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(userId, id, cancellationToken);
        return category is null ? null : Map(category);
    }

    public async Task<CategoryDto> CreateAsync(Guid userId, CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.Name.Trim(),
            IsIncome = request.IsIncome,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
        return Map(category);
    }

    public async Task<CategoryDto?> UpdateAsync(Guid userId, Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(userId, id, cancellationToken);
        if (category is null)
        {
            return null;
        }

        category.Name = request.Name.Trim();
        category.IsIncome = request.IsIncome;
        await _categoryRepository.SaveChangesAsync(cancellationToken);
        return Map(category);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(userId, id, cancellationToken);
        if (category is null)
        {
            return false;
        }

        _categoryRepository.Remove(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static CategoryDto Map(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            IsIncome = category.IsIncome,
            CreatedAtUtc = category.CreatedAtUtc
        };
    }
}
