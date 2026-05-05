using backend_inventory.DTOs.ReceiveGoods;

namespace backend_inventory.Interfaces;

public interface IReceiveGoodsService
{
    Task<IEnumerable<ReceiveGoodsResponseDto>> GetAllReceiveGoodsAsync();
    Task<ReceiveGoodsResponseDto?> GetReceiveGoodsByIdAsync(int id);
    Task<ReceiveGoodsResponseDto?> ConfirmReceiveAsync(int id, ConfirmReceiveDto dto);
}