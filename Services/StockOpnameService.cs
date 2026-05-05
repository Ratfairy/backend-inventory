using Microsoft.EntityFrameworkCore;
using backend_inventory.Data;
using backend_inventory.DTOs.StockOpname;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class StockOpnameService : IStockOpnameService
{
    private readonly AppDbContext _context;

    public StockOpnameService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockOpnameResponseDto>> GetAllOpnamesAsync()
    {
        return await _context.StockOpnames
            .Include(o => o.Items)
                .ThenInclude(i => i.Stock)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new StockOpnameResponseDto
            {
                Id = o.Id,
                Date = o.Date,
                Pic = o.Pic,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new StockOpnameItemResponseDto
                {
                    Id = i.Id,
                    StockId = i.StockId,
                    ItemName = i.Stock!.ItemName,
                    SystemQty = i.SystemQty,
                    ActualQty = i.ActualQty,
                    Adjustment = i.Adjustment,
                }).ToList(),
            })
            .ToListAsync();
    }

    public async Task<StockOpnameResponseDto?> GetOpnameByIdAsync(int id)
    {
        var opname = await _context.StockOpnames
            .Include(o => o.Items)
                .ThenInclude(i => i.Stock)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (opname == null) return null;

        return new StockOpnameResponseDto
        {
            Id = opname.Id,
            Date = opname.Date,
            Pic = opname.Pic,
            Status = opname.Status,
            CreatedAt = opname.CreatedAt,
            Items = opname.Items.Select(i => new StockOpnameItemResponseDto
            {
                Id = i.Id,
                StockId = i.StockId,
                ItemName = i.Stock!.ItemName,
                SystemQty = i.SystemQty,
                ActualQty = i.ActualQty,
                Adjustment = i.Adjustment,
            }).ToList(),
        };
    }

    public async Task<StockOpnameResponseDto> CreateOpnameAsync(CreateStockOpnameDto dto)
    {
        var opname = new StockOpname
        {
            Date = dto.Date,
            Pic = dto.Pic,
            Status = "WAITING",
            CreatedAt = DateTime.UtcNow,
        };

        foreach (var itemDto in dto.Items)
        {
            var stock = await _context.Stocks.FindAsync(itemDto.StockId)
                ?? throw new Exception($"Stock id {itemDto.StockId} tidak ditemukan");

            opname.Items.Add(new StockOpnameItem
            {
                StockId = itemDto.StockId,
                SystemQty = stock.Qty,
                ActualQty = itemDto.ActualQty,
                Adjustment = itemDto.ActualQty - stock.Qty,
            });
        }

        _context.StockOpnames.Add(opname);
        await _context.SaveChangesAsync();

        return (await GetOpnameByIdAsync(opname.Id))!;
    }

    public async Task<StockOpnameResponseDto?> UpdateStatusAsync(int id, UpdateStockOpnameStatusDto dto)
    {
        var opname = await _context.StockOpnames
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (opname == null) return null;

        opname.Status = dto.Status;

        // Kalau APPROVED, update qty semua stock
        if (dto.Status == "APPROVED")
        {
            foreach (var item in opname.Items)
            {
                var stock = await _context.Stocks.FindAsync(item.StockId)
                    ?? throw new Exception($"Stock id {item.StockId} tidak ditemukan");

                stock.Qty = item.ActualQty;
                stock.UpdatedAt = DateTime.UtcNow;

                // Catat di stock movement kalau ada selisih
                if (item.Adjustment != 0)
                {
                    _context.StockMovements.Add(new StockMovement
                    {
                        StockId = stock.Id,
                        Type = item.Adjustment > 0 ? "IN" : "OUT",
                        Qty = Math.Abs(item.Adjustment),
                        Description = $"Stock Opname #{opname.Id}",
                        Pic = opname.Pic,
                        Date = DateTime.UtcNow,
                    });
                }
            }
        }

        await _context.SaveChangesAsync();

        return await GetOpnameByIdAsync(id);
    }

    public async Task<bool> DeleteOpnameAsync(int id)
    {
        var opname = await _context.StockOpnames.FindAsync(id);
        if (opname == null) return false;

        _context.StockOpnames.Remove(opname);
        await _context.SaveChangesAsync();
        return true;
    }
}