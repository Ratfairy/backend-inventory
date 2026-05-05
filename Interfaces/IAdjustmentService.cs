using backend_inventory.DTOs.Adjustment;

namespace backend_inventory.Interfaces;

public interface IAdjustmentService
{
    Task<IEnumerable<AdjustmentResponseDto>> GetAllAdjustmentsAsync();
    Task<AdjustmentResponseDto?> GetAdjustmentByIdAsync(int id);
    Task<AdjustmentResponseDto> CreateAdjustmentAsync(CreateAdjustmentDto dto);
    Task<AdjustmentResponseDto?> UpdateStatusAsync(int id, UpdateAdjustmentStatusDto dto);
    Task<bool> DeleteAdjustmentAsync(int id);
}