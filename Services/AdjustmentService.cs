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
        var adjustments = await _context.Adjustments
            .Include(a => a.Items)
                .ThenInclude(ai => ai.Stock)
                    .ThenInclude(s => s!.Item)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return adjustments.Select(a => new AdjustmentResponseDto
        {
            Id = a.Id,
            Date = a.Date,
            Pic = a.Pic,
            Reason = a.Reason,
            Status = a.Status,
            CreatedAt = a.CreatedAt,
            Items = a.Items.Select(ai => new AdjustmentItemResponseDto
            {
                Id = ai.Id,
                StockId = ai.StockId,
                ItemName = ai.Stock?.Item?.ItemName ?? string.Empty,
                SystemQty = ai.SystemQty,
                ActualQty = ai.ActualQty,
                AdjustmentQty = ai.AdjustmentQty
            }).ToList()
        });
    }

    public async Task<AdjustmentResponseDto?> GetAdjustmentByIdAsync(int id)
    {
        var adjustment = await _context.Adjustments
            .Include(a => a.Items)
                .ThenInclude(ai => ai.Stock)
                    .ThenInclude(s => s!.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adjustment == null)
            return null;

        return new AdjustmentResponseDto
        {
            Id = adjustment.Id,
            Date = adjustment.Date,
            Pic = adjustment.Pic,
            Reason = adjustment.Reason,
            Status = adjustment.Status,
            CreatedAt = adjustment.CreatedAt,
            Items = adjustment.Items.Select(ai => new AdjustmentItemResponseDto
            {
                Id = ai.Id,
                StockId = ai.StockId,
                ItemName = ai.Stock?.Item?.ItemName ?? string.Empty,
                SystemQty = ai.SystemQty,
                ActualQty = ai.ActualQty,
                AdjustmentQty = ai.AdjustmentQty
            }).ToList()
        };
    }

    public async Task<AdjustmentResponseDto> CreateAdjustmentAsync(CreateAdjustmentDto dto)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new Exception("Item adjustment wajib diisi");

        var duplicateStock = dto.Items
            .GroupBy(x => x.StockId)
            .Any(g => g.Count() > 1);

        if (duplicateStock)
        {
            throw new Exception(
                "Stock yang sama tidak boleh dipilih lebih dari sekali"
            );
        }

        var stockIds = dto.Items
            .Select(x => x.StockId)
            .Distinct()
            .ToList();

        var stocks = await _context.Stocks
            .Include(s => s.Item)
            .Where(s => stockIds.Contains(s.Id))
            .ToListAsync();

        if (stocks.Count != stockIds.Count)
            throw new Exception("Ada stock yang tidak ditemukan");

        var adjustment = new Adjustment
        {
            Date = dto.Date,
            Pic = dto.Pic,
            Reason = dto.Reason,
            Status = "WAITING",
            CreatedAt = DateTime.UtcNow,
            Items = new List<AdjustmentItem>()
        };

        foreach (var dtoItem in dto.Items)
        {
            if (dtoItem.ActualQty < 0)
            {
                throw new Exception(
                    "Actual qty tidak boleh minus"
                );
            }
            var stock = stocks.First(s => s.Id == dtoItem.StockId);
            var systemQty = stock.Qty;
            var actualQty = dtoItem.ActualQty;
            var adjustmentQty = actualQty - systemQty;

            adjustment.Items.Add(new AdjustmentItem
            {
                StockId = stock.Id,
                SystemQty = systemQty,
                ActualQty = actualQty,
                AdjustmentQty = adjustmentQty
            });
        }

        _context.Adjustments.Add(adjustment);
        await _context.SaveChangesAsync();

        return (await GetAdjustmentByIdAsync(adjustment.Id))!;
    }

    public async Task<AdjustmentResponseDto?> UpdateStatusAsync(int id, UpdateAdjustmentStatusDto dto)
    {
        using var transaction =
            await _context.Database.BeginTransactionAsync();

        var adjustment = await _context.Adjustments
            .Include(a => a.Items)
                .ThenInclude(ai => ai.Stock)
                    .ThenInclude(s => s!.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adjustment == null)
            return null;

        if (adjustment.Status != "WAITING")
            throw new Exception("Adjustment sudah diproses");

        adjustment.Status = dto.Status;

        if (dto.Status == "APPROVED")
        {
            foreach (var item in adjustment.Items)
            {
                if (item.Stock == null)
                    throw new Exception("Stock adjustment tidak ditemukan");

                item.Stock.Qty = item.ActualQty;
                item.Stock.UpdatedAt = DateTime.UtcNow;

                if (item.AdjustmentQty != 0)
                {
                    _context.StockMovements.Add(new StockMovement
                    {
                        StockId = item.StockId,
                        Type =
                        item.AdjustmentQty > 0
                            ? "IN"
                            : "OUT",
                        Qty = Math.Abs(item.AdjustmentQty),
                        Description = $"Adjustment #{adjustment.Id}",
                        Pic = adjustment.Pic,
                        Date = DateTime.UtcNow,
                        ReferenceType = "ADJUSTMENT"
                    });
                }
            }
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return await GetAdjustmentByIdAsync(id);
    }

    public async Task<bool> DeleteAdjustmentAsync(int id)
    {
        var adjustment = await _context.Adjustments
            .Include(a => a.Items)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adjustment == null)
            return false;

        if (adjustment.Status == "APPROVED")
            throw new Exception("Adjustment yang sudah approved tidak bisa dihapus");

        _context.AdjustmentItems.RemoveRange(adjustment.Items);
        _context.Adjustments.Remove(adjustment);

        await _context.SaveChangesAsync();

        return true;
    }
}
