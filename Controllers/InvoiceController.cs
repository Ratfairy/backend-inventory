using Microsoft.AspNetCore.Mvc;
using backend_inventory.DTOs.Invoice;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    // GET api/invoice
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _invoiceService.GetAllInvoicesAsync();
        return Ok(result);
    }

    // GET api/invoice/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _invoiceService.GetInvoiceByIdAsync(id);
        if (result == null) return NotFound(new { message = "Invoice tidak ditemukan" });
        return Ok(result);
    }

    // GET api/invoice/received-pos
    // Ambil ReceiveGoods yang RECEIVED dan belum punya Invoice
    [HttpGet("received-pos")]
    public async Task<IActionResult> GetReceivedPOs()
    {
        var result = await _invoiceService.GetReceivedPOsForInvoiceAsync();
        return Ok(result);
    }

    // POST api/invoice
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _invoiceService.CreateInvoiceAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PATCH api/invoice/5/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateInvoiceStatusDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _invoiceService.UpdateStatusAsync(id, dto);
            if (result == null) return NotFound(new { message = "Invoice tidak ditemukan" });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE api/invoice/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _invoiceService.DeleteInvoiceAsync(id);
            if (!result) return NotFound(new { message = "Invoice tidak ditemukan" });
            return Ok(new { message = "Invoice berhasil dihapus" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}