using Microsoft.EntityFrameworkCore;

using backend_inventory.Data;
using backend_inventory.DTOs.Item;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class ItemService : IItemService
{
    private readonly AppDbContext _context;

    public ItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ItemResponseDto>>
        GetAllItemsAsync()
    {
        return await _context.Items
            .Include(i => i.Category)
            .Include(i => i.Stock)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new ItemResponseDto
            {
                Id = i.Id,
                CategoryId = i.CategoryId,
                CategoryName = i.Category.Name,
                ItemName = i.ItemName,
                Unit = i.Unit,
                Price = i.Price,
                Status = i.Status,
                CreatedBy = i.CreatedBy,
                HasStock = i.Stock != null,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemResponseDto>>
        GetApprovedItemsByCategoryAsync(int categoryId)
    {
        return await _context.Items
            .Include(i => i.Category)
            .Include(i => i.Stock)
            .Where(i =>
                i.CategoryId == categoryId &&
                i.Status == "APPROVED"
            )
            .OrderBy(i => i.ItemName)
            .Select(i => new ItemResponseDto
            {
                Id = i.Id,
                CategoryId = i.CategoryId,
                CategoryName = i.Category.Name,
                ItemName = i.ItemName,
                Unit = i.Unit,
                Price = i.Price,
                Status = i.Status,
                CreatedBy = i.CreatedBy,
                HasStock = i.Stock != null,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ItemResponseDto?>
        GetItemByIdAsync(int id)
    {
        var item = await _context.Items
            .Include(i => i.Category)
            .Include(i => i.Stock)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return null;

        return new ItemResponseDto
        {
            Id = item.Id,
            CategoryId = item.CategoryId,
            CategoryName = item.Category.Name,
            ItemName = item.ItemName,
            Unit = item.Unit,
            Price = item.Price,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            HasStock = item.Stock != null,
            CreatedAt = item.CreatedAt
        };
    }

    public async Task<ItemResponseDto>
        CreateItemAsync(CreateItemDto dto)
    {
        var category = await _context.Categories
            .FindAsync(dto.CategoryId);

        if (category == null)
            throw new Exception(
                "Category tidak ditemukan"
            );

        var exists = await _context.Items
            .AnyAsync(i =>
                i.ItemName == dto.ItemName &&
                i.CategoryId == dto.CategoryId
            );

        if (exists)
            throw new Exception(
                "Item sudah ada di category ini"
            );

        var item = new Item
        {
            CategoryId = dto.CategoryId,
            ItemName = dto.ItemName,
            Unit = dto.Unit,
            Price = dto.Price,
            Status = "WAITING",
            CreatedBy = dto.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        _context.Items.Add(item);

        await _context.SaveChangesAsync();

        return (await GetItemByIdAsync(item.Id))!;
    }

    public async Task<ItemResponseDto?>
        UpdateItemAsync(
            int id,
            UpdateItemDto dto
        )
    {
        var item = await _context.Items
            .FindAsync(id);

        if (item == null)
            return null;

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
            throw new Exception(
                "Category tidak ditemukan"
            );

        var duplicate = await _context.Items
            .AnyAsync(i =>
                i.Id != id &&
                i.ItemName == dto.ItemName &&
                i.CategoryId == dto.CategoryId
            );

        if (duplicate)
            throw new Exception(
                "Item sudah ada di category ini"
            );

        item.CategoryId = dto.CategoryId;
        item.ItemName = dto.ItemName;
        item.Unit = dto.Unit;
        item.Price = dto.Price;

        await _context.SaveChangesAsync();

        return await GetItemByIdAsync(id);
    }

    public async Task<ItemResponseDto?>
        UpdateStatusAsync(
            int id,
            UpdateItemStatusDto dto
        )
    {
        var item = await _context.Items
            .Include(i => i.Category)
            .Include(i => i.Stock)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return null;

        if (item.Status != "WAITING")
            throw new Exception(
                "Item sudah diproses"
            );

        item.Status = dto.Status;

        await _context.SaveChangesAsync();

        return await GetItemByIdAsync(id);
    }

    public async Task<bool>
        DeleteItemAsync(int id)
    {
        var item = await _context.Items
            .Include(i => i.Stock)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return false;

        if (item.Stock != null)
            throw new Exception(
                "Item sudah digunakan stock"
            );

        _context.Items.Remove(item);

        await _context.SaveChangesAsync();

        return true;
    }
}
