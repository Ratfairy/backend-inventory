using Microsoft.EntityFrameworkCore;
using backend_inventory.Data;
using backend_inventory.DTOs.PurchaseOrder;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly AppDbContext _context;

    public PurchaseOrderService(AppDbContext context)
    {
        _context = context;
    }

    private static POResponseDto MapToDto(PurchaseOrder po) => new()
    {
        Id = po.Id,
        PoNumber = po.PoNumber,
        PurchaseRequestId = po.PurchaseRequestId,
        PrNumber = po.PurchaseRequest?.PrNumber ?? string.Empty,
        Supplier = po.Supplier,
        Status = po.Status,
        Department = po.PurchaseRequest?.Department ?? string.Empty,
        Pic = po.PurchaseRequest?.Pic ?? string.Empty,
        NeededDate = po.PurchaseRequest?.NeededDate ?? DateTime.MinValue,
        Date = po.Date,
        CreatedAt = po.CreatedAt,
        Items = po.Items.Select(i => new POItemDto
        {
            Id = i.Id,
            ItemName = i.ItemName,
            Qty = i.Qty,
            Unit = i.Unit,
            Price = i.Price,
            Reason = i.Reason,
        }).ToList(),
    };

    public async Task<IEnumerable<POResponseDto>> GetAllPOsAsync()
    {
        var pos = await _context.PurchaseOrders
            .Include(po => po.PurchaseRequest)
            .Include(po => po.Items)
            .OrderByDescending(po => po.CreatedAt)
            .ToListAsync();

        return pos.Select(MapToDto);
    }

    public async Task<POResponseDto?> GetPOByIdAsync(int id)
    {
        var po = await _context.PurchaseOrders
            .Include(po => po.PurchaseRequest)
            .Include(po => po.Items)
            .FirstOrDefaultAsync(po => po.Id == id);

        return po == null ? null : MapToDto(po);
    }

    public async Task<IEnumerable<POResponseDto>> GetApprovedPRsForPOAsync()
    {
        // Ambil PR yang APPROVED dan belum punya PO
        var approvedPRs = await _context.PurchaseRequests
            .Include(pr => pr.Items)
            .Include(pr => pr.PurchaseOrder)
            .Where(pr => pr.Status == "APPROVED" && pr.PurchaseOrder == null)
            .ToListAsync();

        return approvedPRs.Select(pr => new POResponseDto
        {
            PurchaseRequestId = pr.Id,
            PrNumber = pr.PrNumber,
            Department = pr.Department,
            Pic = pr.Pic,
            NeededDate = pr.NeededDate,
            Items = pr.Items.Select(i => new POItemDto
            {
                ItemName = i.ItemName,
                Qty = i.Qty,
                Unit = i.Unit,
                Price = i.Price,
                Reason = i.Reason,
            }).ToList(),
        });
    }

    public async Task<POResponseDto> CreatePOAsync(CreatePODto dto)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new Exception("Item PO wajib diisi");

        var pr = await _context.PurchaseRequests
            .Include(pr => pr.PurchaseOrder)
            .FirstOrDefaultAsync(pr => pr.Id == dto.PurchaseRequestId)
            ?? throw new Exception("Purchase Request tidak ditemukan");

        if (pr.Status != "APPROVED")
            throw new Exception("PR harus berstatus APPROVED");

        if (pr.PurchaseOrder != null)
            throw new Exception("PR ini sudah memiliki PO");

        var count = await _context.PurchaseOrders.CountAsync();
        var poNumber = $"PO-{DateTime.Now:yyyy}-{(count + 1):D3}";

        var po = new PurchaseOrder
        {
            PoNumber = poNumber,
            PurchaseRequestId = dto.PurchaseRequestId,
            Supplier = dto.Supplier,
            Status = "DRAFT",
            Date = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            Items = dto.Items.Select(i => new PurchaseOrderItem
            {
                ItemName = i.ItemName,
                Qty = i.Qty,
                Unit = i.Unit,
                Price = i.Price,
                Reason = i.Reason,
            }).ToList(),
        };

        _context.PurchaseOrders.Add(po);

        // Otomatis buat ReceiveGoods dengan status PENDING
        _context.ReceiveGoods.Add(new ReceiveGoods
        {
            PurchaseOrder = po,
            Status = "PENDING",
            CreatedAt = DateTime.UtcNow,
            Items = dto.Items.Select(i => new ReceiveGoodsItem
            {
                ItemName = i.ItemName,
                QtyOrdered = i.Qty,
                QtyReceived = 0,
                Unit = i.Unit,
            }).ToList(),
        });

        await _context.SaveChangesAsync();

        return (await GetPOByIdAsync(po.Id))!;
    }

    public async Task<POResponseDto?> UpdatePOAsync(int id, UpdatePODto dto)
    {
        var po = await _context.PurchaseOrders
            .Include(po => po.Items)
            .Include(po => po.ReceiveGoods)
                .ThenInclude(rg => rg!.Items)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (po == null) return null;
        if (po.Status != "DRAFT")
            throw new Exception("PO hanya bisa diedit saat status DRAFT");

        if (dto.Items == null || !dto.Items.Any())
            throw new Exception("Item PO wajib diisi");

        po.Supplier = dto.Supplier;

        _context.PurchaseOrderItems.RemoveRange(po.Items);
        po.Items = dto.Items.Select(i => new PurchaseOrderItem
        {
            ItemName = i.ItemName,
            Qty = i.Qty,
            Unit = i.Unit,
            Price = i.Price,
            Reason = i.Reason,
            PurchaseOrderId = id,
        }).ToList();

        if (po.ReceiveGoods != null)
        {
            _context.ReceiveGoodsItems.RemoveRange(po.ReceiveGoods.Items);
            po.ReceiveGoods.Items = dto.Items.Select(i => new ReceiveGoodsItem
            {
                ItemName = i.ItemName,
                QtyOrdered = i.Qty,
                QtyReceived = 0,
                Unit = i.Unit,
                ReceiveGoodsId = po.ReceiveGoods.Id,
            }).ToList();
            po.ReceiveGoods.Status = "PENDING";
            po.ReceiveGoods.ReceivedDate = null;
        }

        await _context.SaveChangesAsync();

        return await GetPOByIdAsync(id);
    }

    public async Task<POResponseDto?> UpdateStatusAsync(int id, UpdatePOStatusDto dto)
    {
        var po = await _context.PurchaseOrders
            .Include(po => po.PurchaseRequest)
            .Include(po => po.Items)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (po == null) return null;

        if (po.Status != "DRAFT")
            throw new Exception("PO hanya bisa dikirim dari status DRAFT");

        po.Status = dto.Status;
        await _context.SaveChangesAsync();

        return MapToDto(po);
    }

    public async Task<bool> DeletePOAsync(int id)
    {
        var po = await _context.PurchaseOrders
            .Include(po => po.Items)
            .Include(po => po.ReceiveGoods)
                .ThenInclude(rg => rg!.Items)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (po == null) return false;
        if (po.Status != "DRAFT")
            throw new Exception("PO hanya bisa dihapus saat status DRAFT");

        if (po.ReceiveGoods != null)
        {
            _context.ReceiveGoodsItems.RemoveRange(po.ReceiveGoods.Items);
            _context.ReceiveGoods.Remove(po.ReceiveGoods);
        }

        _context.PurchaseOrderItems.RemoveRange(po.Items);
        _context.PurchaseOrders.Remove(po);
        await _context.SaveChangesAsync();
        return true;
    }
}
