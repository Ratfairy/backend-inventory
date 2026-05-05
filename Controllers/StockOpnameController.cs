using Microsoft.AspNetCore.Mvc;
using backend_inventory.DTOs.StockOpname;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockOpnameController : ControllerBase
{
    private readonly IStockOpnameService _opnameService;

    public StockOpnameController(IStockOpnameService opnameService)
    {
        _opnameService = opnameService;
    }

    // GET api/stockopname
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _opnameService.GetAllOpnamesAsync();
        return Ok(result);
    }

    // GET api/stockopname/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _opnameService.GetOpnameByIdAsync(id);
        if (result == null) return NotFound(new { message = "Stock Opname tidak ditemukan" });
        return Ok(result);
    }

    // POST api/stockopname
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockOpnameDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _opnameService.CreateOpnameAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PATCH api/stockopname/5/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStockOpnameStatusDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _opnameService.UpdateStatusAsync(id, dto);
            if (result == null) return NotFound(new { message = "Stock Opname tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE api/stockopname/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _opnameService.DeleteOpnameAsync(id);
            if (!result) return NotFound(new { message = "Stock Opname tidak ditemukan" });
            return Ok(new { message = "Stock Opname berhasil dihapus" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}