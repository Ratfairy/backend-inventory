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

    // =====================================================
    // STOCK
    // =====================================================

    public async Task<IEnumerable<StockResponseDto>> GetAllStocksAsync()
    {
        return await _context.Stocks
            .Include(s => s.Item)
            .ThenInclude(i => i.Category)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new StockResponseDto
            {
                Id = s.Id,
                ItemId = s.ItemId,
                ItemName = s.Item != null
                    ? s.Item.ItemName
                    : string.Empty,

                CategoryName =
                    s.Item != null &&
                    s.Item.Category != null
                        ? s.Item.Category.Name
                        : string.Empty,

                Unit = s.Item != null
                    ? s.Item.Unit
                    : string.Empty,

                Price = s.Item != null
                    ? s.Item.Price
                    : 0,

                Qty = s.Qty,
                MinQty = s.MinQty,
                UpdatedAt = s.UpdatedAt,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<StockResponseDto?> GetStockByIdAsync(int id)
    {
        var stock = await _context.Stocks
            .Include(s => s.Item)
            .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
            return null;

        return new StockResponseDto
        {
            Id = stock.Id,
            ItemId = stock.ItemId,

            ItemName = stock.Item != null
                ? stock.Item.ItemName
                : string.Empty,

            CategoryName =
                stock.Item != null &&
                stock.Item.Category != null
                    ? stock.Item.Category.Name
                    : string.Empty,

            Unit = stock.Item != null
                ? stock.Item.Unit
                : string.Empty,

            Price = stock.Item != null
                ? stock.Item.Price
                : 0,

            Qty = stock.Qty,
            MinQty = stock.MinQty,
            UpdatedAt = stock.UpdatedAt,
            CreatedAt = stock.CreatedAt
        };
    }

    public async Task<StockResponseDto> CreateStockAsync(CreateStockDto dto)
    {
        var item = await _context.Items
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Id == dto.ItemId);

        if (item == null)
            throw new Exception("Item tidak ditemukan");

        if (item.Status != "APPROVED")
            throw new Exception("Item belum di-approve");

        if (dto.MinQty > dto.Qty)
            throw new Exception(
                "Minimum qty tidak boleh lebih besar dari stock qty"
            );

        var stockExists = await _context.Stocks
            .AnyAsync(s => s.ItemId == dto.ItemId);

        if (stockExists)
            throw new Exception("Stock untuk item ini sudah ada");

        var stock = new Stock
        {
            ItemId = dto.ItemId,
            Qty = dto.Qty,
            MinQty = dto.MinQty,
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.Stocks.Add(stock);

        await _context.SaveChangesAsync();

        return new StockResponseDto
        {
            Id = stock.Id,
            ItemId = stock.ItemId,

            ItemName = item.ItemName,

            CategoryName =
                item.Category != null
                    ? item.Category.Name
                    : string.Empty,

            Unit = item.Unit,
            Price = item.Price,

            Qty = stock.Qty,
            MinQty = stock.MinQty,
            UpdatedAt = stock.UpdatedAt,
            CreatedAt = stock.CreatedAt
        };
    }

    public async Task<StockResponseDto?> UpdateStockAsync(
        int id,
        UpdateStockDto dto
    )
    {
        var stock = await _context.Stocks
            .Include(s => s.Item)
            .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
            return null;

        if (dto.MinQty > dto.Qty)
            throw new Exception(
                "Minimum qty tidak boleh lebih besar dari stock qty"
            );

        stock.Qty = dto.Qty;
        stock.MinQty = dto.MinQty;
        stock.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new StockResponseDto
        {
            Id = stock.Id,
            ItemId = stock.ItemId,

            ItemName = stock.Item != null
                ? stock.Item.ItemName
                : string.Empty,

            CategoryName =
                stock.Item != null &&
                stock.Item.Category != null
                    ? stock.Item.Category.Name
                    : string.Empty,

            Unit = stock.Item != null
                ? stock.Item.Unit
                : string.Empty,

            Price = stock.Item != null
                ? stock.Item.Price
                : 0,

            Qty = stock.Qty,
            MinQty = stock.MinQty,
            UpdatedAt = stock.UpdatedAt,
            CreatedAt = stock.CreatedAt
        };
    }

    public async Task<bool> DeleteStockAsync(int id)
    {
        var stock = await _context.Stocks
            .Include(s => s.StockMovements)
            .Include(s => s.AdjustmentItems)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
            return false;

        if (stock.StockMovements.Any())
            throw new Exception(
                "Stock sudah memiliki riwayat movement"
            );

        if (stock.AdjustmentItems.Any())
            throw new Exception(
                "Stock sudah digunakan adjustment"
            );

        _context.Stocks.Remove(stock);

        await _context.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // STOCK MOVEMENT
    // =====================================================

    public async Task<IEnumerable<StockMovementResponseDto>>
        GetAllMovementsAsync()
    {
        return await _context.StockMovements
            .Include(m => m.Stock)
            .ThenInclude(s => s!.Item)
            .OrderByDescending(m => m.Date)
            .Select(m => new StockMovementResponseDto
            {
                Id = m.Id,
                StockId = m.StockId,

                ItemName =
                    m.Stock != null &&
                    m.Stock.Item != null
                        ? m.Stock.Item.ItemName
                        : string.Empty,

                Type = m.Type,
                Qty = m.Qty,
                Description = m.Description,
                Pic = m.Pic,
                Date = m.Date,
                ReferenceType = m.ReferenceType
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<StockMovementResponseDto>>
        GetMovementsByStockIdAsync(int stockId)
    {
        return await _context.StockMovements
            .Include(m => m.Stock)
            .ThenInclude(s => s!.Item)
            .Where(m => m.StockId == stockId)
            .OrderByDescending(m => m.Date)
            .Select(m => new StockMovementResponseDto
            {
                Id = m.Id,
                StockId = m.StockId,

                ItemName =
                    m.Stock != null &&
                    m.Stock.Item != null
                        ? m.Stock.Item.ItemName
                        : string.Empty,

                Type = m.Type,
                Qty = m.Qty,
                Description = m.Description,
                Pic = m.Pic,
                Date = m.Date,
                ReferenceType = m.ReferenceType
            })
            .ToListAsync();
    }

    public async Task<StockMovementResponseDto>
        CreateMovementAsync(CreateStockMovementDto dto)
    {
        var stock = await _context.Stocks
            .Include(s => s.Item)
            .FirstOrDefaultAsync(s => s.Id == dto.StockId);

        if (stock == null)
            throw new Exception("Stock tidak ditemukan");

        if (dto.Qty <= 0)
            throw new Exception("Qty movement harus lebih besar dari 0");

        if (dto.Type != "IN" && dto.Type != "OUT")
            throw new Exception("Type movement tidak valid");

        if (dto.Type == "OUT" && dto.Qty > stock.Qty)
            throw new Exception("Qty stock tidak mencukupi");

        // UPDATE QTY
        if (dto.Type == "IN")
            stock.Qty += dto.Qty;
        else
            stock.Qty -= dto.Qty;

        stock.UpdatedAt = DateTime.UtcNow;

        // CREATE MOVEMENT
        var movement = new backend_inventory.Models.StockMovement
        {
            StockId = dto.StockId,
            Type = dto.Type,
            Qty = dto.Qty,
            Description = dto.Description,
            Pic = dto.Pic,
            Date = DateTime.UtcNow,
            ReferenceType = dto.ReferenceType
        };

        _context.StockMovements.Add(movement);

        await _context.SaveChangesAsync();

        return new StockMovementResponseDto
        {
            Id = movement.Id,
            StockId = movement.StockId,

            ItemName =
                stock.Item != null
                    ? stock.Item.ItemName
                    : string.Empty,

            Type = movement.Type,
            Qty = movement.Qty,
            Description = movement.Description,
            Pic = movement.Pic,
            Date = movement.Date,
            ReferenceType = movement.ReferenceType
        };
    }
}
