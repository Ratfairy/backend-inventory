using Microsoft.AspNetCore.Mvc;
using backend_inventory.DTOs.Adjustment;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdjustmentController : ControllerBase
{
    private readonly IAdjustmentService _adjustmentService;

    public AdjustmentController(IAdjustmentService adjustmentService)
    {
        _adjustmentService = adjustmentService;
    }

    // GET api/adjustment
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _adjustmentService.GetAllAdjustmentsAsync();
        return Ok(result);
    }

    // GET api/adjustment/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _adjustmentService.GetAdjustmentByIdAsync(id);
        if (result == null) return NotFound(new { message = "Adjustment tidak ditemukan" });
        return Ok(result);
    }

    // POST api/adjustment
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAdjustmentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _adjustmentService.CreateAdjustmentAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PATCH api/adjustment/5/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAdjustmentStatusDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _adjustmentService.UpdateStatusAsync(id, dto);
            if (result == null) return NotFound(new { message = "Adjustment tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE api/adjustment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _adjustmentService.DeleteAdjustmentAsync(id);
            if (!result) return NotFound(new { message = "Adjustment tidak ditemukan" });
            return Ok(new { message = "Adjustment berhasil dihapus" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}