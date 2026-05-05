using Microsoft.AspNetCore.Mvc;
using backend_inventory.DTOs.PurchaseRequest;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseRequestController : ControllerBase
{
    private readonly IPurchaseRequestService _prService;

    public PurchaseRequestController(IPurchaseRequestService prService)
    {
        _prService = prService;
    }

    // GET api/purchaserequest
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _prService.GetAllPRsAsync();
        return Ok(result);
    }

    // GET api/purchaserequest/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _prService.GetPRByIdAsync(id);
        if (result == null) return NotFound(new { message = "Purchase Request tidak ditemukan" });
        return Ok(result);
    }

    // POST api/purchaserequest
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePRDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _prService.CreatePRAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/purchaserequest/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePRDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _prService.UpdatePRAsync(id, dto);
            if (result == null) return NotFound(new { message = "Purchase Request tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PATCH api/purchaserequest/5/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdatePRStatusDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _prService.UpdateStatusAsync(id, dto);
            if (result == null) return NotFound(new { message = "Purchase Request tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE api/purchaserequest/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _prService.DeletePRAsync(id);
            if (!result) return NotFound(new { message = "Purchase Request tidak ditemukan" });
            return Ok(new { message = "Purchase Request berhasil dihapus" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}