using Microsoft.EntityFrameworkCore;
using backend_inventory.Data;
using backend_inventory.DTOs.Invoice;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class InvoiceService : IInvoiceService
{
    private readonly AppDbContext _context;

    public InvoiceService(AppDbContext context)
    {
        _context = context;
    }

    private static InvoiceResponseDto MapToDto(Invoice inv) => new()
    {
        Id = inv.Id,
        InvoiceNumber = inv.InvoiceNumber,
        ReceiveGoodsId = inv.ReceiveGoodsId,
        PoNumber = inv.ReceiveGoods?.PurchaseOrder?.PoNumber ?? string.Empty,
        Supplier = inv.ReceiveGoods?.PurchaseOrder?.Supplier ?? string.Empty,
        Department = inv.ReceiveGoods?.PurchaseOrder?.PurchaseRequest?.Department ?? string.Empty,
        Pic = inv.ReceiveGoods?.PurchaseOrder?.PurchaseRequest?.Pic ?? string.Empty,
        InvoiceDate = inv.InvoiceDate,
        ReceivedDate = inv.ReceiveGoods?.ReceivedDate ?? DateTime.MinValue,
        InvoiceRef = inv.InvoiceRef,
        Notes = inv.Notes,
        Status = inv.Status,
        CreatedAt = inv.CreatedAt,
        Items = inv.Items.Select(i => new InvoiceItemDto
        {
            Id = i.Id,
            ItemName = i.ItemName,
            QtyReceived = i.QtyReceived,
            Unit = i.Unit,
            Price = i.Price,
        }).ToList(),
    };

    public async Task<IEnumerable<InvoiceResponseDto>> GetAllInvoicesAsync()
    {
        var invoices = await _context.Invoices
            .Include(inv => inv.ReceiveGoods)
                .ThenInclude(rg => rg!.PurchaseOrder)
                    .ThenInclude(po => po!.PurchaseRequest)
            .Include(inv => inv.Items)
            .OrderByDescending(inv => inv.CreatedAt)
            .ToListAsync();

        return invoices.Select(MapToDto);
    }

    public async Task<InvoiceResponseDto?> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _context.Invoices
            .Include(inv => inv.ReceiveGoods)
                .ThenInclude(rg => rg!.PurchaseOrder)
                    .ThenInclude(po => po!.PurchaseRequest)
            .Include(inv => inv.Items)
            .FirstOrDefaultAsync(inv => inv.Id == id);

        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<IEnumerable<InvoiceResponseDto>> GetReceivedPOsForInvoiceAsync()
    {
        // Ambil ReceiveGoods yang RECEIVED dan belum punya Invoice
        var receivedGoods = await _context.ReceiveGoods
            .Include(rg => rg.PurchaseOrder)
                .ThenInclude(po => po!.PurchaseRequest)
            .Include(rg => rg.Items)
            .Include(rg => rg.Invoice)
            .Where(rg => rg.Status == "RECEIVED" && rg.Invoice == null)
            .ToListAsync();

        return receivedGoods.Select(rg => new InvoiceResponseDto
        {
            ReceiveGoodsId = rg.Id,
            PoNumber = rg.PurchaseOrder?.PoNumber ?? string.Empty,
            Supplier = rg.PurchaseOrder?.Supplier ?? string.Empty,
            Department = rg.PurchaseOrder?.PurchaseRequest?.Department ?? string.Empty,
            Pic = rg.PurchaseOrder?.PurchaseRequest?.Pic ?? string.Empty,
            ReceivedDate = rg.ReceivedDate ?? DateTime.MinValue,
            Items = rg.Items.Select(i => new InvoiceItemDto
            {
                ItemName = i.ItemName,
                QtyReceived = i.QtyReceived,
                Unit = i.Unit,
            }).ToList(),
        });
    }

    public async Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto dto)
    {
        var rg = await _context.ReceiveGoods
            .Include(rg => rg.Items)
            .Include(rg => rg.Invoice)
            .Include(rg => rg.PurchaseOrder)
                .ThenInclude(po => po!.Items)
            .FirstOrDefaultAsync(rg => rg.Id == dto.ReceiveGoodsId)
            ?? throw new Exception("Receive Goods tidak ditemukan");

        if (rg.Status != "RECEIVED")
            throw new Exception("Barang harus berstatus RECEIVED");

        if (rg.Invoice != null)
            throw new Exception("Invoice sudah dibuat untuk Receive Goods ini");

        var count = await _context.Invoices.CountAsync();
        var invoiceNumber = $"INV-{DateTime.Now:yyyy}-{(count + 1):D3}";

        // Ambil harga dari PO items
        var poItems = rg.PurchaseOrder?.Items.ToDictionary(i => i.ItemName) ?? new();

        var invoice = new Invoice
        {
            InvoiceNumber = invoiceNumber,
            ReceiveGoodsId = dto.ReceiveGoodsId,
            InvoiceDate = dto.InvoiceDate,
            InvoiceRef = dto.InvoiceRef,
            Notes = dto.Notes,
            Status = "DRAFT",
            CreatedAt = DateTime.UtcNow,
            Items = rg.Items.Select(i => new InvoiceItem
            {
                ItemName = i.ItemName,
                QtyReceived = i.QtyReceived,
                Unit = i.Unit,
                Price = poItems.TryGetValue(i.ItemName, out var poItem) ? poItem.Price : 0,
            }).ToList(),
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return (await GetInvoiceByIdAsync(invoice.Id))!;
    }

    public async Task<InvoiceResponseDto?> UpdateStatusAsync(int id, UpdateInvoiceStatusDto dto)
    {
        var invoice = await _context.Invoices
            .Include(inv => inv.ReceiveGoods)
                .ThenInclude(rg => rg!.PurchaseOrder)
                    .ThenInclude(po => po!.PurchaseRequest)
            .Include(inv => inv.Items)
            .FirstOrDefaultAsync(inv => inv.Id == id);

        if (invoice == null) return null;

        invoice.Status = dto.Status;
        await _context.SaveChangesAsync();

        return MapToDto(invoice);
    }

    public async Task<bool> DeleteInvoiceAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return false;
        if (invoice.Status != "DRAFT")
            throw new Exception("Invoice hanya bisa dihapus saat status DRAFT");

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }
}