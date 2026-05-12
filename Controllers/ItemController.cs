// Controllers/ItemController.cs
using Microsoft.AspNetCore.Mvc;
using backend_inventory.DTOs.Item;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _itemService.GetAllItemsAsync();
        return Ok(result);
    }

    [HttpGet("approved-by-category/{categoryId}")]
    public async Task<IActionResult> GetApprovedByCategory(int categoryId)
    {
        var result = await _itemService.GetApprovedItemsByCategoryAsync(categoryId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _itemService.GetItemByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                message = "Item tidak ditemukan"
            });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _itemService.CreateItemAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _itemService.UpdateItemAsync(id, dto);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Item tidak ditemukan"
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateItemStatusDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _itemService.UpdateStatusAsync(id, dto);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Item tidak ditemukan"
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _itemService.DeleteItemAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Item tidak ditemukan"
                });
            }

            return Ok(new
            {
                message = "Item berhasil dihapus"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}