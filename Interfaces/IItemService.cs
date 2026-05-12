// Interfaces/IItemService.cs
using backend_inventory.DTOs.Item;

namespace backend_inventory.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();

    Task<IEnumerable<ItemResponseDto>> GetApprovedItemsByCategoryAsync(int categoryId);

    Task<ItemResponseDto?> GetItemByIdAsync(int id);

    Task<ItemResponseDto> CreateItemAsync(CreateItemDto dto);

    Task<ItemResponseDto?> UpdateItemAsync(
        int id,
        UpdateItemDto dto
    );

    Task<ItemResponseDto?> UpdateStatusAsync(
        int id,
        UpdateItemStatusDto dto
    );

    Task<bool> DeleteItemAsync(int id);
}