using Microsoft.EntityFrameworkCore;
using backend_inventory.Data;
using backend_inventory.DTOs.Stock;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class StockService : IStockService
{
    private readonly AppDbContext _context;

    public StockService(AppDbContext context)
    {
        _context = context;
    }

    // =====================
    // STOCK
    // =====================

    public async Task<IEnumerable<StockResponseDto>> GetAllStocksAsync()
    {
        return await _context.Stocks
            .Select(s => new StockResponseDto
            {
                Id = s.Id,
                ItemName = s.ItemName,
                Qty = s.Qty,
                MinQty = s.MinQty,
                Unit = s.Unit,
                Price = s.Price,
                UpdatedAt = s.UpdatedAt,
                CreatedAt = s.CreatedAt,
            })
            .ToListAsync();
    }

    public async Task<StockResponseDto?> GetStockByIdAsync(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null) return null;

        return new StockResponseDto
        {
            Id = stock.Id,
            ItemName = stock.ItemName,
            Qty = stock.Qty,
            MinQty = stock.MinQty,
            Unit = stock.Unit,
            Price = stock.Price,
            UpdatedAt = stock.UpdatedAt,
            CreatedAt = stock.CreatedAt,
        };
    }

    public async Task<StockResponseDto> CreateStockAsync(CreateStockDto dto)
    {
        var stock = new Stock
        {
            ItemName = dto.ItemName,
            Qty = dto.Qty,
            MinQty = dto.MinQty,
            Unit = dto.Unit,
            Price = dto.Price,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        return new StockResponseDto
        {
            Id = stock.Id,
            ItemName = stock.ItemName,
            Qty = stock.Qty,
            MinQty = stock.MinQty,
            Unit = stock.Unit,
            Price = stock.Price,
            UpdatedAt = stock.UpdatedAt,
            CreatedAt = stock.CreatedAt,
        };
    }

    public async Task<StockResponseDto?> UpdateStockAsync(int id, UpdateStockDto dto)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null) return null;

        stock.ItemName = dto.ItemName;
        stock.MinQty = dto.MinQty;
        stock.Unit = dto.Unit;
        stock.Price = dto.Price;
        stock.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new StockResponseDto
        {
            Id = stock.Id,
            ItemName = stock.ItemName,
            Qty = stock.Qty,
            MinQty = stock.MinQty,
            Unit = stock.Unit,
            Price = stock.Price,
            UpdatedAt = stock.UpdatedAt,
            CreatedAt = stock.CreatedAt,
        };
    }

    public async Task<bool> DeleteStockAsync(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null) return false;

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
        return true;
    }

    // =====================
    // STOCK MOVEMENT
    // =====================

    public async Task<IEnumerable<StockMovementResponseDto>> GetAllMovementsAsync()
    {
        return await _context.StockMovements
            .Include(m => m.Stock)
            .OrderByDescending(m => m.Date)
            .Select(m => new StockMovementResponseDto
            {
                Id = m.Id,
                StockId = m.StockId,
                ItemName = m.Stock!.ItemName,
                Type = m.Type,
                Qty = m.Qty,
                Description = m.Description,
                Pic = m.Pic,
                Date = m.Date,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<StockMovementResponseDto>> GetMovementsByStockIdAsync(int stockId)
    {
        return await _context.StockMovements
            .Include(m => m.Stock)
            .Where(m => m.StockId == stockId)
            .OrderByDescending(m => m.Date)
            .Select(m => new StockMovementResponseDto
            {
                Id = m.Id,
                StockId = m.StockId,
                ItemName = m.Stock!.ItemName,
                Type = m.Type,
                Qty = m.Qty,
                Description = m.Description,
                Pic = m.Pic,
                Date = m.Date,
            })
            .ToListAsync();
    }

    public async Task<StockMovementResponseDto> CreateMovementAsync(CreateStockMovementDto dto)
    {
        var stock = await _context.Stocks.FindAsync(dto.StockId)
            ?? throw new Exception("Stock tidak ditemukan");

        // Update qty stock
        if (dto.Type == "IN")
            stock.Qty += dto.Qty;
        else
            stock.Qty -= dto.Qty;

        stock.UpdatedAt = DateTime.UtcNow;

        var movement = new StockMovement
        {
            StockId = dto.StockId,
            Type = dto.Type,
            Qty = dto.Qty,
            Description = dto.Description,
            Pic = dto.Pic,
            Date = DateTime.UtcNow,
        };

        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();

        return new StockMovementResponseDto
        {
            Id = movement.Id,
            StockId = movement.StockId,
            ItemName = stock.ItemName,
            Type = movement.Type,
            Qty = movement.Qty,
            Description = movement.Description,
            Pic = movement.Pic,
            Date = movement.Date,
        };
    }
}