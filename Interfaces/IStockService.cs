using backend_inventory.DTOs.Stock;

namespace backend_inventory.Interfaces;

public interface IStockService
{
    // Stock
    Task<IEnumerable<StockResponseDto>> GetAllStocksAsync();
    Task<StockResponseDto?> GetStockByIdAsync(int id);
    Task<StockResponseDto> CreateStockAsync(CreateStockDto dto);
    Task<StockResponseDto?> UpdateStockAsync(int id, UpdateStockDto dto);
    Task<bool> DeleteStockAsync(int id);

    // Stock Movement
    Task<IEnumerable<StockMovementResponseDto>> GetAllMovementsAsync();
    Task<IEnumerable<StockMovementResponseDto>> GetMovementsByStockIdAsync(int stockId);
    Task<StockMovementResponseDto> CreateMovementAsync(CreateStockMovementDto dto);
}