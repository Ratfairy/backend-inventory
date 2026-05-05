using backend_inventory.DTOs.PurchaseOrder;

namespace backend_inventory.Interfaces;

public interface IPurchaseOrderService
{
    Task<IEnumerable<POResponseDto>> GetAllPOsAsync();
    Task<POResponseDto?> GetPOByIdAsync(int id);
    Task<IEnumerable<POResponseDto>> GetApprovedPRsForPOAsync();
    Task<POResponseDto> CreatePOAsync(CreatePODto dto);
    Task<POResponseDto?> UpdatePOAsync(int id, UpdatePODto dto);
    Task<POResponseDto?> UpdateStatusAsync(int id, UpdatePOStatusDto dto);
    Task<bool> DeletePOAsync(int id);
}