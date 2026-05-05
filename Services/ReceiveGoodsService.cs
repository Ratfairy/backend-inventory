using Microsoft.EntityFrameworkCore;
using backend_inventory.Data;
using backend_inventory.DTOs.ReceiveGoods;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class ReceiveGoodsService : IReceiveGoodsService
{
    private readonly AppDbContext _context;

    public ReceiveGoodsService(AppDbContext context)
    {
        _context = context;
    }

    private static ReceiveGoodsResponseDto MapToDto(ReceiveGoods rg) => new()
    {
        Id = rg.Id,
        PurchaseOrderId = rg.PurchaseOrderId,
        PoNumber = rg.PurchaseOrder?.PoNumber ?? string.Empty,
        Supplier = rg.PurchaseOrder?.Supplier ?? string.Empty,
        Department = rg.PurchaseOrder?.PurchaseRequest?.Department ?? string.Empty,
        Pic = rg.PurchaseOrder?.PurchaseRequest?.Pic ?? string.Empty,
        Status = rg.Status,
        ReceivedDate = rg.ReceivedDate,
            CreatedAt = rg.CreatedAt,
        Items = rg.Items.Select(i => new ReceiveGoodsItemDto
        {
            Id = i.Id,
            ItemName = i.ItemName,
            QtyOrdered = i.QtyOrdered,
            QtyReceived = i.QtyReceived,
            Unit = i.Unit,
            Note = i.Note,
        }).ToList(),
    };

    public async Task<IEnumerable<ReceiveGoodsResponseDto>> GetAllReceiveGoodsAsync()
    {
        var rgs = await _context.ReceiveGoods
            .Include(rg => rg.PurchaseOrder)
                .ThenInclude(po => po!.PurchaseRequest)
            .Include(rg => rg.Items)
            .OrderByDescending(rg => rg.CreatedAt)
            .ToListAsync();

        return rgs.Select(MapToDto);
    }

    public async Task<ReceiveGoodsResponseDto?> GetReceiveGoodsByIdAsync(int id)
    {
        var rg = await _context.ReceiveGoods
            .Include(rg => rg.PurchaseOrder)
                .ThenInclude(po => po!.PurchaseRequest)
            .Include(rg => rg.Items)
            .FirstOrDefaultAsync(rg => rg.Id == id);

        return rg == null ? null : MapToDto(rg);
    }

    public async Task<ReceiveGoodsResponseDto?> ConfirmReceiveAsync(int id, ConfirmReceiveDto dto)
    {
        var rg = await _context.ReceiveGoods
            .Include(rg => rg.Items)
            .Include(rg => rg.PurchaseOrder)
                .ThenInclude(po => po!.PurchaseRequest)
            .FirstOrDefaultAsync(rg => rg.Id == id);

        if (rg == null) return null;
        if (rg.Status == "RECEIVED")
            throw new Exception("Barang sudah diterima semua");

        foreach (var confirmItem in dto.Items)
        {
            var item = rg.Items.FirstOrDefault(i => i.Id == confirmItem.ReceiveGoodsItemId)
                ?? throw new Exception($"Item id {confirmItem.ReceiveGoodsItemId} tidak ditemukan");

            var newQtyReceived = item.QtyReceived + confirmItem.QtyReceived;
            if (newQtyReceived > item.QtyOrdered)
                throw new Exception($"Qty diterima melebihi qty order untuk item {item.ItemName}");

            item.QtyReceived = newQtyReceived;
            item.Note = confirmItem.Note;

            // Update stock
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ItemName == item.ItemName);

            if (stock != null)
            {
                stock.Qty += confirmItem.QtyReceived;
                stock.UpdatedAt = DateTime.UtcNow;

                // Catat stock movement
                _context.StockMovements.Add(new StockMovement
                {
                    StockId = stock.Id,
                    Type = "IN",
                    Qty = confirmItem.QtyReceived,
                    Description = $"Receive Goods dari PO {rg.PurchaseOrder?.PoNumber}",
                    Pic = rg.PurchaseOrder?.PurchaseRequest?.Pic,
                    Date = DateTime.UtcNow,
                });
            }
        }

        // Update status receive goods
        var allReceived = rg.Items.All(i => i.QtyReceived >= i.QtyOrdered);
        var anyReceived = rg.Items.Any(i => i.QtyReceived > 0);

        rg.Status = allReceived ? "RECEIVED" : anyReceived ? "PARTIAL" : "PENDING";
        rg.ReceivedDate = allReceived ? DateTime.UtcNow : rg.ReceivedDate;

        await _context.SaveChangesAsync();

        return MapToDto(rg);
    }
}