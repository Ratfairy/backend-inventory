using backend_inventory.DTOs.Category;

namespace backend_inventory.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();

    Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);

    Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto);

    Task<CategoryResponseDto?> UpdateCategoryAsync(
        int id,
        UpdateCategoryDto dto
    );

    Task<bool> DeleteCategoryAsync(int id);
}