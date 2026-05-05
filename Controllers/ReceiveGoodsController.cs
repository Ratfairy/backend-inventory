using Microsoft.AspNetCore.Mvc;
using backend_inventory.DTOs.ReceiveGoods;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiveGoodsController : ControllerBase
{
    private readonly IReceiveGoodsService _receiveGoodsService;

    public ReceiveGoodsController(IReceiveGoodsService receiveGoodsService)
    {
        _receiveGoodsService = receiveGoodsService;
    }

    // GET api/receivegoods
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _receiveGoodsService.GetAllReceiveGoodsAsync();
        return Ok(result);
    }

    // GET api/receivegoods/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _receiveGoodsService.GetReceiveGoodsByIdAsync(id);
        if (result == null) return NotFound(new { message = "Receive Goods tidak ditemukan" });
        return Ok(result);
    }

    // POST api/receivegoods/5/confirm
    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> Confirm(int id, [FromBody] ConfirmReceiveDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _receiveGoodsService.ConfirmReceiveAsync(id, dto);
            if (result == null) return NotFound(new { message = "Receive Goods tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}