using Microsoft.EntityFrameworkCore;
using backend_inventory.Data;
using backend_inventory.DTOs.PurchaseRequest;
using backend_inventory.Interfaces;
using backend_inventory.Models;

namespace backend_inventory.Services;

public class PurchaseRequestService : IPurchaseRequestService
{
    private readonly AppDbContext _context;

    public PurchaseRequestService(AppDbContext context)
    {
        _context = context;
    }

    private static PRResponseDto MapToDto(PurchaseRequest pr) => new()
    {
        Id = pr.Id,
        PrNumber = pr.PrNumber,
        Department = pr.Department,
        Pic = pr.Pic,
        Date = pr.Date,
        NeededDate = pr.NeededDate,
        Status = pr.Status,
        Comment = pr.Comment,
        CreatedAt = pr.CreatedAt,
        Items = pr.Items.Select(i => new PRItemDto
        {
            Id = i.Id,
            ItemName = i.ItemName,
            Qty = i.Qty,
            Unit = i.Unit,
            Price = i.Price,
            Reason = i.Reason,
        }).ToList(),
    };

    public async Task<IEnumerable<PRResponseDto>> GetAllPRsAsync()
    {
        var prs = await _context.PurchaseRequests
            .Include(pr => pr.Items)
            .OrderByDescending(pr => pr.CreatedAt)
            .ToListAsync();

        return prs.Select(MapToDto);
    }

    public async Task<PRResponseDto?> GetPRByIdAsync(int id)
    {
        var pr = await _context.PurchaseRequests
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(pr => pr.Id == id);

        return pr == null ? null : MapToDto(pr);
    }

    public async Task<PRResponseDto> CreatePRAsync(CreatePRDto dto)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new Exception("Item PR wajib diisi");

        // Generate PR Number
        var count = await _context.PurchaseRequests.CountAsync();
        var prNumber = $"PR-{DateTime.Now:yyyy}-{(count + 1):D3}";

        var pr = new PurchaseRequest
        {
            PrNumber = prNumber,
            Department = dto.Department,
            Pic = dto.Pic,
            Date = DateTime.UtcNow,
            NeededDate = dto.NeededDate,
            Status = "DRAFT",
            CreatedAt = DateTime.UtcNow,
            Items = dto.Items.Select(i => new PurchaseRequestItem
            {
                ItemName = i.ItemName,
                Qty = i.Qty,
                Unit = i.Unit,
                Price = i.Price,
                Reason = i.Reason,
            }).ToList(),
        };

        _context.PurchaseRequests.Add(pr);
        await _context.SaveChangesAsync();

        return (await GetPRByIdAsync(pr.Id))!;
    }

    public async Task<PRResponseDto?> UpdatePRAsync(int id, UpdatePRDto dto)
    {
        var pr = await _context.PurchaseRequests
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(pr => pr.Id == id);

        if (pr == null) return null;
        if (pr.Status != "DRAFT")
            throw new Exception("PR hanya bisa diedit saat status DRAFT");

        if (dto.Items == null || !dto.Items.Any())
            throw new Exception("Item PR wajib diisi");

        pr.Department = dto.Department;
        pr.Pic = dto.Pic;
        pr.NeededDate = dto.NeededDate;

        // Hapus items lama, replace dengan yang baru
        _context.PurchaseRequestItems.RemoveRange(pr.Items);
        pr.Items = dto.Items.Select(i => new PurchaseRequestItem
        {
            ItemName = i.ItemName,
            Qty = i.Qty,
            Unit = i.Unit,
            Price = i.Price,
            Reason = i.Reason,
            PurchaseRequestId = id,
        }).ToList();

        await _context.SaveChangesAsync();

        return await GetPRByIdAsync(id);
    }

    public async Task<PRResponseDto?> UpdateStatusAsync(int id, UpdatePRStatusDto dto)
    {
        var pr = await _context.PurchaseRequests
            .Include(pr => pr.Items)
            .FirstOrDefaultAsync(pr => pr.Id == id);

        if (pr == null) return null;

        if (dto.Status == "WAITING_APPROVAL" && pr.Status != "DRAFT")
            throw new Exception("PR hanya bisa diajukan dari status DRAFT");

        if ((dto.Status == "APPROVED" || dto.Status == "REJECTED") &&
            pr.Status != "WAITING_APPROVAL")
            throw new Exception("PR hanya bisa diproses setelah WAITING_APPROVAL");

        pr.Status = dto.Status;
        pr.Comment = dto.Comment;

        await _context.SaveChangesAsync();

        return MapToDto(pr);
    }

    public async Task<bool> DeletePRAsync(int id)
    {
        var pr = await _context.PurchaseRequests.FindAsync(id);
        if (pr == null) return false;
        if (pr.Status != "DRAFT")
            throw new Exception("PR hanya bisa dihapus saat status DRAFT");

        _context.PurchaseRequests.Remove(pr);
        await _context.SaveChangesAsync();
        return true;
    }
}
