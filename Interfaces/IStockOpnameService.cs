using backend_inventory.DTOs.StockOpname;

namespace backend_inventory.Interfaces;

public interface IStockOpnameService
{
    Task<IEnumerable<StockOpnameResponseDto>> GetAllOpnamesAsync();
    Task<StockOpnameResponseDto?> GetOpnameByIdAsync(int id);
    Task<StockOpnameResponseDto> CreateOpnameAsync(CreateStockOpnameDto dto);
    Task<StockOpnameResponseDto?> UpdateStatusAsync(int id, UpdateStockOpnameStatusDto dto);
    Task<bool> DeleteOpnameAsync(int id);
}