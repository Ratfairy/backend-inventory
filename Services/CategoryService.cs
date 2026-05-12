using Microsoft.EntityFrameworkCore;

using backend_inventory.Data;
using backend_inventory.DTOs.Category;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryResponseDto>>
        GetAllCategoriesAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.Name)
            .Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<CategoryResponseDto?>
        GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return null;

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }

    public async Task<CategoryResponseDto>
        CreateCategoryAsync(CreateCategoryDto dto)
    {
        var exists = await _context.Categories
            .AnyAsync(c => c.Name == dto.Name);

        if (exists)
            throw new Exception("Category sudah ada");

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }

    public async Task<CategoryResponseDto?>
        UpdateCategoryAsync(
            int id,
            UpdateCategoryDto dto
        )
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return null;

        category.Name = dto.Name;
        category.Description = dto.Description;

        await _context.SaveChangesAsync();

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }

    public async Task<bool>
        DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return false;

        // Prevent delete kalau masih ada item
        if (category.Items.Any())
            throw new Exception(
                "Category masih digunakan item"
            );

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();

        return true;
    }
}