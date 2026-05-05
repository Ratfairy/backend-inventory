using Microsoft.AspNetCore.Mvc;
using backend_inventory.DTOs.Stock;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    // =====================
    // STOCK
    // =====================

    // GET api/stock
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _stockService.GetAllStocksAsync();
        return Ok(result);
    }

    // GET api/stock/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _stockService.GetStockByIdAsync(id);
        if (result == null) return NotFound(new { message = "Stock tidak ditemukan" });
        return Ok(result);
    }

    // POST api/stock
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _stockService.CreateStockAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/stock/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStockDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _stockService.UpdateStockAsync(id, dto);
            if (result == null) return NotFound(new { message = "Stock tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE api/stock/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _stockService.DeleteStockAsync(id);
            if (!result) return NotFound(new { message = "Stock tidak ditemukan" });
            return Ok(new { message = "Stock berhasil dihapus" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // =====================
    // STOCK MOVEMENT
    // =====================

    // GET api/stock/movement
    [HttpGet("movement")]
    public async Task<IActionResult> GetAllMovements()
    {
        var result = await _stockService.GetAllMovementsAsync();
        return Ok(result);
    }

    // GET api/stock/5/movement
    [HttpGet("{stockId}/movement")]
    public async Task<IActionResult> GetMovementsByStockId(int stockId)
    {
        var result = await _stockService.GetMovementsByStockIdAsync(stockId);
        return Ok(result);
    }

    // POST api/stock/movement
    [HttpPost("movement")]
    public async Task<IActionResult> CreateMovement([FromBody] CreateStockMovementDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _stockService.CreateMovementAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}