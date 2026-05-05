using Microsoft.AspNetCore.Mvc;
using backend_inventory.DTOs.PurchaseOrder;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderController : ControllerBase
{
    private readonly IPurchaseOrderService _poService;

    public PurchaseOrderController(IPurchaseOrderService poService)
    {
        _poService = poService;
    }

    // GET api/purchaseorder
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _poService.GetAllPOsAsync();
        return Ok(result);
    }

    // GET api/purchaseorder/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _poService.GetPOByIdAsync(id);
        if (result == null) return NotFound(new { message = "Purchase Order tidak ditemukan" });
        return Ok(result);
    }

    // GET api/purchaseorder/approved-prs
    // Ambil PR yang sudah APPROVED dan belum punya PO
    [HttpGet("approved-prs")]
    public async Task<IActionResult> GetApprovedPRs()
    {
        var result = await _poService.GetApprovedPRsForPOAsync();
        return Ok(result);
    }

    // POST api/purchaseorder
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePODto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _poService.CreatePOAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/purchaseorder/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePODto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _poService.UpdatePOAsync(id, dto);
            if (result == null) return NotFound(new { message = "Purchase Order tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PATCH api/purchaseorder/5/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdatePOStatusDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _poService.UpdateStatusAsync(id, dto);
            if (result == null) return NotFound(new { message = "Purchase Order tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE api/purchaseorder/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _poService.DeletePOAsync(id);
            if (!result) return NotFound(new { message = "Purchase Order tidak ditemukan" });
            return Ok(new { message = "Purchase Order berhasil dihapus" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}