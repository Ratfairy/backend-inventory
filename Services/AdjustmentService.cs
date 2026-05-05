using Microsoft.EntityFrameworkCore;
using backend_inventory.Data;
using backend_inventory.DTOs.Adjustment;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class AdjustmentService : IAdjustmentService
{
    private readonly AppDbContext _context;

    public AdjustmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AdjustmentResponseDto>> GetAllAdjustmentsAsync()
    {
        return await _context.Adjustments
            .Include(a => a.Stock)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AdjustmentResponseDto
            {
                Id = a.Id,
                StockId = a.StockId,
                ItemName = a.Stock!.ItemName,
                AdjustmentQty = a.AdjustmentQty,
                Reason = a.Reason,
                Pic = a.Pic,
                Status = a.Status,
                CreatedAt = a.CreatedAt,
            })
            .ToListAsync();
    }

    public async Task<AdjustmentResponseDto?> GetAdjustmentByIdAsync(int id)
    {
        var a = await _context.Adjustments
            .Include(a => a.Stock)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (a == null) return null;

        return new AdjustmentResponseDto
        {
            Id = a.Id,
            StockId = a.StockId,
            ItemName = a.Stock!.ItemName,
            AdjustmentQty = a.AdjustmentQty,
            Reason = a.Reason,
            Pic = a.Pic,
            Status = a.Status,
            CreatedAt = a.CreatedAt,
        };
    }

    public async Task<AdjustmentResponseDto> CreateAdjustmentAsync(CreateAdjustmentDto dto)
    {
        var stock = await _context.Stocks.FindAsync(dto.StockId)
            ?? throw new Exception("Stock tidak ditemukan");

        var adjustment = new Adjustment
        {
            StockId = dto.StockId,
            AdjustmentQty = dto.AdjustmentQty,
            Reason = dto.Reason,
            Pic = dto.Pic,
            Status = "WAITING",
            CreatedAt = DateTime.UtcNow,
        };

        _context.Adjustments.Add(adjustment);
        await _context.SaveChangesAsync();

        return new AdjustmentResponseDto
        {
            Id = adjustment.Id,
            StockId = adjustment.StockId,
            ItemName = stock.ItemName,
            AdjustmentQty = adjustment.AdjustmentQty,
            Reason = adjustment.Reason,
            Pic = adjustment.Pic,
            Status = adjustment.Status,
            CreatedAt = adjustment.CreatedAt,
        };
    }

    public async Task<AdjustmentResponseDto?> UpdateStatusAsync(int id, UpdateAdjustmentStatusDto dto)
    {
        var adjustment = await _context.Adjustments
            .Include(a => a.Stock)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adjustment == null) return null;

        adjustment.Status = dto.Status;

        // Kalau APPROVED, update qty stock
        if (dto.Status == "APPROVED")
        {
            var stock = await _context.Stocks.FindAsync(adjustment.StockId)
                ?? throw new Exception("Stock tidak ditemukan");

            stock.Qty += adjustment.AdjustmentQty;
            stock.UpdatedAt = DateTime.UtcNow;

            // Catat di stock movement
            _context.StockMovements.Add(new StockMovement
            {
                StockId = stock.Id,
                Type = adjustment.AdjustmentQty > 0 ? "IN" : "OUT",
                Qty = Math.Abs(adjustment.AdjustmentQty),
                Description = $"Adjustment: {adjustment.Reason}",
                Pic = adjustment.Pic,
                Date = DateTime.UtcNow,
            });
        }

        await _context.SaveChangesAsync();

        return new AdjustmentResponseDto
        {
            Id = adjustment.Id,
            StockId = adjustment.StockId,
            ItemName = adjustment.Stock!.ItemName,
            AdjustmentQty = adjustment.AdjustmentQty,
            Reason = adjustment.Reason,
            Pic = adjustment.Pic,
            Status = adjustment.Status,
            CreatedAt = adjustment.CreatedAt,
        };
    }

    public async Task<bool> DeleteAdjustmentAsync(int id)
    {
        var adjustment = await _context.Adjustments.FindAsync(id);
        if (adjustment == null) return false;

        _context.Adjustments.Remove(adjustment);
        await _context.SaveChangesAsync();
        return true;
    }
}