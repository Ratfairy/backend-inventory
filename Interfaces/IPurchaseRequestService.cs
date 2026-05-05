using backend_inventory.DTOs.PurchaseRequest;

namespace backend_inventory.Interfaces;

public interface IPurchaseRequestService
{
    Task<IEnumerable<PRResponseDto>> GetAllPRsAsync();
    Task<PRResponseDto?> GetPRByIdAsync(int id);
    Task<PRResponseDto> CreatePRAsync(CreatePRDto dto);
    Task<PRResponseDto?> UpdatePRAsync(int id, UpdatePRDto dto);
    Task<PRResponseDto?> UpdateStatusAsync(int id, UpdatePRStatusDto dto);
    Task<bool> DeletePRAsync(int id);
}